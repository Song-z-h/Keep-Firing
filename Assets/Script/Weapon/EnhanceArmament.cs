using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnhanceArmament : MonoBehaviour
{
    public Slider energybar;
    public int energy;
    public Button ultiButton;
    EnemySpawn es;
    public GameObject buttonImgOutline;
    public GameObject buttonImg;
    public GameObject ultiObject;
    Animator animator;
    AudioSource audioSource;
    string animName;
    Vector3 mousePos;
    RecordData data;
    // fire weaponindex 2
    public GameObject fireBallFalls;
    public GameObject fireBallCollider;
    public int canSummonFireball; 
    
    void Awake()
    {
        canSummonFireball = 0;
         data = SaveSystem.LoadGameState();
        switch (data.weaponIndex)
        {
         
            case 1:
                animName = "IceFrozenEffect";
                buttonImgOutline.GetComponent<Image>().sprite = Resources.Load<Sprite>("Skins/Skin1/Crystal");
                buttonImg.GetComponent<Image>().sprite = Resources.Load<Sprite>("Skins/Skin1/Crystal");
                //  buttonImgOutline.transform.localScale = new Vector3(1.5f, 1.5f, 1f);
                // buttonImg.transform.localScale = new Vector3(1.26f, 1.26f, 1f);
                energybar.maxValue = 50;
                break;
            case 2:
                animName = null;
                buttonImgOutline.GetComponent<Image>().sprite = Resources.Load<Sprite>("Skins/Skin2/fireball");
                buttonImg.GetComponent<Image>().sprite = Resources.Load<Sprite>("Skins/Skin2/fireball");
                // buttonImgOutline.transform.localScale = new Vector3(2.2f, 2.2f, 1f);
                // buttonImg.transform.localScale = new Vector3(1.85f, 1.85f, 1f);
                energybar.maxValue = 150;
                break;

            case 3:
                animName = "RockRollingEffect";
                buttonImgOutline.GetComponent<Image>().sprite = Resources.Load<Sprite>("Skins/Skin3/RockButton");
                buttonImg.GetComponent<Image>().sprite = Resources.Load<Sprite>("Skins/Skin3/RockButton");
                energybar.maxValue = 10;
                break;
            default:
                gameObject.SetActive(false);
                ultiObject.SetActive(false);
                return;
        }
        energy = 0;
        es = FindObjectOfType<EnemySpawn>();
        ultiObject = ultiObject.transform.GetChild(data.weaponIndex - 1).gameObject;
        animator = ultiObject.GetComponent<Animator>();
        audioSource = ultiObject.GetComponent<AudioSource>();
        SetEnergyBar();
        
    }
    
    void Update()
    {
        if(canSummonFireball > 0)
        if (Time.timeScale != 0)
            if (Input.GetMouseButtonDown(0))
            {
                canSummonFireball--;
                mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                mousePos.z = 0;
                Instantiate(fireBallFalls, mousePos, Quaternion.identity);
                    //Instantiate(fireBallCollider, mousePos, Quaternion.identity);
            }
    }

    public void SetEnergyBar()
    {
        energybar.value = energy;
        if (energybar.value == energybar.maxValue)
        {
            //ready to realse the power!  Enhance Armament
            ultiButton.interactable = true;
            buttonImgOutline.SetActive(true);
            ultiObject.SetActive(true);

        }
        else
        {
            ultiButton.interactable = false;
            buttonImgOutline.SetActive(false);
            ultiObject.SetActive(false);
            
           
        }
    }


    public void EnhanceArmamentActicate()
    {

        StartCoroutine(Wait()); 
    }

    IEnumerator Wait()
    {
        energy = 0;
        energybar.value = energy;
        ultiButton.interactable = false;
        buttonImgOutline.SetActive(false);
        if(animName != null)
        animator.Play(animName, -1, 0);

        switch (data.weaponIndex)
        {
            case 2:
                canSummonFireball = 2;
                break;
        }

        yield return new WaitForSecondsRealtime(2.5f);
        
        if(data.weaponIndex != 3)
            audioSource.Play();

        switch (data.weaponIndex)
        {
            case 1:
                es.FreezeAllEnemies();
                break;
            
        }
        
        
        yield return new WaitForSecondsRealtime(1f);
        SetEnergyBar();

        yield return new WaitForSecondsRealtime(4f);
        canSummonFireball = 0;


    }


    public bool IsAnimPlaying()
    {
        if (gameObject.activeSelf)
        {
            if (this.animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1 &&
            this.animator.GetCurrentAnimatorStateInfo(0).IsName(animName)
            )
                return true;
            else
                return false;
        }

        return false;
        
    }
}
