using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class doShoot : MonoBehaviour
{
    [SerializeField] float spreadAngle, numberOfBullets;
    float currentAngle;
    public objectPool objectPool;
    public Transform spawnLocation;
    public Transform[] spreadSpawns;
    public pooledObjectKey pooledObject;
    [SerializeField] GameObject[] bullet;

    public void FireWeapon()
    {

        Debug.Log("Pew Pew");

        bullet[0]= objectPool.sharedInstance.GetPooledObject(pooledObject);
        if (bullet != null && pooledObject != pooledObjectKey.BULLET_2)
        {
            bullet[0].transform.position = spawnLocation.position;
            bullet[0].transform.rotation = spawnLocation.rotation;
            bullet[0].SetActive(true);
        }
        else if (bullet != null && pooledObject == pooledObjectKey.BULLET_2)
        {
            for(int i = 0; i < spreadSpawns.Length - 1; ++i)
            {

                bullet[i] = objectPool.sharedInstance.GetPooledObject(pooledObject);

                bullet[i].transform.position = spreadSpawns[i].position;
                bullet[i].transform.rotation = spreadSpawns[i].rotation;
                bullet[i].SetActive(true);
            }
        }
    }

}
