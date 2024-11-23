using UnityEngine;

public class Enemy : Trap ,IHitable {
    
    [SerializeField, Range(0, 100)] private int health = 1;

    protected int Health {
        get => health;
        set => health = value < 0 ? 0 : value;
    }
    
    public virtual void TakeDamage(int damageTaken) {
        Health -= damageTaken;
        if(Health <= 0) Die();
    }

    public virtual void Die() {
        Destroy(gameObject);
    }
}