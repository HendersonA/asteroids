using System.Collections.Generic;
using UnityEngine;

namespace Asteroids
{
    public class ObjectPool
    {
        private static ObjectPool _instance;
        public static ObjectPool Instance => _instance ??= new ObjectPool();

        private readonly Dictionary<GameObject, Queue<GameObject>> _pools = new();
        private readonly Dictionary<GameObject, Transform> _parents = new();
        private Transform _root;

        private ObjectPool() { }

        /// <summary>
        /// Define um transform root para organizar os objetos na hierarquia.
        /// </summary>
        public void SetRoot(Transform root)
        {
            _root = root;
        }
        public void InitializePool(GameObject prefab, int amount)
        {
            EnsurePoolExists(prefab);

            for (int i = 0; i < amount; i++)
            {
                GameObject obj = CreateNewObject(prefab);
                obj.SetActive(false);
                _pools[prefab].Enqueue(obj);
            }
        }
        public GameObject Get(GameObject prefab)
        {
            EnsurePoolExists(prefab);

            if (_pools[prefab].Count == 0)
                return CreateNewObject(prefab);

            GameObject obj = _pools[prefab].Dequeue();
            obj.SetActive(true);
            return obj;
        }
        public void Release(GameObject obj, GameObject prefab)
        {
            if (!_pools.ContainsKey(prefab)) return;

            obj.SetActive(false);
            obj.transform.SetParent(_parents[prefab]);
            _pools[prefab].Enqueue(obj);
        }
        public void Clear()
        {
            foreach (var queue in _pools.Values)
            {
                while (queue.Count > 0)
                {
                    GameObject obj = queue.Dequeue();
                    if (obj != null)
                        Object.Destroy(obj);
                }
            }

            _pools.Clear();
            _parents.Clear();
        }
        private void EnsurePoolExists(GameObject prefab)
        {
            if (_pools.ContainsKey(prefab)) return;

            _pools[prefab] = new Queue<GameObject>();

            GameObject parent = new(prefab.name + "_Pool");
            if (_root != null)
                parent.transform.SetParent(_root);

            _parents[prefab] = parent.transform;
        }
        private GameObject CreateNewObject(GameObject prefab)
        {
            GameObject obj = Object.Instantiate(prefab, _parents[prefab]);
            obj.AddComponent<PoolReturnHelper>().Setup(this, prefab);
            return obj;
        }
        private class PoolReturnHelper : MonoBehaviour
        {
            private ObjectPool _pool;
            private GameObject _prefab;

            public void Setup(ObjectPool pool, GameObject prefab)
            {
                _pool = pool;
                _prefab = prefab;
            }

            private void OnDisable()
            {
                _pool?.Release(gameObject, _prefab);
            }
        }
    }
}