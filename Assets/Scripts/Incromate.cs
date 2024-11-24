using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

public class Incromate : MonoBehaviour, IHitable { 

    NavMeshAgent _agent;
    private PlayerController _player;
    public PlayerController Player => _player;
    private IncromateFusionProvider _fusionProvider;
    
    private Renderer _renderer;
    private Material _material;

    private int _health;
    
    [HideInInspector] public IncroAttackProvider incroAttack;

    [HideInInspector] public bool isPicked;
    
    public int Health {
        get => _health;
        set => _health = value < 0 ? 0 : value;
    }

    [SerializeField] private GameObject deathEffect;

    private void Awake() {
        _agent = GetComponent<NavMeshAgent>();
        _fusionProvider = GetComponent<IncromateFusionProvider>();
        _fusionProvider.canMerge = false;
        _fusionProvider.SetIncromate(this);
        if (NavMesh.SamplePosition(transform.position, out NavMeshHit hit, 5, NavMesh.AllAreas)) {
            _agent.Warp(hit.position);
        }
        _renderer = GetComponent<Renderer>();
        _material = _renderer.materials[0];
        incroAttack = GetComponent<IncroAttackProvider>();
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
        _player.OrderAction.started += OnOrderActionStarted;
        _player.OrderAction.canceled += OnOrderActionCanceled;
    }

    private void OnOrderActionStarted(InputAction.CallbackContext obj) {
        var pos= _player.GetOrderPosition();
        if(pos == Vector3.zero) return;
        StopAllCoroutines(); 
        _agent!.SetDestination(pos); 
        StartCoroutine(SetSpeed(_player.Speed)); 
        incroAttack.enabled = true;
        
    }
    
    private void OnOrderActionCanceled(InputAction.CallbackContext obj) {
        StopAllCoroutines();
        _agent!.SetDestination(_player.transform.position);
        incroAttack.enabled = true;
    }

    /// <summary>
    /// Called when move started
    /// </summary>
    /// <param name="speed"></param>
    private void OnPlayerMoved(float speed) {
        if(_agent == null) return;
        StopAllCoroutines();
        StartCoroutine(SetSpeed(speed));
    }
    
    /// <summary>
    /// Called every iteration of movement
    /// </summary>
    /// <param name="speed"></param>
    private void OnPlayerMoveUpdate(float speed) {
        if(_agent == null) return;
        _agent!.SetDestination(_player.transform.position);
    }
    
    /// <summary>
    /// Called when move stopped
    /// </summary>
    /// <param name="speed"></param>
    private void OnPlayerStopped(float speed) {
        if(_agent == null) return;
        StopAllCoroutines();
        StartCoroutine(SetSpeed(speed));
    }

    /// <summary>
    /// Manages acceleration / Deceleration
    /// </summary>
    /// <param name="targetSpeed"></param>
    /// <returns></returns>
    IEnumerator SetSpeed(float targetSpeed) {
        if(_agent == null) yield break;
        bool speedUp = _agent!.speed < targetSpeed;
        int factor = speedUp ? 1 : -1;
        while (!Mathf.Approximately(_agent!.speed, targetSpeed)) {
            _agent!.speed += 0.2f * factor;
            yield return null;
        }
    }
    
    private void OnAttractActionStarted(InputAction.CallbackContext obj) {
        _fusionProvider.canMerge = true;
        StartCoroutine(SetSpeed(_player.Speed * 2));
        StartCoroutine(Gather());
    }

    private void OnAttractActionCanceled(InputAction.CallbackContext obj) {
        _fusionProvider.canMerge = false;
        StopCoroutine(Gather());
        StartCoroutine(SetSpeed(0));
    }

    IEnumerator Gather() {
        while (_fusionProvider.canMerge) {
            _agent!.SetDestination(_player.transform.position);
            yield return new WaitForSeconds(.5f);
        }
    }
    
    private void OnDisable() {
        if(!_player) return;
        _player.moved -= OnPlayerMoved;
        _player.moveUpdate -= OnPlayerMoveUpdate;
        _player.stopped -= OnPlayerStopped;
        _player.AttractAction.started -= OnAttractActionStarted;
        _player.AttractAction.canceled -= OnAttractActionCanceled;
        _player.OrderAction.started -= OnOrderActionStarted;
        _player.OrderAction.canceled -= OnOrderActionCanceled;
    }
    
    private void OnDestroy() {
        if (_player == null) return;
        _player.moved -= OnPlayerMoved;
        _player.moveUpdate -= OnPlayerMoveUpdate;
        _player.stopped -= OnPlayerStopped;
        _player.AttractAction.started -= OnAttractActionStarted;
        _player.AttractAction.canceled -= OnAttractActionCanceled;
        _player.OrderAction.started -= OnOrderActionStarted;
        _player.OrderAction.canceled -= OnOrderActionCanceled;
    }
    
    public void TakeDamage(int damageTaken) {
        Health -= damageTaken;
        _material.DOColor(Color.red, .2f).onComplete += () => _material.DOColor(Color.white, .2f);
        if(Health <= 0) Die();
        else {
            transform.DOMove(transform.position + transform.forward * -.5f, .5f);
        }
    }
    
    public void Die() {
        Destroy(GetComponent<Collider>());
        StopAllCoroutines();
        TrySplit();
        Instantiate(deathEffect, transform.position, Quaternion.identity);
        transform.DOScale(Vector3.zero, .5f).onComplete += () => Destroy(gameObject);
    }

    private void TrySplit() {
        _fusionProvider.Split();
    }
}