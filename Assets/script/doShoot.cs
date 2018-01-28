using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class doShoot : MonoBehaviour
{
    public objectPool objectPool;
    public Transform spawnLocation;
    public pooledObjectKey pooledObject;

    public void FireWeapon()
    {
        Debug.Log("Pew Pew");
        GameObject bullet = objectPool.sharedInstance.GetPooledObject(pooledObject);
        if (bullet != null)
        {
            bullet.transform.position = spawnLocation.position;
            bullet.transform.rotation = spawnLocation.rotation;
            bullet.SetActive(true);
        }
    }

}
