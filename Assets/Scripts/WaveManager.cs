using System;
using UnityEngine;
using static ObjectPool;

public class WaveManager : MonoBehaviour
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
                spawnedObject.TryGetComponent(out Health health);
                health.OnDeath.AddListener(EnemyDefeated);
            }
        }
    }
    private GameObject SpawnFromPool(GameObject prefabObject)
    {
        GameObject poolObject = ObjectPool.Instance.Get(prefabObject);

        if (poolObject != null)
        {
            Poolable poolable = poolObject.GetComponent<Poolable>();
            if (poolable != null)
                poolable.Prefab = prefabObject;
            else
            {
                poolable = poolObject.AddComponent<Poolable>();
                poolable.Prefab = prefabObject;
            }
            poolObject.SetActive(true);
        }
        return poolObject;
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
    //TODO Aleatoriedade da posição
}
