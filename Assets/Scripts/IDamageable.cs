public interface IDamageable
{
    bool IsDead { get; }
    bool IsImmortal { get; set; }
    int MaxLives { get; set; }
    int _CurrentLive { get; }
    void TakeDamage(int value = 1);
    void Heal(int value);
    void FullRestore();
    void Die();
}