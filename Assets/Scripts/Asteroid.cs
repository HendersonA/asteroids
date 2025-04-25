using UnityEngine;

public class Asteroid : MonoBehaviour
{
    [SerializeField] private int _amountFragments;
    [SerializeField] private GameObject _asteroidPrefab;

    [SerializeField] private float _speedMin = 1f;
    [SerializeField] private float _speedMax = 2f;
    [SerializeField] private float _speedRotationMin = 1f;
    [SerializeField] private float _speeRotationdMax = 2f;
    [SerializeField] private LayerMask _layerMask;
    private IDamageable _damageable;
    private Rigidbody2D _rigidBody;

    private void Awake()
    {
        _rigidBody = GetComponent<Rigidbody2D>();
        _damageable = GetComponent<IDamageable>();
    }
    private void OnEnable()
    {
        _damageable.FullRestore();
        SetAsteroidMovement();
    }
    private void OnDisable()
    {
        SpawnFragments();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out IDamageable damageable) &&
            (_layerMask.value & (1 << collision.gameObject.layer)) != 0)
        {
            damageable.TakeDamage();
        }
    }
    private void SpawnFragments()
    {
        for (int i = 0; i < _amountFragments; i++)
        {
            GameObject fragment = ObjectPool.Instance.Get(_asteroidPrefab);
            fragment.transform.position = this.transform.position;
            WaveManager.Instance.RegisterEnemy();
        }
    }
    public void RemoveFromWave()
    {
        WaveManager.Instance.EnemyDefeated();
    }
    [ContextMenu("SetAsteroidMovement")]
    private void SetAsteroidMovement()
    {
        var randomRotationSpeed = Random.Range(_speedRotationMin, _speeRotationdMax);
        var randomSpeed = Random.Range(_speedMin, _speedMax);
        var directionX = Random.Range(-1, 1);
        var directiony = Random.Range(-1, 1);
        var force = new Vector2(directionX, directiony) * randomSpeed;
        _rigidBody.linearVelocity = force;
        _rigidBody.angularVelocity = randomRotationSpeed;
    }
}
