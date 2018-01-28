using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class homingMissile : MonoBehaviour
{

    [SerializeField] float damage, duration, speed;
    float durationReset;
    GameObject player;

    // Use this for initialization
    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        durationReset = duration;
    }

    // Update is called once per frame
    void Update()
    {
        Movement();

        duration -= Time.deltaTime;
        if (duration <= 0)
        {
            duration = durationReset;
            gameObject.SetActive(false);
        }
    }

    void Movement()
    {
        transform.position = Vector3.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            collision.GetComponent<playerMove>().TakeDamage(damage);
            duration = durationReset;
            gameObject.SetActive(false);

        }
    }
}

