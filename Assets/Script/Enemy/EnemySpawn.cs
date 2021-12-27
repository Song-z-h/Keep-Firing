using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawn : MonoBehaviour
{
   
    public GameObject enemy;
    Vector3 randomPos;
     List<GameObject> EnemyPool;
    const int poolSize = 100;
    float accelerateRate, i, f;
    //SpriteRenderer spriteRenderer;
   
    // Start is called before the first frame update
    void Start()
    {
        EnemyPool = new List<GameObject>();
        randomPos = new Vector3(Random.Range(-8f, 8f), Random.Range(-9f, 9f), 0);
        for (int i = 0; i < poolSize; i++)
        {
            
            EnemyPool.Add(Instantiate(enemy, randomPos, enemy.transform.rotation));
            EnemyPool[i].SetActive(false);
        }

        accelerateRate = 0.8f;
        i = 1f;
        f = 2f;
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        float t = Time.time;
         //Debug.Log(t % 10);
        if (t % 10 <= 0.01 && t % 10 >= 0)
        {
            i *= accelerateRate;
            f *= accelerateRate;
          
        }
        //Debug.Log(i);
       // Debug.Log(f);
       if(!GameManager.gameOver)
        StartCoroutine(Spawn(i, f));
       
    }


    IEnumerator Spawn(float intervalI, float intervalF)
    {
        int num = Random.Range(0, 4);
        if(num == 0)
            randomPos = new Vector3(Random.Range(-8f, -4f), Random.Range(-9f, -4f), 0);
        else if(num == 1)
            randomPos = new Vector3(Random.Range(4f, 8f), Random.Range(-9f, -4f), 0);
        else if(num == 2 )
            randomPos = new Vector3(Random.Range(-8f, -4f), Random.Range(4f, 9f), 0);
        else
            randomPos = new Vector3(Random.Range(4f, 8f), Random.Range(4f, 9f), 0);
        for (int i = 0; i < poolSize; i++)
        {
            if (EnemyPool[i] != null)
            {
                if (!EnemyPool[i].activeSelf)
                {
                    EnemyPool[i].transform.position = randomPos;
                    yield return new WaitForSecondsRealtime(Random.Range(intervalI, intervalF));

                    EnemyPool[i].SetActive(true);
                    break;
                }
            }
       
        }
       
       
       
    }


    public void FreezeAllEnemies()
    {
        foreach(GameObject enemy in EnemyPool)
        {
           
            if (enemy.activeSelf)
            {
                //check if it is visible in the screen
               
               // spriteRenderer = enemy.GetComponent<SpriteRenderer>();
                if (enemy.GetComponent<EnemyControlEffect>().isVisible)
                {
                    enemy.GetComponent<EnemyControlEffect>().FreezeTarget();
                }
            }
        }
    }

    /*public void BurnTargetEnemies()
    {

        foreach (GameObject enemy in EnemyPool)
        {

            if (enemy.activeSelf)
            {
                //check if it is visible in the screen

                // spriteRenderer = enemy.GetComponent<SpriteRenderer>();
                if (enemy.GetComponent<EnemyControlEffect>().gotFire)
                {
                    enemy.GetComponent<EnemyControlEffect>().BurnTarget();
                }
            }
        }
    }*/

}
