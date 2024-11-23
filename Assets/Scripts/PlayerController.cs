using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour {
    [SerializeField] InputActionReference moveActionRef, sprintActionRef, attractActionRef, orderActionRef, mousePosActionRef, rollActionRef;
    InputAction MoveAction => moveActionRef.action;
    InputAction SprintAction => sprintActionRef.action;
    public InputAction AttractAction => attractActionRef.action;
    public InputAction OrderAction => orderActionRef.action;
    private InputAction MousePosAction => mousePosActionRef.action;
    public Vector2 MousePosInput => MousePosAction.ReadValue<Vector2>();
    Vector2 MoveInput => MoveAction.ReadValue<Vector2>();
    
    CharacterController _characterController;
    [SerializeField, Range(1,100)] float speed = 50;
    public float Speed => speed;
    [SerializeField, Range(1,100)] float sprintBoost = 10;
    
    public delegate void MoveDelegate(float speed);
    public event MoveDelegate moved, moveUpdate, stopped;
    
    [SerializeField] private CameraScript cameraScript;

    private Vector2Int _movementMode = new Vector2Int(0, 0);
    
    public Animator animator;
    private void Awake() {
        _characterController = GetComponent<CharacterController>();
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
        moved?.Invoke(speed);
    }
    
    private void OnSprintActionCanceled(InputAction.CallbackContext obj) {
        speed -= sprintBoost;
        animator?.SetBool("IsRunning", false);
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
        _movementMode = mode;
    }

    private Vector3 GetMovementMode(Vector3 movement) {
        bool moveFront = movement.x != 0;
        Vector3 newMovement = new Vector3(0,0,0);
        newMovement.x = moveFront ? movement.x : movement.z * _movementMode.y;
        newMovement.z = moveFront ? movement.z * _movementMode.x : movement.x * _movementMode.y;
        return newMovement;
    }

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.layer == LayerMask.NameToLayer("Incromate")) {
            var incro = other.gameObject.GetComponent<Incromate>();
            incro.SetPlayerAndBindMovement(this);
            animator.SetTrigger("Pickup");
        }
    }
}
