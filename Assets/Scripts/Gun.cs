using Assets.Scripts;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using static ObjectPool;

public class Gun : MonoBehaviour
{
    [SerializeField] private UnityEvent _onShot;
    [SerializeField] private float _fireRate = 1f;
    [SerializeField] private GameObject _bulletPrefab;
    [Header("Muzzle")]
    [SerializeField] private Transform _muzzlePosition;
    [SerializeField] private SpriteRenderer _muzzleFlash;
    [SerializeField] private Sprite[] _muzzleSprites;
    private float _nextfireTime = 0f;
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
                bulletObject.GetComponent<Bullet>().SetBullet(_muzzlePosition, position);
            }
        }
        else
        {
            Debug.LogError("Bullet Prefab not assigned in the Shooter script.");
        }
    }
    public void MuzzleFlashFX() => StartCoroutine(MuzzleFlashCoroutine());
    private IEnumerator MuzzleFlashCoroutine() //TODO Refatorar
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
