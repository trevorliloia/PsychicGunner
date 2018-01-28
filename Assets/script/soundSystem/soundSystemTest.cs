using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class soundSystemTest : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.A)) {
            soundSystem.sharedInstance.Play(audioClipKey.TEST_CLIP_1);
        }

        if (Input.GetKeyDown(KeyCode.S)) {
            soundSystem.sharedInstance.Play(audioClipKey.TEST_CLIP_2);
        }
	}
}
