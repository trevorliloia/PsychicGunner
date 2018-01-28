using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class objectPoolTest : MonoBehaviour {
    private objectPool pool;
    private int Count = 0;
    private GameObject prevObj;

	// Use this for initialization
	void Start () {
        pool = objectPool.sharedInstance;
        
    }
	
	// Update is called once per frame
	void Update () {
        GameObject obj = pool.GetPooledObject(pooledObjectKey.BULLET_1);
        if (obj != null) {
            obj.SetActive(true);
        }

        if (Count > 5) {
            prevObj.SetActive(false);
        }
        prevObj = obj;
        Count++;
    }
}
