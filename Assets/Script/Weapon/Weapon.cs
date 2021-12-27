using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public float fireRate;
    public GameObject bullet;
    const int bulletPoolSize = 100;
    float count;
    List<GameObject> bulletPool;
    int index;
    AudioClip clip;
    // Start is called before the first frame update
    void Start()
    {
      
        index = 0;
        count = fireRate;
        RecordData data = SaveSystem.LoadGameState();
        fireRate = data.weaponMaxFireRate[data.weaponIndex]
            - data.weaponLevel[data.weaponIndex] * 4;

        bulletPool = new List<GameObject>();

        int s = (int)data.weaponIndex;
        //s = 2;
        switch (s)
        {
            case 1:
                bullet = Resources.Load<GameObject>("Skins/Skin1/tide_bullet");
                break;
            case 2:
          
                bullet = Resources.Load<GameObject>("Skins/Skin2/fire_bullet");
                //clip = Resources.Load<AudioClip>("Music/Sound Effect/Weapon2/fireballwave_old");
                break;
            case 3:
                bullet = Resources.Load<GameObject>("Skins/Skin3/rock_bullet");       
                break;

            default:
                bullet = Resources.Load<GameObject>("Sprite/bullet");
                break;
        }

        clip = bullet.GetComponent<AudioSource>().clip;

        for (int i = 0;i < bulletPoolSize; i++)
        {
            bulletPool.Add(Instantiate(bullet, transform.position, transform.rotation));
            bulletPool[i].SetActive(false);
           
        }
       
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Input.GetMouseButton(0))
        {
            count++;
            if(count % fireRate == 0)
            {
                
                Shoot();
            }
        }
    }
    

    void Shoot()
    {
        if(!GameManager.gameOver)
        {
           // for(int i = 0; i < bulletPoolSize; i++)
          //  {
                if (!bulletPool[index].activeSelf)
                {
                    bulletPool[index].transform.position = transform.position;
                    bulletPool[index].transform.rotation = transform.rotation;

                    bulletPool[index].SetActive(true);
                // break;
                    index++;
                    index %= bulletPoolSize;
                }
            // }
            AudioSource.PlayClipAtPoint(clip, new Vector3(0, 0, -10));
        }
        //Instantiate(bullet, transform.position, transform.rotation);
    }
}
