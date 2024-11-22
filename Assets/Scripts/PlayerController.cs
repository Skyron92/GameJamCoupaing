using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] InputActionReference moveActionRef, sprintActionRef;
    InputAction MoveAction => moveActionRef.action;
    InputAction SprintAction => sprintActionRef.action;
    Vector2 MoveInput => MoveAction.ReadValue<Vector2>();
    
    CharacterController _characterController;
    [SerializeField, Range(1,100)] float speed = 10;
    [SerializeField, Range(1,100)] float sprintBoost = 5;
    
    public delegate void MoveDelegate(float speed);
    public event MoveDelegate moved, moveUpdate, stopped;


    public Incromate test;

    private void Awake() {
        _characterController = GetComponent<CharacterController>();
        test.SetPlayerAndBindMovement(this);
    }

    private void OnEnable() {
        MoveAction.Enable();
        MoveAction.started += OnMoveActionStarted;
        MoveAction.canceled += OnMoveActionCanceled;
        SprintAction.Enable();
        SprintAction.started += OnSprintActionStarted;
        sprintActionRef.action.canceled += OnSprintActionCanceled;
    }

    private void OnDisable() {
        MoveAction.Disable();
        MoveAction.started -= OnMoveActionStarted;
        MoveAction.canceled -= OnMoveActionCanceled;
        SprintAction.Disable();
        SprintAction.started -= OnSprintActionStarted;
        sprintActionRef.action.canceled -= OnSprintActionCanceled;
    }

    private void OnMoveActionStarted(InputAction.CallbackContext obj) {
        StartCoroutine(Move());
        moved?.Invoke(speed);
    }
    
    private void OnMoveActionCanceled(InputAction.CallbackContext obj) {
        StopAllCoroutines();
        stopped?.Invoke(0);
    }

    private void OnSprintActionStarted(InputAction.CallbackContext obj) {
        speed += sprintBoost;
        moved?.Invoke(speed);
    }
    
    private void OnSprintActionCanceled(InputAction.CallbackContext obj) {
        speed -= sprintBoost;
    }

    IEnumerator Move() {
        while (MoveInput != Vector2.zero) {
            var direction = new Vector3(MoveInput.x, 0, MoveInput.y);
            direction *= speed * Time.deltaTime;
            _characterController?.Move(direction);
            moveUpdate?.Invoke(0);
            yield return null;
        }
    }
}
