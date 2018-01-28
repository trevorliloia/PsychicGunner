using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class musicShift : MonoBehaviour {

    public bool action;
    public AudioSource bg;
    public AudioSource ac;
	void Start () {
        action = false;
        bg.volume = 1;
        ac.volume = 0;
	}
	
	// Update is called once per frame
	void Update () {
		if(action)
        {
            ac.volume += Time.deltaTime;
            bg.volume -= Time.deltaTime;
        }
        else
        {
            ac.volume -= Time.deltaTime;
            bg.volume += Time.deltaTime;
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            action = !action;
        }
	}
}
