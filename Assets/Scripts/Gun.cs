using UnityEngine;

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
    private void Shot()
    {
        Instantiate(_bulletPrefab, _muzzlePosition.position, _muzzlePosition.rotation);
    }
}
