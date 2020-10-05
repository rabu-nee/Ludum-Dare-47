using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Singleton;

public class ObjectPooler : Singleton<ObjectPooler> {

    [System.Serializable]
    public class Pool {
        public string tag;
        public GameObject[] prefab;
        public int size;
    }

    public List<Pool> pools;
    public Dictionary<string, Queue<GameObject>> poolDictionary;

    // Use this for initialization
    new void Awake() {
        base.Awake();

        poolDictionary = new Dictionary<string, Queue<GameObject>>();

        foreach (Pool pool in pools) {
            Queue<GameObject> objectPool = new Queue<GameObject>();
            for (int i = 0; i < pool.size; i++) {
                int rand = Random.Range(0, pool.prefab.Length);
                GameObject obj = Instantiate(pool.prefab[rand]);
                obj.SetActive(false);
                obj.transform.parent = this.transform;
                objectPool.Enqueue(obj);
            }
            poolDictionary.Add(pool.tag, objectPool);
        }

    }

    public static GameObject SpawnFromPool(string tag, Vector3 position, Quaternion rotation) {

        if (!Instance.poolDictionary.ContainsKey(tag)) {
            return null;
        }

        GameObject objToSpawn = Instance.poolDictionary[tag].Dequeue();

        objToSpawn.SetActive(true);
        objToSpawn.transform.position = position;
        objToSpawn.transform.rotation = rotation;


        IPooledObject pooledObject = objToSpawn.GetComponent<IPooledObject>();
        if (pooledObject != null) {
            pooledObject.OnObjectSpawn();
        }

        Instance.poolDictionary[tag].Enqueue(objToSpawn);
        return objToSpawn;
    }

}