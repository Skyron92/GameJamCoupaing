using UnityEngine;

public class Enemy : Trap ,IHitable {
    
    [SerializeField, Range(0, 5)] private int health = 1;

    private int Health {
        get => health;
        set => health = value < 0 ? 0 : value;
    }
    
    public void TakeDamage(int damageTaken) {
        Health -= damageTaken;
        if(Health <= 0) Die();
    }

    public void Die() {
        Debug.Log(gameObject.name + " is dead");
        Destroy(gameObject);
    }
}