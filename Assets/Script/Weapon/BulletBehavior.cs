using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BulletBehavior : MonoBehaviour
{
    public GameObject flare;
     AudioSource audiodata;
    Rigidbody2D rg;
    public float flyingSpeed;
    TrailRenderer tr;
    // Start is called before the first frame update
    void Start()
    {
        audiodata = GetComponent<AudioSource>();
        rg = GetComponent<Rigidbody2D>();
        RecordData data = SaveSystem.LoadGameState();
        flyingSpeed += data.weaponLevel[data.weaponIndex];
  
        tr = GetComponent<TrailRenderer>();
        if(tr)
        tr.time = tr.time * (10 - data.weaponLevel[data.weaponIndex]) * 0.1f;
    }

    void Update()
    {
        
        if(gameObject.activeSelf)
        rg.velocity = transform.up * flyingSpeed;
    }
   
   
    void OnTriggerEnter2D(Collider2D col)
    {
        
        if (col.gameObject.tag == "Enemy" || col.gameObject.tag == "Border")
        {
            Instantiate(flare, transform.position, Quaternion.identity);
            gameObject.SetActive(false);
        }
            //Destroy(gameObject);
    }
}
