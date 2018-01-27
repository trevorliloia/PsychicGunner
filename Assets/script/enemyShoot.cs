using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyShoot : MonoBehaviour
{
    public GameObject bullet;
    public Vector3 spawnLocation;
    public float fireRate;
    float fireTimer = 0;
    public bool playerDetected;
    float direction;

    enemyLandBasic movemement;
    Animator anim;

    void Start()
    {
        movemement = GetComponent<enemyLandBasic>();
        anim = GetComponentInParent<Animator>();
    }

    void Update ()
    {
        if (playerDetected)
            Shoot();
	}

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            playerDetected = true;
            anim.SetBool("Shooting", true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            playerDetected = false;
            fireTimer = 0;
            anim.SetBool("Shooting", false);
        }
    }

    void Shoot()
    {
        fireTimer -= Time.deltaTime;

        if(fireTimer <= 0)
        {
            Debug.Log("Pew Pew!");
            //Shoot object pooled projectiles
            fireTimer = fireRate;
        }
    }
}
