using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Firebase.Database;
using Firebase.Unity.Editor;

public class GameManager : MonoBehaviour
{
    int gems; //gems in this game round
    public Text gemTextInGame;
    public GameObject gemOnTopScreen;
    Animator gemAnimator;
    public static bool gameOver = false;
    public GameObject enhancearmament;
    EnhanceArmament ea;
    RecordData data;
    // Start is called before the first frame update
    void Start()
    {
        gems = 0;
        gemAnimator = gemOnTopScreen.GetComponent<Animator>();
        GameManager.gameOver = false;
        ea = enhancearmament.GetComponent<EnhanceArmament>();
        data = SaveSystem.LoadGameState();
    }


    public void AddGems()
    {
        //Debug.Log("Adding gems  +1");
       // Debug.Log(gems);
        gems++;
        gemTextInGame.text = gems.ToString();

        //fill energy bar
        if (enhancearmament.activeSelf)
        {
            if (!ea.IsAnimPlaying() || data.weaponIndex != 1)
            {
                ea.SetEnergyBar();
            }
            ea.energy ++;
        }
        
    }

    public void SaveGemsAtEnd()
    {
        gemAnimator.Play("Gem", -1, 0);
        SaveSystem.AddGems(gems);
    }


    

    

}
