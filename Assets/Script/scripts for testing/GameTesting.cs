using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class GameTesting : MonoBehaviour
{
    /* [MenuItem("MyMenu/AddGemsForTesting")]

    static void AddGemsForTesting()
      {
          RecordData data = SaveSystem.LoadGameState();
          data.gems += 50000;
          SaveSystem.SaveGameState(data);
      }
      */
    [MenuItem("MyMenu/UpdateWeaponTo5")]
    static void UpdateWeaponTo5()
    {
        RecordData data = SaveSystem.LoadGameState();
        //data.weaponIndex = 2;
        data.weaponLevel[3] = 5;
        SaveSystem.SaveGameState(data);
    }
    
    [MenuItem("MyMenu/SwitchWeaponTo3")]
    static void SwitchWeaponToIce()
    {
        RecordData data = SaveSystem.LoadGameState();
        Debug.Log(data.weaponIndex);
        data.weaponIndex = 3;
        data.isEvilMode = false;
        SaveSystem.SaveGameState(data);
    }

    [MenuItem("MyMenu/SwitchWeaponTo2")]
    static void SwitchWeaponToFire()
    {
        RecordData data = SaveSystem.LoadGameState();
        Debug.Log(data.weaponIndex);
        data.weaponIndex = 2;
        data.isEvilMode = false;
        SaveSystem.SaveGameState(data);
    }
}
