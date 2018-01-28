using System.Collections.Generic;
using UnityEngine;

public class objectPool : MonoBehaviour {
    public static objectPool sharedInstance;
    public List<objectToPoolMeta> objectsToPool;
    private Dictionary<pooledObjectKey, List<GameObject>> pooledObjects;

	void Awake () {
        if (sharedInstance == null) {
            sharedInstance = this;
        }

        pooledObjects = new Dictionary<pooledObjectKey, List<GameObject>>();

        foreach (objectToPoolMeta meta in objectsToPool) {
            pooledObjects[meta.key] = new List<GameObject>();
            for (int i = 0; i < meta.count; i++) {
                InstantiateAndPool(pooledObjects[meta.key], meta.prototype);
            }
        }
	}

    /// <summary>
    /// Get one inactive pooled object of certain key. 
    /// When no object found, if shouldExpand is true, expand the pool, otherwise return null.
    /// </summary>
    /// <param name="key">Object key</param>
    /// <returns>Obtained available object, returned as inactive</returns>
    public GameObject GetPooledObject(pooledObjectKey key) {
        List<GameObject> list = pooledObjects[key];
        foreach (GameObject obj in list) {
            if (!obj.activeInHierarchy) {
                return obj;
            }
        }
        foreach (objectToPoolMeta meta in objectsToPool) {
            if (meta.shouldExpand) {
                return InstantiateAndPool(pooledObjects[meta.key], meta.prototype);
            }
        }
        Debug.LogWarning(System.String.Format("Unable to load pooled object {0}, too many active instances", System.Enum.GetName(typeof(pooledObjectKey), key)));
        return null;
    }

    private GameObject InstantiateAndPool(List<GameObject> list, GameObject prefab) {
        GameObject obj = Instantiate(prefab);
        obj.SetActive(false);
        list.Add(obj);
        return obj;
    }
}
