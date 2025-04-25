using UnityEngine;

namespace Assets.Scripts
{
    [CreateAssetMenu(fileName = "NewGunSettings", menuName = "ScriptableObjects/GunSettings")]
    public class GunSettings : ScriptableObject
    {
        [Range(0, 100)] public int FirePrecision = 100;
        public float FireRate = 1f;
        public GameObject BulletPrefab;
        public LayerMask HitMask;
    }
}