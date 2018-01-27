using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class raycastController : MonoBehaviour {

    public LayerMask collisionMask;

    public const float skinWidth = 0.015f;
    public int horizontalRayCount = 4;
    public int verticalRayCount = 4;

    public float maxClimbAngle = 80;
    public float maxDescendAngle = 80;

    public float horizontalRaySpacing;
    public float verticalRaySpacing;

    public RaycastOrigins raycastOrigins;
    public BoxCollider2D colliderB;
    public virtual void Start () {
        colliderB = GetComponent<BoxCollider2D>();
        CalculateRaySpacing();
    }
    

    public void UpdateRaycastOrigin()
    {
        Bounds bounds = colliderB.bounds;
        bounds.Expand(skinWidth * -2);

        raycastOrigins.bottomL = new Vector2(bounds.min.x, bounds.min.y);
        raycastOrigins.bottomR = new Vector2(bounds.max.x, bounds.min.y);
        raycastOrigins.topL = new Vector2(bounds.min.x, bounds.max.y);
        raycastOrigins.topR = new Vector2(bounds.max.x, bounds.max.y);
    }

    public void CalculateRaySpacing()
    {
        Bounds bounds = colliderB.bounds;
        bounds.Expand(skinWidth * -2);

        horizontalRayCount = Mathf.Clamp(horizontalRayCount, 2, int.MaxValue);
        verticalRayCount = Mathf.Clamp(verticalRayCount, 2, int.MaxValue);

        horizontalRaySpacing = bounds.size.y / (horizontalRayCount - 1);
        verticalRaySpacing = bounds.size.x / (verticalRayCount - 1);
    }

    public struct RaycastOrigins
    {
        public Vector2 topL, topR;
        public Vector2 bottomL, bottomR;
    }
}
