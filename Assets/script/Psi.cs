using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Psi : MonoBehaviour {

    public GameObject player;
    public Image psiBar;
    public float psiAmt;

    public Image portCool;

    public float portTimer;

    public float poofTime;
    public SpriteRenderer render;
    public BoxCollider2D box;
    public Vector3 vel;
    public ParticleSystem partSys;
    Animator anim;
    void Start () {
        player = gameObject;
        psiAmt = 100;
        portTimer = 0;
        poofTime = 0;
        anim = GetComponentInChildren<Animator>();
        //render = gameObject.GetComponent<MeshRenderer>();
        box = gameObject.GetComponent<BoxCollider2D>();
        partSys = gameObject.GetComponent<ParticleSystem>();
        vel = player.GetComponent<playerMove>().velocity;
    }
	
	// Update is called once per frame
	void Update () {
        portTimer += Time.deltaTime;
        poofTime -= Time.deltaTime;
        if(Input.GetMouseButtonDown(1) && psiAmt >= 20 && portTimer >= 1)
        {
            player.transform.position = new Vector3(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y, 0);
            partSys.Emit(15);
            psiAmt -= 20;
            portTimer = 0;
            poofTime = .2f;
            soundSystem.sharedInstance.Play(audioClipKey.TELEPORT);           
        }
        if(psiAmt <= 100)
        {
            psiAmt += (10 * Time.deltaTime);
        }
        if(psiAmt > 100)
        {
            psiAmt = 100;
        }
        if(poofTime >= 0)
        {
            vel = Vector3.zero;
            render.enabled = false;
 
            box.enabled = false;
            partSys.Emit(2);
        }
        else
        {
            render.enabled = true;
            box.enabled = true;
        }
        psiBar.fillAmount = (psiAmt / 100);
        portCool.fillAmount = portTimer;
	}
}
