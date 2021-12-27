using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMoviment : MonoBehaviour
{
   
    GameObject player;
    Vector2 playerPos;
    float angle;
    public float enemySpeed;
    Rigidbody2D rg;
    public GameObject flare;
    public GameObject canvas;
    public AudioClip audioclip;
    GameObject gmObject;
    GameManager gm;
    public bool isFrozen;
    EnemyControlEffect ece;
    float clipVolume;
    // Start is called before the first frame update
    void Start()
    {
        clipVolume = 1.0f;
        isFrozen = false;
        player = GameObject.Find("Me");
        playerPos = new Vector2(player.transform.position.x, player.transform.position.y);
        rg = FindObjectOfType<Rigidbody2D>();
        enemySpeed = 0.005f;
        canvas = GameObject.Find("Canvas");
        gmObject = GameObject.Find("GameManager");
        gm = gmObject.GetComponent<GameManager>();
        ece = gameObject.GetComponent<EnemyControlEffect>(); 

        RecordData data = SaveSystem.LoadGameState();
        int s = (int)data.weaponIndex;
        //s = 2;
        switch (s)
        {
            case 1:
                audioclip = Resources.Load<AudioClip>("Music/Sound Effect/icesound");
                clipVolume = 0.5f;
                break;

            case 2:
                audioclip = Resources.Load<AudioClip>("Music/Sound Effect/firehit");
                    break;

            case 3:
                audioclip = Resources.Load<AudioClip>("Music/Sound Effect/Weapon3/rockhit");
                break;

            default:
                audioclip = Resources.Load<AudioClip>("Music/Sound Effect/Hit Default");
                break;
        }

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(gameObject.activeSelf)
            //if(Time.timeScale != 0)
            if (!GameManager.gameOver)
            {
                if(!isFrozen)
                MoveTo(playerPos);
            }
       
    }

    

    void MoveTo(Vector2 targetPoint)
    {
        Vector2 myPos = new Vector2(transform.position.x, transform.position.y);
        Vector2 dir = targetPoint - myPos;
        angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));

        transform.position = Vector3.Lerp(transform.position, targetPoint, enemySpeed);
        

    }

   


    public void Death()
    {
        if (gameObject.activeSelf)
        {
            
            ece.fronzeEffect.SetActive(false);
            isFrozen = false;
            ece.isVisible = false;
            gameObject.SetActive(false);

            //StartCoroutine(Play_Sound_Before_Dying());

            gm.AddGems();
        }
    }
    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.CompareTag("Rock"))
        {
            Death();
             AudioSource.PlayClipAtPoint(audioclip, new Vector3(0, 0, -10), clipVolume);
            //Instantiate(flare, transform.position, Quaternion.identity);
        }
      
    }

    void OnTriggerEnter2D(Collider2D col)
    {


        if (!col.CompareTag("VisibleZone") && !col.CompareTag("FireZone"))  
        {
            Death();
            AudioSource.PlayClipAtPoint(audioclip, new Vector3(0, 0, -10), clipVolume);
            Instantiate(flare, transform.position, Quaternion.identity);
        }


        /*if (col.tag == "Bullets")
            gm.AddGems();*/
        


            if (col.name == "Me")
            {
            //Debug.Log("Die");
            GameManager.gameOver = true;
                 gm.SaveGemsAtEnd();
                // Debug.Log("save Gems");
                canvas.GetComponent<Canvas>().enabled = true;
                //Time.timeScale = 0;
        }


    }

    IEnumerator Play_Sound_Before_Dying()
    {
            
        yield return new WaitForSeconds(1);
        
    }
}
