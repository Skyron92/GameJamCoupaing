using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class Incromate : MonoBehaviour { 

    NavMeshAgent _agent;
    private PlayerController _player;

    private void Awake() {
        _agent = GetComponent<NavMeshAgent>();
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
        int factor = _agent.speed < targetSpeed ? 1 : -1;
        while (!Mathf.Approximately(_agent.speed, targetSpeed)) {
            _agent.speed += 0.1f * factor;
            yield return null;
        }
    }

    private void OnDestroy() {
        if (_player == null) return;
        _player.moved -= OnPlayerMoved;
        _player.stopped -= OnPlayerStopped;
    }
}