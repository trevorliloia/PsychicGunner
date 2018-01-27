using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class timedDestroy : MonoBehaviour
{
    [SerializeField] float duration;

    private void Start()
    {
        Invoke("DestroySelf", duration);
    }

    void DestroySelf()
    {
        Destroy(gameObject);
    }

}
