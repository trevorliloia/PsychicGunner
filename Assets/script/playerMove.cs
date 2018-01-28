using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent (typeof(controller2D))]
public class playerMove : MonoBehaviour, iDamageable {

    public float health = 100;
    public Image HPBar;

    public float jumpHeight = 4.5f;
    public float timeToApex = .3f;

    public bool stillJumping = false;


    public float moveSpeed = 6;
    float gravity;
    float jumpVelocity;
    public Vector3 velocity;
    float velXSmooth;
    public float accelTimeAir = .2f;
    public float accelTimeGround = .1f;


    public Vector2 jumpClimb;
    public Vector2 jumpOff;
    public Vector2 jumpLeap;
    public float wallSpeedMax = 2.25f;
    public float wallStick = .3f;
    public float timeToUnstick;

    public controller2D controller;

    public bool dieTest;

    Animator anim;
	// Use this for initialization
	void Start () {
        controller = GetComponent<controller2D>();
        anim = GetComponentInChildren<Animator>();

        gravity = -(2 * jumpHeight) / Mathf.Pow(timeToApex, 2);
        jumpVelocity = Mathf.Abs(gravity) * timeToApex;
        print("Gravity " + gravity + "  Jump Vel " + jumpVelocity);

	}
	
    public void TakeDamage(float damage)
    {
        health -= damage;

        if(health <= 0)
        {
            anim.SetBool("Die", true);
        }
    }

	// Update is called once per frame
	void Update () {
        HPBar.fillAmount = (health / 100);
        Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        int wallDirX = (controller.collisions.left) ? -1 : 1;

        float targetVelocityX = input.x * moveSpeed;
        anim.SetBool("Running", input.x != 0);
        velocity.x = Mathf.SmoothDamp(velocity.x, targetVelocityX, ref velXSmooth, (controller.collisions.below) ? accelTimeGround : accelTimeAir);

        bool wallSliding = false;

        if(controller.collisions.left || controller.collisions.right && !controller.collisions.below && velocity.y < 0)
        {
            wallSliding = true;
            
            if (velocity.y < -wallSpeedMax)
            { velocity.y = -wallSpeedMax; }

            if(timeToUnstick > 0)
            {
                velocity.x = 0;
                velXSmooth = 0;

                if(input.x != wallDirX && input.x != 0)
                {
                    timeToUnstick -= Time.deltaTime;
                }
                else
                {
                    timeToUnstick = wallStick;
                }
            }
            else
            {
                timeToUnstick = wallStick;
            }

        }

        if (controller.collisions.above || controller.collisions.below)
        {
            velocity.y = 0;
        }
        else if (velocity.y < 0)
        {
            velocity.y *= (1.03f);
        }
        anim.SetBool("Jumping", velocity.y != 0);


        if (Input.GetKeyDown(KeyCode.Space))
        {
            if(wallSliding)
            {
                if(wallDirX == input.x)
                {
                    velocity.x = -wallDirX * jumpClimb.x;
                    velocity.y = jumpClimb.y;
                }
                else if(input.x == 0)
                {
                    velocity.x = -wallDirX * jumpOff.x;
                    velocity.y = jumpOff.y;
                }
                else
                {
                    velocity.x = -wallDirX * jumpLeap.x;
                    velocity.y = jumpLeap.y;
                }
            }
            if (controller.collisions.below)
            velocity.y = jumpVelocity;
        }

        anim.SetBool("WallSlide", wallSliding);
        

       
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
        print(stillJumping);
	}
}
