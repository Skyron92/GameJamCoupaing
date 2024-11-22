using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] InputActionReference moveActionRef;
    InputAction MoveAction => moveActionRef.action;
    Vector2 MoveInput => MoveAction.ReadValue<Vector2>();
    
    CharacterController _characterController;
    [SerializeField, Range(1,100)] float speed = 10;

    private void Awake() {
        _characterController = GetComponent<CharacterController>();
    }

    private void OnEnable() {
        MoveAction.Enable();
        MoveAction.started += OnMoveActionStarted;
        MoveAction.canceled += OnMoveActionCanceled;
    }

    private void OnDisable() {
        MoveAction.Disable();
        MoveAction.started -= OnMoveActionStarted;
        MoveAction.canceled -= OnMoveActionCanceled;
    }

    private void OnMoveActionStarted(InputAction.CallbackContext obj) {
        StartCoroutine(Move());
    }
    
    private void OnMoveActionCanceled(InputAction.CallbackContext obj) {
        StopAllCoroutines();
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
