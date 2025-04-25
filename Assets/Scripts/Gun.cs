using Assets.Scripts;
using UnityEngine;
using UnityEngine.Events;
using static ObjectPool;

public class Gun : MonoBehaviour
{
    [SerializeField] private UnityEvent _onShot;
    [SerializeField] private GunSettings _gunSettings;
    [Header("Muzzle")]
    [SerializeField] private Transform _muzzlePosition;
    [SerializeField] private SpriteRenderer _muzzleFlash;
    [SerializeField] private Sprite[] _muzzleSprites;
    private float _nextfireTime = 0f;

    private void Start()
    {
        ObjectPool.Instance.InitializePool(_gunSettings.BulletPrefab, 5);
    }
    [ContextMenu("Shot")]
    public void Shot(bool isEnemy = false, Vector2? position = null)
    {
        if (Time.time <= _nextfireTime) return;
        _nextfireTime = Time.time + _gunSettings.FireRate;
        var bullet = SpawnFromPool();
        _onShot?.Invoke();
        bullet.SetActive(true); 
        var newDirection = position == null ? -this.transform.up : position;
        var shotPrecision = SetPrecision(newDirection.Value);
        if(isEnemy)
            shotPrecision = (shotPrecision - transform.position).normalized;
        bullet.GetComponent<Bullet>().SetBullet(_gunSettings.HitMask, _muzzlePosition, shotPrecision);
    }
    private GameObject SpawnFromPool()
    {
        if (_gunSettings.BulletPrefab != null)
        {
            GameObject bulletObject = ObjectPool.Instance.Get(_gunSettings.BulletPrefab);
            return bulletObject;
        }
        else
            Debug.LogError("Bullet Prefab not assigned in the Shooter script.");
        return null;
    }
    private Vector3 SetPrecision(Vector2 position)
    {
        float maxDeviation = Mathf.Lerp(5f, 0f, _gunSettings.FirePrecision / 100f); 
        Vector2 deviation = new Vector3(
           Random.Range(-maxDeviation, maxDeviation),
           Random.Range(-maxDeviation, maxDeviation));
        Vector3 target = position + deviation;
        return target;
    }
}
