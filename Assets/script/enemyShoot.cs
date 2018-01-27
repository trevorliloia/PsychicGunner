using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyShoot : MonoBehaviour
{
    public GameObject bullet;
    public Transform spawnLocation;
    public float burstRate, reloadRate;
    float burstTimer, reloadTimer;
    public bool playerDetected;
    float direction;

    enemyLandBasic movemement;
    Animator anim;

    void Start()
    { 
        movemement = GetComponent<enemyLandBasic>();
        anim = GetComponentInParent<Animator>();
        burstTimer = burstRate;
        reloadTimer = reloadRate;
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
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            playerDetected = false;
            burstTimer = burstRate;
            reloadTimer = reloadRate;
            anim.SetBool("Shooting", false);
        }
    }



    void Shoot()
    {
        anim.SetBool("Shooting", true);
        burstTimer -= Time.deltaTime;
        if (burstTimer <= 0)
        {
            anim.SetBool("Reload", true);
            Reload();
        }
    }

    void Reload()
    {
        reloadTimer -= Time.deltaTime;
        if(reloadTimer <= 0)
        {
            anim.SetBool("Reload", false);
            burstTimer = burstRate;
            reloadTimer = reloadRate;
        }
    }
}
