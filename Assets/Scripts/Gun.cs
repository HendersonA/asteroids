using Assets.Scripts;
using UnityEngine;
using static ObjectPool;

public class Gun : MonoBehaviour
{
    [SerializeField] private GameObject _bulletPrefab;
    [SerializeField] private Transform _muzzlePosition;

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Space))
        {
            Shot();
        }
    }
    [ContextMenu("Shot")]
    public void Shot(Vector2? position = null)
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
                bulletObject.SetActive(true);
                bulletObject.GetComponent<Bullet>().SetBullet(_muzzlePosition, position);
            }
        }
        else
        {
            Debug.LogError("Bullet Prefab not assigned in the Shooter script.");
        }
    }
}
