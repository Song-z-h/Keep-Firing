using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Firebase;
using Firebase.Unity.Editor;
using Firebase.Database;
//currenly working on version04 of the game


public class GameState : MonoBehaviour
{
    //public int gems;
    public Text gems_uitext;
    public Text gems_uitext_Shop;
    public Text score_uitext;
    public GameObject registerNamePanel;
    public InputField inputFieldForNameRegister;
     

    public bool reset;
    // Start is called before the first frame update
    void Start()
    {
        if (reset)
            PlayerPrefs.DeleteAll();
       // DebugShowInfo();


        if (PlayerPrefs.GetInt("FIRSTTIMEOPENING", 1) == 1)
        {
            Debug.Log("First time opening");
            //Set first time opening to false
            PlayerPrefs.SetInt("FIRSTTIMEOPENING", 0);

            RecordData data = new RecordData();

            SaveSystem.SaveGameState(data);
        } else
        {
            // Debug.Log("NOT First Time Opening");
            RefleshInfo();
            RefleshGemsStore();
            Application.targetFrameRate = -1;
        }

        /*if (PlayerPrefs.GetInt("version03_gemBugFix", 1) == 1)
        {
            PlayerPrefs.SetInt("version03_gemBugFix", 0);
            if (PlayerPrefs.GetInt("version03", 1) == 0)
            {
                //compensation
                RecordData data = SaveSystem.LoadGameState();
                data.gems += 5000;
                SaveSystem.SaveGameState(data);
                RefleshInfo();
                RefleshGemsStore();
            }
        }*/

        if (PlayerPrefs.GetInt("version03", 1) == 1)
        {
            RecordData data = SaveSystem.LoadGameState();
            PlayerPrefs.SetInt("version03", 0);
            //first time open after update
            data.weaponAvailability = new bool[10];
            data.weaponPrice = new int[10];
            for (int i = 0; i < 10; i++)
            {
                data.weaponAvailability[i] = false;
                data.weaponPrice[i] = 1000;

            }
            data.weaponPrice[0] = 0;
            data.weaponAvailability[0] = true;
            SaveSystem.SaveGameState(data);
        }

        if (PlayerPrefs.GetInt("version05", 1) == 1)
        {
            RecordData data = SaveSystem.LoadGameState();
            PlayerPrefs.SetInt("version05", 0);
            //first time open after update
            data.score = 0;
            data.name = null;
            data.id = null;
            SaveSystem.SaveGameState(data);
            Debug.Log("version05!");
        }


        if (PlayerPrefs.GetInt("version06", 1) == 1)
        {
            RecordData data = SaveSystem.LoadGameState();
            PlayerPrefs.SetInt("version06", 0);
            //first time open after update
            data.isEvilMode = false;
            data.loggedInWithGoogle = false;
            data.googleId = null;
            SaveSystem.SaveGameState(data);
            Debug.Log("version06!");
        }

        if (PlayerPrefs.GetInt("version08", 1) == 1)
        {
            PlayerPrefs.SetInt("version08", 0);
            RecordData data = SaveSystem.LoadGameState();
            for (int i = 0; i < 10; i++)
            {

                data.weaponMaxFireRate[i] = 40;
                data.weaponMinFireRate[i] = 10;

            }
            data.weaponPrice[2] = 3500;
            SaveSystem.SaveGameState(data);
        }

    }

    public void ShowRegisterNamePanel()
    {
        if(PlayerPrefs.GetInt("registered", 1) == 1)
        {
            RecordData data = SaveSystem.LoadGameState();
            if(data.name == null || data.name == "")
            registerNamePanel.SetActive(true);
           // PlayerPrefs.SetInt("registered", 0);
        }
        
        
    }

    public void RegisterName(Text name)
    {
        string validName = name.text.ToString();
        //check name validation
        if(validName == "" || validName.Length <= 0 || validName.Length >= 10 || validName.Contains("fuck") || validName.Contains(" ") || validName.Contains("bitch"))
        {
            return;
        }


        


        //save name to local 

        RecordData data = SaveSystem.LoadGameState();
        data.name = name.text.ToString();
        


        // Set up the Editor before calling into the realtime database.
        FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://keepfiring-d0d46.firebaseio.com/");

        // Get the root reference location of the database.
        DatabaseReference reference = FirebaseDatabase.DefaultInstance.RootReference;

        Leaderboard leader = new Leaderboard(data.name, data.score);

        string json = JsonUtility.ToJson(leader);

        if(data.id == null)
        data.id = reference.Child("leader").Push().Key;

        reference.Child("leader").Child(data.id).SetRawJsonValueAsync(json);

        SaveSystem.SaveGameState(data);

        PlayerPrefs.SetInt("registered", 0);
        registerNamePanel.SetActive(false);


       /*if (PlayerPrefs.GetInt("fixTata", 1) == 1)
        {
             data = SaveSystem.LoadGameState();
            if (data.name == "Tata")
            {
                data.gems = 7073;
                data.weaponAvailability[1] = true;
                data.weaponAvailability[0] = true;
                data.weaponLevel[0] = 8;
                data.weaponLevel[1] = 8;
            }
            PlayerPrefs.SetInt("fixTata", 0);
        }*/
    }

    public void RefleshInfo()
    {
        RecordData data = SaveSystem.LoadGameState();
         gems_uitext.text = data.gems.ToString();
        score_uitext.text = data.score.ToString();
        DebugShowInfo();
    }

    public void RefleshGemsStore()
    {
        RecordData data = SaveSystem.LoadGameState();
        gems_uitext_Shop.text = data.gems.ToString();
    }

    public void DebugShowInfo()
    {
        RecordData data = SaveSystem.LoadGameState();
       // Debug.Log(data.ToString());
    }
    
    
}
