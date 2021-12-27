using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Auto_Destroy_Fireballs : MonoBehaviour
{
    Animator animator;
    EnemySpawn es;
    // Start is called before the first frame update
    void Start()
    {
        es = GameObject.Find("EnemySpawn").GetComponent<EnemySpawn>();
        animator = GetComponent<Animator>();
        animator.Play("fireballFall", -1, 0);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!IsAnimPlaying())
        {
           // es.BurnTargetEnemies();
            Destroy(gameObject);
        }
    }

   /* public void BurningTarget()
    {
        es.BurnTargetEnemies();
    }*/

    public bool IsAnimPlaying()
    {
        if (gameObject.activeSelf)
        {
            if (this.animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1 
            )
                return true;
            else
                return false;
        }

        return false;

    }
}
