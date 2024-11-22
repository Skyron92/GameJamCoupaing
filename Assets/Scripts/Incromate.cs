using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

public class Incromate : MonoBehaviour { 

    NavMeshAgent _agent;
    private PlayerController _player;
    public PlayerController Player => _player;
    private IncromateFusionProvider _fusionProvider;
    
    private void Awake() {
        _agent = GetComponent<NavMeshAgent>();
        _fusionProvider = GetComponent<IncromateFusionProvider>();
        _fusionProvider.canMerge = false;
        _fusionProvider.SetIncromate(this);
        if (NavMesh.SamplePosition(transform.position, out NavMeshHit hit, 5, NavMesh.AllAreas)) {
            _agent.Warp(hit.position);
        }
    }

    private void OnAttractActionStarted(InputAction.CallbackContext obj) {
        _fusionProvider.canMerge = true;
    }

    private void OnAttractActionCanceled(InputAction.CallbackContext obj) {
        _fusionProvider.canMerge = false;
    }

    private void OnDisable() {
        if(!_player) return;
        _player.AttractAction.started -= OnAttractActionStarted;
        _player.AttractAction.canceled -= OnAttractActionCanceled;
    }

    /// <summary>
    /// Called when an incromate is harvested by the player
    /// </summary>
    /// <param name="player">Player ref</param>
    public void SetPlayerAndBindMovement(PlayerController player) {
        _player = player;
        _player.moved += OnPlayerMoved;
        _player.moveUpdate += OnPlayerMoveUpdate;
        _player.stopped += OnPlayerStopped;
        _player.AttractAction.started += OnAttractActionStarted;
        _player.AttractAction.canceled += OnAttractActionCanceled;
    }

    /// <summary>
    /// Called when move started
    /// </summary>
    /// <param name="speed"></param>
    private void OnPlayerMoved(float speed) {
        StartCoroutine(SetSpeed(speed));
    }
    
    /// <summary>
    /// Called every iteration of movement
    /// </summary>
    /// <param name="speed"></param>
    private void OnPlayerMoveUpdate(float speed) {
        _agent?.SetDestination(_player.transform.position);
    }
    
    /// <summary>
    /// Called when move stopped
    /// </summary>
    /// <param name="speed"></param>
    private void OnPlayerStopped(float speed) {
        StartCoroutine(SetSpeed(0));
    }

    /// <summary>
    /// Manages acceleration / Deceleration
    /// </summary>
    /// <param name="targetSpeed"></param>
    /// <returns></returns>
    IEnumerator SetSpeed(float targetSpeed) {
        if(_agent == null) yield break;
        int factor = _agent.speed < targetSpeed ? 1 : -1;
        while (!Mathf.Approximately(_agent.speed, targetSpeed)) {
            _agent.speed += 0.1f * factor;
            yield return null;
        }
    }

    private void OnDestroy() {
        if (_player == null) return;
        _player.moved -= OnPlayerMoved;
        _player.moveUpdate -= OnPlayerMoveUpdate;
        _player.stopped -= OnPlayerStopped;
    }
}