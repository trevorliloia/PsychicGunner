using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class platformController : raycastController{

    public LayerMask passengerMask;

    public Vector3[] localWaypoints;
    Vector3[] globalWaypoints;

    public float speed;
    public bool cyclic;
    public float waitTime;

    [Range(0,2)]
    public float easeAmt;

    int fromPoint;
    float percentBetween;
    float nextMoveTime;

    List<PassengerMovement> passengerMoveList;
    Dictionary<Transform, controller2D> passengerDictionary = new Dictionary<Transform, controller2D>();

	public override void Start () {
        base.Start();

        globalWaypoints = new Vector3[localWaypoints.Length];
        for (int i = 0; i < localWaypoints.Length; i++)
        {
            globalWaypoints[i] = localWaypoints[i] + transform.position;
        }

    }
	
	// Update is called once per frame
	void Update () {
        UpdateRaycastOrigin();

        Vector3 velocity = CalculatePlatformMove();

        CalculatePassengerMove(velocity);

        MovePassengers(true);
        transform.Translate(velocity);
        MovePassengers(false);
	}

    float Ease(float x)
    {
        float a = easeAmt + 1;
        return Mathf.Pow(x, a) / (Mathf.Pow(x, a) + Mathf.Pow(1 - x, a));
    }

    Vector3 CalculatePlatformMove()
    {
        if (Time.time < nextMoveTime)
        {
           return Vector3.zero;
        }

        fromPoint %= globalWaypoints.Length;
        int toPoint = (fromPoint + 1) % globalWaypoints.Length;
        float distanceBetween = Vector3.Distance(globalWaypoints[fromPoint], globalWaypoints[toPoint]);
        percentBetween += Time.deltaTime * speed / distanceBetween;
        percentBetween = Mathf.Clamp01(percentBetween);
        float eased = Ease(percentBetween);

        Vector3 newPos = Vector3.Lerp(globalWaypoints[fromPoint], globalWaypoints[toPoint], eased);

        if(percentBetween >= 1)
        {
            percentBetween = 0;
            fromPoint++;
            if(!cyclic)
            {
                if(fromPoint >= globalWaypoints.Length - 1)
                {
                    fromPoint = 0;
                    System.Array.Reverse(globalWaypoints);
                }
            }
            nextMoveTime = Time.time + waitTime;
        }

        return newPos - transform.position;
    }

    void MovePassengers(bool beforeMovePlatform)
    {
        foreach(PassengerMovement p in passengerMoveList)
        {
            if(!passengerDictionary.ContainsKey(p.transform))
            {
                passengerDictionary.Add(p.transform, p.transform.GetComponent<controller2D>());
            }

            if (p.moveBeforePlatform == beforeMovePlatform)
            {
                passengerDictionary[p.transform].Move(p.velocity, p.standOnPlatform);
            }
        }
    }

    void CalculatePassengerMove(Vector3 velocity)
    {
        HashSet<Transform> movedPassengers = new HashSet<Transform>();
        passengerMoveList = new List<PassengerMovement>();

        float directionX = Mathf.Sign(velocity.x);
        float directionY = Mathf.Sign(velocity.y);

        if (directionY != 0)
        {
            float rayLength = Mathf.Abs(velocity.y) + skinWidth;

            for (int i = 0; i < verticalRayCount; i++)
            {
                Vector2 rayOrigin = (directionY == -1) ? raycastOrigins.bottomL : raycastOrigins.topL;
                rayOrigin += Vector2.right * (verticalRaySpacing * i + velocity.x);
                RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.up * directionY, rayLength, passengerMask);

                if (hit)
                {
                    if(!movedPassengers.Contains(hit.transform))
                    {
                        movedPassengers.Add(hit.transform);
                        float pushX = (directionY == 1) ? velocity.x : 0;
                        float pushY = velocity.y - (hit.distance - skinWidth) * directionY;


                        passengerMoveList.Add(new PassengerMovement(hit.transform, new Vector3(pushX, pushY), directionY == 1, true));
                        
                    }

                    
                }
            }
        }

        if (directionX != 0)
        {
            float rayLength = Mathf.Abs(velocity.x) + skinWidth;

            for (int i = 0; i < horizontalRayCount; i++)
            {
                Vector2 rayOrigin = (directionX == -1) ? raycastOrigins.bottomL : raycastOrigins.bottomR;
                rayOrigin += Vector2.up * (horizontalRaySpacing * i);
                RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.right * directionX, rayLength, passengerMask);

                if (hit)
                {
                    if (!movedPassengers.Contains(hit.transform))
                    {
                        float pushX = velocity.x - (hit.distance - skinWidth) * directionX;
                        float pushY = -skinWidth;

                        passengerMoveList.Add(new PassengerMovement(hit.transform, new Vector3(pushX, pushY), false, true));
                        movedPassengers.Add(hit.transform);
                    }


                }
            }
        }


        if(directionY == -1 || velocity.y == 0 && velocity.x != 0)
        {
            float rayLength = skinWidth * 2;

            for (int i = 0; i < verticalRayCount; i++)
            {
                Vector2 rayOrigin = raycastOrigins.topL + Vector2.right * (verticalRaySpacing * i + velocity.x);
                RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.up, rayLength, passengerMask);

                if (hit)
                {
                    if (!movedPassengers.Contains(hit.transform))
                    {
                        float pushX = velocity.x;
                        float pushY = velocity.y;
                        
                        movedPassengers.Add(hit.transform);

                        passengerMoveList.Add(new PassengerMovement(hit.transform, new Vector3(pushX, pushY), true, false));
                    }


                }
            }
        }
    }
    struct PassengerMovement
    {
        public Transform transform;
        public Vector3 velocity;
        public bool standOnPlatform;
        public bool moveBeforePlatform;

        public PassengerMovement(Transform _transform, Vector3 _velocity, bool _standingOnPlatform, bool _moveBeforePlatform)
        {
            transform = _transform;
            velocity = _velocity;
            standOnPlatform = _standingOnPlatform;
            moveBeforePlatform = _moveBeforePlatform;
        }
    }

    void OnDrawGizmos()
    {
        if(localWaypoints!=null)
        {
            Gizmos.color = Color.cyan;
            float size = .4f;

            for (int i = 0; i < localWaypoints.Length; i++)
            {
                Vector3 globalWaypointPos = (Application.isPlaying)?globalWaypoints[i] : localWaypoints[i] + transform.position;
                Gizmos.DrawLine(globalWaypointPos - Vector3.up * size, globalWaypointPos + Vector3.up * size);
                Gizmos.DrawLine(globalWaypointPos - Vector3.left * size, globalWaypointPos + Vector3.left * size);
            }
        }


    }
}
