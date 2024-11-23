using System.Collections;
using UnityEngine;

public class Spike : Trap {
    
    private Animator _animator;
    [SerializeField, Range(0, 10)] private float stateDuration = 2;
    private Collider _collider;

    private void Awake() {
        _animator = GetComponent<Animator>();
        _collider = GetComponent<Collider>();
        _collider.enabled = _animator.GetBool("Shown");
    }

    private void Start() {
        StartCoroutine(ManagesAnimation());
    }

    IEnumerator ManagesAnimation() {
        float time = 0;
        while (gameObject.activeInHierarchy) {
            time += Time.deltaTime;
            if (time >= stateDuration) {
                _animator.SetBool("Shown", !_animator.GetBool("Shown"));
                time = 0;
                _collider.enabled = _animator.GetBool("Shown");
            }
            yield return null;
        }
    }

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.CompareTag("Hitable")) {
            var target = other.gameObject.GetComponent<IHitable>();
            Attack(target, damage);
        }
    }
}