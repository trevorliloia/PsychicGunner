using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyLandBasic : MonoBehaviour, iDamageable
{
    [SerializeField] float health;
    [SerializeField] Vector3[] localWayPoints;
    [SerializeField] float speed, movementDelay, delayReset;
    Vector3[] globalWayPoints;
    int currentPoint;
    [SerializeField] enemyShoot shoot;
    [SerializeField] GameObject deathEffect;
    Quaternion baseRot;

    public bool isMoving;

    public bool dieTest;

    void Start ()
    {
        globalWayPoints = new Vector3[localWayPoints.Length];
        delayReset = movementDelay;
        baseRot = transform.rotation;

        for (int i = 0; i < localWayPoints.Length; ++i)
            globalWayPoints[i] = localWayPoints[i] + transform.position;
	}
	
	void Update ()
    {
        if(!shoot.playerDetected)
            MoveToWayPoint();
        if (isMoving)
            SetRotation();

        if (dieTest)
            Die();
    }

    void MoveToWayPoint()
    {
        movementDelay -= Time.deltaTime;
        if(movementDelay <= 0)
        {
            transform.position = Vector3.Lerp(transform.position, globalWayPoints[currentPoint], speed * Time.deltaTime);

            if (transform.position.x >= globalWayPoints[currentPoint].x - .5 && transform.position.x <= globalWayPoints[currentPoint].x + .5 && transform.position.y >= globalWayPoints[currentPoint].y - .5 && transform.position.y <= globalWayPoints[currentPoint].y + .5)
            {
                if (currentPoint + 1 >= localWayPoints.Length)
                    currentPoint = 0;
                else
                    currentPoint++;
                movementDelay = delayReset;
                isMoving = false;               
            }
            else
                isMoving = true;
        }
    }

    public float Direction()
    {
       return globalWayPoints[currentPoint].x - transform.position.x;
    }

    void SetRotation()
    {
        if (Direction() < 0)
        {
            Quaternion rot = new Quaternion();
            rot.eulerAngles = new Vector3(0, 180, 0);
            transform.rotation = rot;
         }
        else
        {
            transform.rotation = baseRot;
        }
    }

    public void TakeDamage(float damage)
    {
        health -= damage;

        if(health <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Instantiate(deathEffect, transform.position, deathEffect.transform.rotation);
        Destroy(gameObject);
    }

    void OnDrawGizmos()
    {
        if (localWayPoints != null)
        {
            Gizmos.color = Color.cyan;
            float size = .4f;

            for (int i = 0; i < localWayPoints.Length; i++)
            {
                Vector3 globalWaypointPos = (Application.isPlaying) ? globalWayPoints[i] : localWayPoints[i] + transform.position;
                Gizmos.DrawLine(globalWaypointPos - Vector3.up * size, globalWaypointPos + Vector3.up * size);
                Gizmos.DrawLine(globalWaypointPos - Vector3.left * size, globalWaypointPos + Vector3.left * size);
            }
        } 
    }
}
