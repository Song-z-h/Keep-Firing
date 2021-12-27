using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBallColliderBehavior : MonoBehaviour
{
    Auto_Destroy_Fireballs a;
    // Start is called before the first frame update
    void Start()
    {
        a = gameObject.GetComponentInParent<Auto_Destroy_Fireballs>();
    }


    void OnParticleCollision(GameObject other)
    {
        if (other.CompareTag("Enemy"))
        {
            //Debug.Log("collide");
            other.GetComponent<EnemyMoviment>().Death();
        }

    }
}
