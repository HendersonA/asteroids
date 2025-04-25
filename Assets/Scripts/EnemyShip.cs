using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(Rigidbody2D), typeof(BoxCollider2D))]
public class EnemyShip : MonoBehaviour
{
    [SerializeField] private float _secondsAppear = 3f;
    [SerializeField] private float _speed = 2f;
    [SerializeField] private ScreenWrap screenWrap;
    [SerializeField] private Gun _gun;
    private Rigidbody2D _rigidBody;
    private Vector2 _startOrientation;
    private float _buffer = 0.5f;

    private void Awake()
    {
        _rigidBody = GetComponent<Rigidbody2D>();
    }
    private void OnEnable()
    {
        SetStartPosition();
        StartCoroutine(WaitToStartCoroutine());
    }
    private void OnDisable()
    {
        StopAllCoroutines();
    }
    private IEnumerator WaitToStartCoroutine()
    {
        yield return new WaitForSeconds(_secondsAppear);
        StartCoroutine(ShootRandomOverTime());
        EnemyMovement();
    }
    private void SetStartPosition()
    {
        var randomHeight = Random.Range(-screenWrap._screenHeight, screenWrap._screenHeight);
        var randomWidth = Random.Range(0f, 1f) <= .5f ? screenWrap._screenWidth : -screenWrap._screenWidth;
        transform.position = new Vector3(randomWidth, randomHeight, 0);
        var clamp = Mathf.Clamp(randomWidth, -1, 1);
        _startOrientation = (transform.right * -clamp);
    }
    [ContextMenu("Enemy Movement")]
    private void EnemyMovement()
    {
        var force = _startOrientation * _speed;
        _rigidBody.AddForce(force);
    }
    public void CheckPoint()
    {
        var isOffScreen = IsOffScreen();
        if (isOffScreen)
        {
            gameObject.SetActive(false);
        }
    }
    private bool IsOffScreen()
    {
        return transform.position.x < -screenWrap._screenWidth + _buffer ||
           transform.position.x > screenWrap._screenWidth - _buffer;
    }
    #region Gun
    public IEnumerator ShootRandomOverTime()
    {
        while (true)
        {
            if (PlayerController.Instance != null)
            {
                yield return new WaitForSeconds(1f);
                _gun.Shot(true, PlayerController.Instance.transform.position);
            }
            yield return null;
        }
    }
   
    #endregion
}
