using Assets.Scripts;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using static ObjectPool;

public class Gun : MonoBehaviour
{
    [SerializeField] private UnityEvent _onShot;
    [SerializeField, Range(0, 100)] private int _firePrecision = 100;
    [SerializeField] private float _fireRate = 1f;
    [SerializeField] private GameObject _bulletPrefab;
    [SerializeField] private LayerMask _hitMask;
    [Header("Muzzle")]
    [SerializeField] private Transform _muzzlePosition;
    [SerializeField] private SpriteRenderer _muzzleFlash;
    [SerializeField] private Sprite[] _muzzleSprites;
    private float _nextfireTime = 0f;

    [ContextMenu("Shot")]
    public void Shot(Vector2? position = null)
    {
        if (_bulletPrefab != null)
        {
            if (Time.time <= _nextfireTime) return;
            _nextfireTime = Time.time + _fireRate;
            GameObject bulletObject = ObjectPool.Instance.Get(_bulletPrefab);

            if (bulletObject != null)
            {
                Poolable poolable = bulletObject.GetComponent<Poolable>();
                if (poolable != null)
                    poolable.Prefab = _bulletPrefab;
                else
                {
                    poolable = bulletObject.AddComponent<Poolable>();
                    poolable.Prefab = _bulletPrefab;
                }
                _onShot?.Invoke();
                bulletObject.SetActive(true); 
                var newDirection = position == null ? -this.transform.up : position;
                var shotPrecision = GetPrecision(newDirection.Value);
                bulletObject.GetComponent<Bullet>().SetBullet(_hitMask, _muzzlePosition, shotPrecision);
            }
        }
        else
        {
            Debug.LogError("Bullet Prefab not assigned in the Shooter script.");
        }
    }
    private Vector3 GetPrecision(Vector2 position)
    {
        float maxDesvio = Mathf.Lerp(5f, 0f, _firePrecision / 100f); 
        Vector2 desvio = new Vector3(
           Random.Range(-maxDesvio, maxDesvio),
           Random.Range(-maxDesvio, maxDesvio));
        Vector3 pontoDeMira = position + desvio;
        Vector3 direcao = (pontoDeMira - transform.position).normalized;
        return direcao;
    }
    public void MuzzleFlashFX() => StartCoroutine(MuzzleFlashCoroutine());
    private IEnumerator MuzzleFlashCoroutine()
    {
        for (int i = 0; i < _muzzleSprites.Length; i++)
        {
            var sprite = _muzzleSprites[i];
            _muzzleFlash.sprite = sprite;
            yield return new WaitForSeconds(.1f);
        }
        _muzzleFlash.sprite = null;
        yield return new WaitForEndOfFrame();
    }
}
