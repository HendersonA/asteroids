using Assets.Scripts;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;
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
    public void Shot(bool isEnemy = false, Vector2? position = null)
    {
        if (Time.time <= _nextfireTime) return;
        _nextfireTime = Time.time + _fireRate;
        var bullet = SpawnFromPool();
        _onShot?.Invoke();
        bullet.SetActive(true); 
        var newDirection = position == null ? -this.transform.up : position;
        var shotPrecision = SetPrecision(newDirection.Value);
        if(isEnemy)
            shotPrecision = (shotPrecision - transform.position).normalized;
        bullet.GetComponent<Bullet>().SetBullet(_hitMask, _muzzlePosition, shotPrecision);
    }
    private GameObject SpawnFromPool()
    {
        if (_bulletPrefab != null)
        {
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
            }
            return bulletObject;
        }
        else
        {
            Debug.LogError("Bullet Prefab not assigned in the Shooter script.");
        }
        return null;
    }
    private Vector3 SetPrecision(Vector2 position)
    {
        float maxDeviation = Mathf.Lerp(5f, 0f, _firePrecision / 100f); 
        Vector2 deviation = new Vector3(
           Random.Range(-maxDeviation, maxDeviation),
           Random.Range(-maxDeviation, maxDeviation));
        Vector3 target = position + deviation;
        return target;
    }
}
