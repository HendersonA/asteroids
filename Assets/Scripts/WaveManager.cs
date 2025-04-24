using System;
using System.Collections.Generic;
using UnityEngine;
using static ObjectPool;

public class WaveManager : MonoBehaviour
{
    [Serializable]
    public struct Wave
    {
        public GameObject[] _prefabs;
    }
    [SerializeField] private Wave[] _waves;
    public Action OnStartWave;
    private List<GameObject> _cachedObjects;
    private int _currentWaveIndex = -1;

    private void Start()
    {
        _cachedObjects = new List<GameObject>();
        SpawnWave();
        OnStartWave += SpawnWave;
    }
    [ContextMenu("Spawn Wave")]
    public void SpawnWave()
    {
        _currentWaveIndex++;
        if (_currentWaveIndex > _waves.Length - 1) _currentWaveIndex = 0;

        var wave = _waves[_currentWaveIndex];
        for (int j = 0; j < wave._prefabs.Length; j++)
        {
            var prefab = wave._prefabs[j];
            SpawnFromPool(prefab);
        }
    }
    private void SpawnFromPool(GameObject prefabObject)
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
            _cachedObjects.Add(poolObject);
        }
    }
    //TODO Aleatoriedade da posição
}
