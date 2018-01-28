using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDeath : MonoBehaviour
{
    public GameObject parent;
    public void Death()
    {
        parent.SetActive(false);
    }
}
