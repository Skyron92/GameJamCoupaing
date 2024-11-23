public interface IAttacker {
    public void Attack(IHitable target, int damage);
}

public interface IHitable {
    public void TakeDamage(int damage);
    public void Die();
}