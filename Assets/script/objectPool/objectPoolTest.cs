using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class objectPoolTest : MonoBehaviour {
    private objectPool pool;
    private int Count = 0;
    private GameObject prevObj;
    private List<GameObject> activeObj;

	// Use this for initialization
	void Start () {
        pool = objectPool.sharedInstance;
        activeObj = new List<GameObject>();
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.A)) {
            GameObject obj = pool.GetPooledObject(pooledObjectKey.BULLET_CLASSIC);
            if (obj != null) {
                obj.SetActive(true);
                activeObj.Add(obj);
            }
        }
        
        if (Input.GetKeyDown(KeyCode.S)) {
            GameObject obj = activeObj[activeObj.Count - 1];
            activeObj.RemoveAt(activeObj.Count - 1);
            obj.SetActive(false);
        }
    }
}
