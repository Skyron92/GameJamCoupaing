using UnityEngine;

public class Trap : MonoBehaviour, IAttacker
{
    [SerializeField, Range(0, 5)] protected int damage = 1;
    
    
    public void Attack(IHitable target, int damageDone) {
        target.TakeDamage(damageDone);
    }
}