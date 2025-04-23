using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour // Removendo o genérico aqui para simplificar a chamada do Shooter
{
    private Dictionary<GameObject, Queue<GameObject>> _pools = new Dictionary<GameObject, Queue<GameObject>>();
    private static ObjectPool _instance;

    public static ObjectPool Instance
    {
        get
        {
            if (_instance == null)
            {
                // Tenta encontrar uma instância existente na cena
                _instance = FindObjectOfType<ObjectPool>();

                // Se não existir, cria uma nova
                if (_instance == null)
                {
                    GameObject poolObject = new GameObject("ObjectPool");
                    _instance = poolObject.AddComponent<ObjectPool>();
                }
            }
            return _instance;
        }
        private set => _instance = value;
    }

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }
        _instance = this;
        DontDestroyOnLoad(gameObject); // Opcional: manter o pool entre as cenas
    }

    public GameObject Get(GameObject prefab)
    {
        if (prefab == null)
        {
            Debug.LogError("Prefab cannot be null.");
            return null;
        }

        if (!_pools.ContainsKey(prefab) || _pools[prefab].Count == 0)
        {
            // Se não houver pool para este prefab ou estiver vazio, instancia um novo
            GameObject newObject = Instantiate(prefab);
            newObject.SetActive(false);
            // Adiciona um componente para rastrear a desativação e retornar a este pool específico
            PoolReturner returner = newObject.AddComponent<PoolReturner>();
            returner.Pool = this;
            return newObject;
        }

        GameObject pooledObject = _pools[prefab].Dequeue();
        pooledObject.SetActive(true);
        return pooledObject;
    }

    public void Release(GameObject objectToRelease)
    {
        if (objectToRelease == null) return;

        Poolable poolable = objectToRelease.GetComponent<Poolable>();
        if (poolable != null && poolable.Prefab != null)
        {
            objectToRelease.SetActive(false);
            if (!_pools.ContainsKey(poolable.Prefab))
            {
                _pools[poolable.Prefab] = new Queue<GameObject>();
            }
            _pools[poolable.Prefab].Enqueue(objectToRelease);
        }
        else
        {
            Debug.LogWarning($"Object {objectToRelease.name} released but does not have a Poolable component or its prefab is not set. It will be destroyed.");
            Destroy(objectToRelease);
        }
    }

    // Componente auxiliar para identificar o prefab de origem e o pool
    public class Poolable : MonoBehaviour
    {
        public GameObject Prefab { get; set; }
    }

    private class PoolReturner : MonoBehaviour
    {
        public ObjectPool Pool { get; set; }

        private void OnDisable()
        {
            Pool?.Release(gameObject);
        }
    }
}