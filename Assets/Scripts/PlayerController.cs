using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class PlayerController : MonoBehaviour
{
    [SerializeField] InputActionReference moveActionRef, sprintActionRef;
    InputAction MoveAction => moveActionRef.action;
    InputAction SprintAction => sprintActionRef.action;
    Vector2 MoveInput => MoveAction.ReadValue<Vector2>();
    
    CharacterController _characterController;
    [SerializeField, Range(1,100)] float speed = 10;
    [SerializeField, Range(1,100)] float sprintBoost = 5;

    private void Awake() {
        _characterController = GetComponent<CharacterController>();
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
    }
    
    private void OnMoveActionCanceled(InputAction.CallbackContext obj) {
        StopAllCoroutines();
    }

    private void OnSprintActionStarted(InputAction.CallbackContext obj) {
        speed += sprintBoost;
    }
    
    private void OnSprintActionCanceled(InputAction.CallbackContext obj) {
        speed -= sprintBoost;
    }

    IEnumerator Move() {
        while (MoveInput != Vector2.zero) {
            var direction = new Vector3(MoveInput.x, 0, MoveInput.y);
            direction *= speed * Time.deltaTime;
            _characterController?.Move(direction);
            yield return null;
        }
    }
}
