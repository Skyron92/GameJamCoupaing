using UnityEngine;

public class IncroAttackProvider : MonoBehaviour {
    
    int _damage;
    public int Damage { set => _damage = value; }
    
    private void OnTriggerEnter(Collider other) {
        var otherObj = other.gameObject;
        if (otherObj.CompareTag("Hitable")) {
            if(otherObj.layer == LayerMask.NameToLayer("Incromate") || otherObj.layer == LayerMask.NameToLayer("Player")) return;
            var target = otherObj.GetComponent<IHitable>();
            target.TakeDamage(_damage);
        }
    }
}