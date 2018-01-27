using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class camFollow : MonoBehaviour {

    public GameObject player;
    Vector2 moveVec;
	void Start () {
		
	}
	
	
	void Update () {
        moveVec = (player.transform.position - gameObject.transform.position);
        gameObject.transform.position += new Vector3( (moveVec * .3f).x, (moveVec * .2f).y, 0);
	}
}
