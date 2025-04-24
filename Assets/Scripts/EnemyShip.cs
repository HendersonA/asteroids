using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(BoxCollider2D))]
public class EnemyShip : MonoBehaviour
{
    [SerializeField] private float _secondsAppear = 3f;
    [SerializeField] private float _speed = 2f;
    [SerializeField] private ScreenWrap screenWrap;
    [SerializeField, Range(0, 100)] private int _firePrecision = 100;
    [SerializeField] private float _fireRate = 1f;
    [SerializeField] private Gun _gun;
    private Rigidbody2D _rigidBody;
    private Vector2 _startPosition;
    private void Awake()
    {
        _rigidBody = GetComponent<Rigidbody2D>();
    }
    private void OnEnable()
    {
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
    [ContextMenu("Enemy Movement")]
    private void EnemyMovement()
    {
        var randomHeight = Random.Range(-screenWrap._screenHeight, screenWrap._screenHeight);
        var randomWidth = Random.Range(0f, 1f) <= .5f ? screenWrap._screenWidth : -screenWrap._screenWidth;
        transform.position = new Vector3(randomWidth, randomHeight, 0);
        _startPosition = transform.position;
        var clamp = Mathf.Clamp(randomWidth, -1, 1);
        var orientation = (transform.right * -clamp);
        var force = orientation * _speed;
        _rigidBody.AddForce(force);
    }
    public void CheckPoint()
    {
        if ((int)transform.position.x == (int)_startPosition.x) //TODO Corrigir
        {
            gameObject.SetActive(false);
        }
    }
    #region Gun
    public IEnumerator ShootRandomOverTime()
    {
        while (true)
        {
            if (PlayerMovement.Instance != null)
            {
                var direction = DetectionPrecision(PlayerMovement.Instance.transform);
                _gun.Shot(direction);
            }
            yield return new WaitForSeconds(_fireRate);
        }
    }
    private Vector3 DetectionPrecision(Transform target)
    {
        float distancia = Vector3.Distance(transform.position, target.position);

        // Quanto menor a precisão, maior o ângulo máximo de desvio
        float maxDesvio = Mathf.Lerp(5f, 0f, _firePrecision / 100f); // 15 graus de desvio quando precisão = 0

        // Gera rotação aleatória dentro do cone de desvio
        Vector3 desvio = new Vector3(
           Random.Range(-maxDesvio, maxDesvio),
           Random.Range(-maxDesvio, maxDesvio),
           0);
        Vector3 pontoDeMira = target.position + desvio;
        Vector3 direcao = (pontoDeMira - transform.position).normalized;

        // Aqui você pode instanciar o projétil ou fazer raycast, usando `direcaoFinal`
        Debug.DrawRay(transform.position, direcao * distancia, Color.red, 1f); // visualiza no editor
        return direcao;
    }
    #endregion
}
