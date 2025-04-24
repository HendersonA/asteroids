using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour, IDamageable
{
    [Header("Events")]
    public UnityEvent OnTakeDamage;
    public UnityEvent OnHealDamage;
    public UnityEvent OnDeath;

    public bool IsDead { get; private set; }
    [field: SerializeField] public int MaxLives { get; set; }
    [field: SerializeField] public int _CurrentLive { get; private set; }

    private void Start()
    {
        Initialize();
    }
    public void Initialize()
    {
        _CurrentLive = MaxLives;
        IsDead = false;
    }
    public void TakeDamage(int value = 1)
    {
        if (IsDead)
            return;
        _CurrentLive -= value;
        if (_CurrentLive <= 0)
        {
            Die();
            return;
        }
        if (_CurrentLive > 0)
            OnTakeDamage.Invoke();
    }
    public void Heal(int value)
    {
        if (IsDead)
            return;
        _CurrentLive += value;

        if (_CurrentLive > MaxLives)
            _CurrentLive = MaxLives;

        if (_CurrentLive >= 0)
            OnHealDamage.Invoke();
    }
    [ContextMenu("Die")]
    public void Die()
    {
        _CurrentLive = 0;
        IsDead = true;
        OnDeath?.Invoke();
    }
    public void SetHealth(int newHealth) => _CurrentLive = newHealth;
    public void FullRestore()
    {
        _CurrentLive = MaxLives;
        IsDead = false;
    }
}