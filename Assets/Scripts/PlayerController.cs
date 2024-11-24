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
    [Header("Movement")]
    [SerializeField, Range(1,100)] float speed = 50;
    public float Speed => speed;
    [SerializeField, Range(1,100)] float sprintBoost = 10;
    [SerializeField] GameObject sprintEffect;
    
    public delegate void MoveDelegate(float speed);
    public event MoveDelegate moved, moveUpdate, stopped;
    
    [SerializeField] private CameraScript cameraScript;

    [SerializeField] private Vector2Int _movementMode = new Vector2Int(1, 0);
    
    public Animator animator;
    [SerializeField] private FootstepSoundManager SoundManager;
    
    [Header("Health")]
    
    [SerializeField, Range(0, 5)] private int health = 1;
    [SerializeField] SliderManager healthSlider;
    private Renderer[] _renderers;
    private List<Material> _materials = new List<Material>();

    private int Health {
        get => health;
        set => health = value < 0 ? 0 : value;
    }
    
    private void Awake() {
        _characterController = GetComponent<CharacterController>();
        healthSlider.maxValue = Health;
        _renderers = GetComponentsInChildren<Renderer>();
        foreach (var renderer in _renderers) {
            _materials.Add(renderer.materials[0]);
        }
    }

    private void OnEnable() {
        MoveAction.Enable();
        MoveAction.started += OnMoveActionStarted;
        MoveAction.canceled += OnMoveActionCanceled;
        SprintAction.Enable();
        SprintAction.started += OnSprintActionStarted;
        SprintAction.canceled += OnSprintActionCanceled;
        AttractAction.Enable();
        OrderAction.Enable();
        MousePosAction.Enable();
    }

    private void OnDisable() {
        MoveAction.Disable();
        MoveAction.started -= OnMoveActionStarted;
        MoveAction.canceled -= OnMoveActionCanceled;
        SprintAction.Disable();
        SprintAction.started -= OnSprintActionStarted;
        sprintActionRef.action.canceled -= OnSprintActionCanceled;
        AttractAction.Disable();
        OrderAction.Disable();
        MousePosAction.Enable();
    }

    private void OnMoveActionStarted(InputAction.CallbackContext obj) {
        cameraScript?.CameraMoveHorizontally(MoveInput.x);
        StartCoroutine(Move());
        animator.SetBool("IsWalking", true);
        moved?.Invoke(speed);
    }
    
    private void OnMoveActionCanceled(InputAction.CallbackContext obj) {
        StopAllCoroutines();
        animator?.SetBool("IsWalking", false);
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
        animator.SetTrigger("Dead");
        healthSlider.OutAnimation();
        MoveAction.Disable();
        MoveAction.started -= OnMoveActionStarted;
        MoveAction.canceled -= OnMoveActionCanceled;
        SprintAction.Disable();
        SprintAction.started -= OnSprintActionStarted;
        sprintActionRef.action.canceled -= OnSprintActionCanceled;
        AttractAction.Disable();
        OrderAction.Disable();
        MousePosAction.Enable();
        enabled = false;
    }
}
