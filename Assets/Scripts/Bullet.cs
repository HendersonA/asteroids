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
        private void Awake()
        {
            _rigidbody2D = GetComponent<Rigidbody2D>();
            _collider = GetComponent<Collider2D>();
        }
        private IEnumerator AutoDestroyCoroutine()
        {
            yield return new WaitForSeconds(_secondsDestroy);
            gameObject.SetActive(false);
            _collider.enabled = false;
        }
        private void OnEnable()
        {
            _rigidbody2D.AddForce(-transform.up * _speed);
            StartCoroutine(AutoDestroyCoroutine());
        }
        private void OnTriggerEnter2D(Collider2D collision)
        {
            
        }
    }
}