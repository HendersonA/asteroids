using UnityEngine;

public class Asteroid : MonoBehaviour
{
    [SerializeField] private float _speedMin = 1f;
    [SerializeField] private float _speedMax = 2f;
    [SerializeField] private float _speedRotationMin = 1f;
    [SerializeField] private float _speeRotationdMax = 2f;
    private Rigidbody2D _rigidBody;

    private void Awake()
    {
        _rigidBody = GetComponent<Rigidbody2D>();
    }
    private void Start()
    {
        SetAsteroidMovement();
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
