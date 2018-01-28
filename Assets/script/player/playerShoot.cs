using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerShoot : MonoBehaviour {
    public float gunPivotHeight;
    public float gunLength;

    public float testAngle;

    private Transform gunTransform;

    private void Awake() {
        gunTransform = transform.Find("Gun");
    }

    // Use this for initialization
    void Start () {

    }
	
	// Update is called once per frame
	void Update () {
        RotateGun();
    }

    private void RotateGun() {
        Vector3 gunPivotPosition = transform.TransformPoint(new Vector3(0, gunPivotHeight, 0));
        Vector3 mousePosition = new Vector3(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y, 0);
        Vector3 targetGunDirection = (mousePosition - gunPivotPosition).normalized;
        float angle = Vector3.SignedAngle(gunTransform.right, targetGunDirection, Vector3.forward);
        gunTransform.RotateAround(gunPivotPosition, Vector3.forward, angle);
    }
}
