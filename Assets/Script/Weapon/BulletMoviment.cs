using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletMoviment : MonoBehaviour
{
     Rigidbody2D rg;
    public float speed;
    // Start is called before the first frame update
    void Start()
    {
        rg = FindObjectOfType<Rigidbody2D>();
        rg.velocity = transform.up * speed;
    }

   
}
