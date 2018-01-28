using UnityEngine;

[System.Serializable]
public class objectToPoolMeta {
    public pooledObjectKey key;
    public GameObject prototype;
    public int count;
    public bool shouldExpand;
}
