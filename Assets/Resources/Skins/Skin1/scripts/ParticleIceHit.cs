using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleIceHit : MonoBehaviour
{
     ParticleSystem part;
    public List<ParticleCollisionEvent> collisionEvents;
    // Start is called before the first frame update
    void Start()
    {
        part = GetComponent<ParticleSystem>();
        collisionEvents = new List<ParticleCollisionEvent>();
    }

    void OnParticleCollision(GameObject other)
    {
        if (other.CompareTag("Enemy"))
        {
            other.GetComponent<EnemyMoviment>().isFrozen = true;
            other.GetComponent<EnemyControlEffect>().FreezeTarget();
        }
       
    }
}
