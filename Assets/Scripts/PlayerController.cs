using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour, IHitable
{
    [Header("Inputs")] [SerializeField] private InputActionReference moveActionRef;
    [SerializeField] InputActionReference sprintActionRef, attractActionRef, orderActionRef, mousePosActionRef, rollActionRef;
    InputAction MoveAction => moveActionRef.action;
    InputAction SprintAction => sprintActionRef.action;
    public InputAction AttractAction => attractActionRef.action;
    public InputAction OrderAction => orderActionRef.action;
    private InputAction MousePosAction => mousePosActionRef.action;
    public Vector2 MousePosInput => MousePosAction.ReadValue<Vector2>();
    Vector2 MoveInput => MoveAction.ReadValue<Vector2>();
    
    CharacterController _characterController;
    private Rigidbody _rigidbody;
    [Header("Movement")]
    [SerializeField, Range(1,100)] float speed = 50;
    public float Speed => speed;
    [SerializeField, Range(1,100)] float sprintBoost = 10;
    [SerializeField] GameObject pathEffect ,sprintEffect;
    
    public delegate void MoveDelegate(float speed);
    public event MoveDelegate moved, moveUpdate, stopped;
    
    [SerializeField] private CameraScript cameraScript;

    [SerializeField] private Vector2Int _movementMode = new Vector2Int(1, 0);
    
    public Animator animator;
    [SerializeField] private FootstepSoundManager SoundManager;
    
    [Header("Health")]
    
    [SerializeField, Range(0, 5)] private int health = 1;
    [SerializeField] SliderManager healthSlider;
    [SerializeField] private GameObject gameOverPanel;
    private Renderer[] _renderers;
    private List<Material> _materials = new List<Material>();

    [Header("Incromates")] [SerializeField]
    private GameObject waveEffect;
    [SerializeField] private GameObject orderEffect;
    private GameObject _orderEffectInstance;
    private Vector3 _orderPosition;
    [SerializeField] private LayerMask groundLayerMask;

    private int Health {
        get => health;
        set => health = value < 0 ? 0 : value;
    }
    
    private void Awake() {
        _characterController = GetComponent<CharacterController>();
        _rigidbody = GetComponent<Rigidbody>();
        healthSlider.maxValue = Health;
        _renderers = GetComponentsInChildren<Renderer>();
        foreach (var renderer in _renderers) {
            _materials.Add(renderer.materials[0]);
        }
    }

    private void OnEnable() {
        BindInputs();
    }

    private void OnDisable() {
        UnbindInputs();
    }
    
    private void BindInputs() {
        MoveAction.Enable();
        MoveAction.started += OnMoveActionStarted;
        MoveAction.canceled += OnMoveActionCanceled;
        SprintAction.Enable();
        SprintAction.started += OnSprintActionStarted;
        SprintAction.canceled += OnSprintActionCanceled;
        AttractAction.Enable();
        AttractAction.started += OnAttractActionStated;
        AttractAction.canceled += OnAttractActionCanceled;
        OrderAction.Enable();
        OrderAction.started += OnOrderActionStarted;
        OrderAction.canceled += OnOrderActionCanceled;
        MousePosAction.Enable();
    }

    private void UnbindInputs() {
        MoveAction.Disable();
        MoveAction.started -= OnMoveActionStarted;
        MoveAction.canceled -= OnMoveActionCanceled;
        SprintAction.Disable();
        SprintAction.started -= OnSprintActionStarted;
        sprintActionRef.action.canceled -= OnSprintActionCanceled;
        AttractAction.Disable();
        AttractAction.started -= OnAttractActionStated;
        AttractAction.canceled -= OnAttractActionCanceled;
        OrderAction.Disable();
        OrderAction.started -= OnOrderActionStarted;
        OrderAction.canceled -= OnOrderActionCanceled;
        MousePosAction.Disable();
    }

    private void OnMoveActionStarted(InputAction.CallbackContext obj) {
        cameraScript?.CameraMoveHorizontally(MoveInput.x);
        StartCoroutine(Move());
        animator.SetBool("IsWalking", true);
        pathEffect.SetActive(true);
        moved?.Invoke(speed);
    }
    
    private void OnMoveActionCanceled(InputAction.CallbackContext obj) {
        StopAllCoroutines();
        animator?.SetBool("IsWalking", false);
        pathEffect.SetActive(false);
        stopped?.Invoke(0);
    }

    private void OnSprintActionStarted(InputAction.CallbackContext obj) {
        speed += sprintBoost;
        if (!MoveAction.IsPressed()) return;
        animator?.SetBool("IsRunning", true);
        SoundManager.isRunning = true;
        sprintEffect.SetActive(true);
        moved?.Invoke(speed);
    }
    
    private void OnSprintActionCanceled(InputAction.CallbackContext obj) {
        speed -= sprintBoost;
        animator?.SetBool("IsRunning", false);
        SoundManager.isRunning = false;
        sprintEffect.SetActive(false);
    }
    
    private void OnAttractActionStated(InputAction.CallbackContext obj) {
        waveEffect.SetActive(true);
    }
    
    private void OnAttractActionCanceled(InputAction.CallbackContext obj) {
        waveEffect.SetActive(false);
    }

    private void OnOrderActionStarted(InputAction.CallbackContext obj) {
        var screenPos = MousePosInput;
        var pos = new Vector3(screenPos.x, screenPos.y, 100);
        if (Physics.Raycast(Camera.main.ScreenPointToRay(pos), out RaycastHit hit,
                1000, groundLayerMask)) {
            _orderPosition = hit.point;
            if (_orderEffectInstance != null) Destroy(_orderEffectInstance);
            var target = _orderPosition;
            target.y += .1f;
            _orderEffectInstance = Instantiate(orderEffect, target, Quaternion.identity);
        }
        else _orderPosition = Vector3.zero;
    }
    
    private void OnOrderActionCanceled(InputAction.CallbackContext obj) {
        if(_orderEffectInstance != null) Destroy(_orderEffectInstance);
        _orderPosition = Vector3.zero;
    }

    public Vector3 GetOrderPosition() {
        return _orderPosition;
    }

    IEnumerator Move() {
        while (MoveInput != Vector2.zero) {
            var direction = new Vector3(MoveInput.x, 0, MoveInput.y);
            direction = GetMovementMode(direction);
            direction *= speed * Time.deltaTime;
            _characterController?.Move(direction);
            float angle = Vector3.SignedAngle(Vector3.forward, direction, Vector3.up);
            Quaternion rotation = Quaternion.Euler(0,angle,0);
            transform.rotation = rotation;
            moveUpdate?.Invoke(0);
            yield return null;
        }
    }

    public void SetMovementMode(Vector2Int mode) {
        StopCoroutine(SetMovementModeWithDelay(mode, 1f));
        // Ok je vais changer la config de mes inputs :)
        StartCoroutine(SetMovementModeWithDelay(mode, 1f));
    }

    IEnumerator SetMovementModeWithDelay(Vector2Int mode, float delay) {
        // J'attend une seconde que la caméra ai le temps de faire son animation
        yield return new WaitForSeconds(delay);
        //!!!!!! Problème ici => J'ai pu recevoir une nouvelle demande de changement d'input, au quel cas j'arrête mon changement.
        // Je change la config de mes inputs
        _movementMode = mode;
    }

    private Vector3 GetMovementMode(Vector3 movement) {
       Vector3 newMovement = Vector3.zero;
       if (_movementMode.x != 0) {
           if (_movementMode.x > 0) newMovement = movement;
           else if (_movementMode.x < 0) newMovement = -movement;
           return newMovement;
       }
       else {
          if(_movementMode.y > 0) newMovement = new Vector3(-movement.z,0,movement.x); 
          else if(_movementMode.y < 0) newMovement = new Vector3(movement.z,0,-movement.x);
          return newMovement;
       }
    }

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.layer == LayerMask.NameToLayer("Incromate")) {
            var incro = other.gameObject.GetComponent<Incromate>();
            if(incro.isPicked) return;
            incro.SetPlayerAndBindMovement(this);
            incro.isPicked = true;
            animator.SetTrigger("Pickup");
        }
    }

    public void TakeDamage(int damage) {
        Health -= damage;
        foreach (var mat in _materials) {
            mat.DOColor(Color.red, .2f).onComplete += () => mat.DOColor(Color.white, .2f);
        }
        if(Health <= 0) Die();
        else {
            healthSlider.SetSliderValue(Health);
            transform.DOMove(transform.position + transform.forward * -.5f, .5f);
        }
    }

    public void Die() {
        Destroy(_rigidbody);
        animator.SetTrigger("Dead");
        healthSlider.OutAnimation();
        UnbindInputs();
        gameOverPanel.SetActive(true);
        Destroy(this);
    }
}