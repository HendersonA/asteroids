using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Asteroids
{
    public class WaveManager : Singleton<WaveManager>
    {
        [Serializable]
        public struct WaveElement
        {
            public GameObject Prefab;
            public int Amount;
        }
        [Serializable]
        public struct Wave
        {
            public string Name;
            public WaveElement[] WaveElements;
        }
        [SerializeField] private Wave[] _waves;
        private int activeEnemies = 0;
        private int _currentWaveIndex = -1;

        private void Start()
        {
            SpawnWave();
        }
        [ContextMenu("Spawn Wave")]
        public void SpawnWave()
        {
            _currentWaveIndex++;
            if (_currentWaveIndex > _waves.Length - 1) _currentWaveIndex = 0;

            var wave = _waves[_currentWaveIndex];
            for (int j = 0; j < wave.WaveElements.Length; j++)
            {
                var element = wave.WaveElements[j];
                for (int i = 0; i < element.Amount; i++)
                {
                    var spawnedObject = SpawnFromPool(element.Prefab);
                    RegisterEnemy();
                    spawnedObject.transform.position = RandomPosition();
                    spawnedObject.TryGetComponent(out Health health);
                    health.OnDeath.RemoveListener(EnemyDefeated);
                    health.OnDeath.AddListener(EnemyDefeated);
                }
            }
        }
        private GameObject SpawnFromPool(GameObject prefabObject)
        {
            GameObject newObject = ObjectPool.Instance.Get(prefabObject);
            newObject.SetActive(true);
            return newObject;
        }
        public void RegisterEnemy()
        {
            activeEnemies++;
        }
        public void EnemyDefeated()
        {
            activeEnemies--;
            if (activeEnemies <= 0)
                SpawnWave();
        }
        private Vector2 RandomPosition()
        {
            float angle = Random.Range(0f, Mathf.PI * 2f);

            var playerPosition = new Vector2(
                GameManager.PlayerTransform.position.x,
                GameManager.PlayerTransform.position.y);

            float distance = Random.Range(-playerPosition.x * 2, playerPosition.y * 2);
            Vector2 offset = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * distance;

            return playerPosition + offset;
        }
    }
}