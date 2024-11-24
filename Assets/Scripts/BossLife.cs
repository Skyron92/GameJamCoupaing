using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class BossLife : Enemy
{
    [SerializeField] SliderManager healthSlider;
    private Renderer[] _renderers;
    private List<Material> _materials = new List<Material>();
    
    private void Awake() {
        healthSlider.maxValue = Health;
        _renderers = GetComponentsInChildren<Renderer>();
        foreach (var renderer in _renderers) {
            _materials.Add(renderer.materials[0]);
        }
    }
    
    public override void TakeDamage(int damageTaken) {
        base.TakeDamage(damageTaken);
        foreach (var mat in _materials) {
            mat.DOColor(Color.red, .2f).onComplete += () => mat.DOColor(Color.white, .2f);
        }
        healthSlider.SetSliderValue(Health);
    }

    public override void Die() {
        healthSlider.OutAnimation();
        base.Die();
    }
    
    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.CompareTag("Hitable")) {
            var target = other.gameObject.GetComponent<IHitable>();
            Attack(target, damage);
        }
    }
}
