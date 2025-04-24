using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(BoxCollider2D))]
public class PlayerMovement : MonoBehaviour
{
    public static PlayerMovement Instance;
    [SerializeField] private float _maxSpeed = 100f;
    [SerializeField] private float thrust = 2f;
    [SerializeField] private float _rotationSpeed = 2f;
    private Rigidbody2D _rigidBody;

    private void Awake()
    {
        Instance = this;
        _rigidBody = GetComponent<Rigidbody2D>();
    }
    private void Update()
    {
        if (Input.GetKey(KeyCode.UpArrow))
        {
            var force = -transform.up * thrust * Time.deltaTime;
            _rigidBody.AddForce(force, ForceMode2D.Impulse);
            if (_rigidBody.linearVelocity.magnitude > _maxSpeed)
            {
                _rigidBody.linearVelocity = _rigidBody.linearVelocity.normalized * _maxSpeed;
            }
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            transform.Rotate(0, 0.0f, _rotationSpeed + transform.position.z, Space.Self);
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            transform.Rotate(0, 0.0f, -_rotationSpeed + transform.position.z, Space.Self);
        }
    }
}
