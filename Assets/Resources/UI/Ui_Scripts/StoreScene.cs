using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StoreScene : MonoBehaviour
{
    public int price;
    AudioSource purchaseSound;
    GameState gs;
    public GameObject me;
    MeSkin meskin;
    // Start is called before the first frame update
    void Start()
    {
        RefleshStoreConditions();
        purchaseSound = GetComponent<AudioSource>();
        gs = GameObject.Find("GameState").GetComponent<GameState>();

        meskin = me.GetComponent<MeSkin>();
    }

   public void RefleshStoreConditions()
    {
         int children = transform.childCount;
       // Debug.Log(children);
        RecordData data = SaveSystem.LoadGameState();
        int childNum = 0;
        foreach (Transform child in transform)
        {

           //inside items[0~10]
            Transform kid = child.GetChild(1);
           // Debug.Log(kid);
           
            int index = 0;
            foreach (Transform kids in kid)
            {
                //inside potential
                if (index++ < data.weaponLevel[childNum])
                {
                    //Debug.Log(kids);
                    Image img = kids.GetComponent<Image>();
                    img.color = new Vector4(1, 1, 1, 1);
                }
               
               }

            //for button_upgrade
            kid = child.GetChild(2).GetChild(0);
            //Debug.Log(data.weaponLevel[childNum] + 1);
            int cost = 0;
            if (data.weaponAvailability[childNum])
            {
                 cost = (int)Mathf.Pow(2, (data.weaponLevel[childNum] + 1)) * price;
                
            }
            else
            {
                cost = data.weaponPrice[childNum];
            }

            //Debug.Log("cost" + cost);
            Text text = kid.GetComponentInChildren<Text>();
            if (data.weaponLevel[childNum] + 1 <= 8)
                text.text = cost.ToString();
            else
                text.text = "Max";


            //for button selection
            Image ima = child.GetChild(3).GetComponent<Image>();
            Button bt = child.GetChild(3).GetComponent<Button>();
            if (childNum == data.weaponIndex)
            {
                //button selection
                
                ima.color = new Vector4(1, 1, 1, 0.2f);
            }
            else
            {
                ima.color = new Vector4(1, 1, 1, 0);
            }

            if (!data.weaponAvailability[childNum])
            {
                ima.color = new Vector4(0, 0, 0, 0.4f);
                bt.interactable = false;
            }
            else
            {
                bt.interactable = true;
            }

            childNum++;
        }
               
    }

    public void UpgradeWeapon(int weaponIndex)
    {
        RecordData data = SaveSystem.LoadGameState();
        int cost = 0;
        if (data.weaponAvailability[weaponIndex] == false)
        {
            //need to buy first
            cost = data.weaponPrice[weaponIndex];
            if (data.gems >= cost && !data.weaponAvailability[weaponIndex])
            {
                //enough gems to buy it
                
                data.weaponAvailability[weaponIndex] = true;
                data.gems -= cost;
                purchaseSound.Play();
            }
        }
        else
        {
            //upgrade relative weapon
             cost = (int)Mathf.Pow(2, (data.weaponLevel[weaponIndex] + 1)) * price;
            if (data.gems >= cost && data.weaponLevel[weaponIndex] + 1 <= 8)
            {
                //enough gems to buy it
                
                data.weaponLevel[weaponIndex]++;
                data.gems -= cost;
                purchaseSound.Play();
            }
        }
       
        SaveSystem.SaveGameState(data);
        RefleshStoreConditions();   
        gs.RefleshGemsStore();

    }

    public void SelectWeapon(int index)
    {
        RecordData data = SaveSystem.LoadGameState();
        data.weaponIndex = index;
        SaveSystem.SaveGameState(data);
        RefleshStoreConditions();
        
        meskin.ChangeSkin();
    }
    
}
