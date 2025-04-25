using System.Collections;
using UnityEngine;

namespace Assets.Scripts
{
    [RequireComponent(typeof(BoxCollider2D))]
    public class Bullet : MonoBehaviour
    {
        [SerializeField] private float _speed = 2f;
        [SerializeField] private float _secondsDestroy = 2f;
        private Collider2D _collider;
        private Rigidbody2D _rigidbody2D;
        private LayerMask _layerMask;
        private void Awake()
        {
            _rigidbody2D = GetComponent<Rigidbody2D>();
            _collider = GetComponent<Collider2D>();
        }
        public void SetBullet(LayerMask layerMask, Transform startTransform, Vector2 direction)
        {
            _layerMask = layerMask;
            this.transform.position = startTransform.position; 
            this.transform.rotation = startTransform.rotation;
            _rigidbody2D.AddForce(direction * _speed);
        }
        private IEnumerator AutoDestroyCoroutine()
        {
            yield return new WaitForSeconds(_secondsDestroy);
            gameObject.SetActive(false);
        }
        private void Hit()
        {
            this.transform.position = Vector3.zero;
            this.transform.rotation = Quaternion.identity;
            _rigidbody2D.linearVelocity = Vector3.zero;
            _collider.enabled = false;
        }
        private void OnEnable()
        {
            _collider.enabled = true;
            StartCoroutine(AutoDestroyCoroutine());
        }
        private void OnDisable()
        {
            Hit();
        }
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.TryGetComponent(out IDamageable damageable) &&
            (_layerMask.value & (1 << collision.gameObject.layer)) != 0)
            {
                gameObject.SetActive(false);
                damageable.TakeDamage();
            }
        }
    }
}