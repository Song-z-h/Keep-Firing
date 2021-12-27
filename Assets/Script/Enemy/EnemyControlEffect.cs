using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyControlEffect : MonoBehaviour
{
    public GameObject fronzeEffect;
    public bool isVisible;
    //public bool gotFire;

    void Start()
    {
        ResetControlStatus();
    }

    public void FreezeTarget()
    {
        fronzeEffect.SetActive(true);
        gameObject.GetComponent<EnemyMoviment>().isFrozen = true;
    }

    public void BurnTarget()
    {
        ResetControlStatus();
        gameObject.GetComponent<EnemyMoviment>().Death();
    }

    public void SmashTarget()
    {
        ResetControlStatus();
        gameObject.GetComponent<EnemyMoviment>().Death();
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        //frozen range for ice weapon index1
        if (col.CompareTag("VisibleZone"))
        {
            isVisible = true;
        }

        //this item is inside  the fireball zone
      
        /*if (col.CompareTag("FireZone"))
        {
            gotFire = true;
        }*/
    }

    public void ResetControlStatus()
    {
        isVisible = false;
        //gotFire = false;
    }
}
