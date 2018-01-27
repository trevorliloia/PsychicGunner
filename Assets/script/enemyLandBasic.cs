using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyLandBasic : MonoBehaviour
{
    [SerializeField] Vector3[] localWayPoints;
    [SerializeField] float speed, movementDelay, delayReset;
    [SerializeField] LayerMask targetMask;
    Vector3[] globalWayPoints;
    int currentPoint;

    bool playerDetected;


    void Start ()
    {
        globalWayPoints = new Vector3[localWayPoints.Length];
        delayReset = movementDelay;

        for (int i = 0; i < localWayPoints.Length; ++i)
            globalWayPoints[i] = localWayPoints[i] + transform.position;
	}
	
	void Update ()
    {
        if(!playerDetected)
            MoveToWayPoint();
	}

    void MoveToWayPoint()
    {
        movementDelay -= Time.deltaTime;
        if(movementDelay <= 0)
        {
            transform.position = Vector3.Lerp(transform.position, globalWayPoints[currentPoint], speed * Time.deltaTime);

            if (transform.position.x >= globalWayPoints[currentPoint].x - .1 && transform.position.x <= globalWayPoints[currentPoint].x + .1 && transform.position.y >= globalWayPoints[currentPoint].y - .1 && transform.position.y <= globalWayPoints[currentPoint].y + .1)
            {
                if (currentPoint + 1 >= localWayPoints.Length)
                    currentPoint = 0;
                else
                    currentPoint++;
                movementDelay = delayReset;
            }
        }
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
