using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class RecordData
{
    public int gems;
    public int weaponIndex;
    public int[] weaponLevel;
    public float[] weaponMaxFireRate;
    public float[] weaponMinFireRate;


    //verison 03 added
    public bool[] weaponAvailability;
    public int[] weaponPrice;

    //version 04 added
    public int score;
    public string name;
    public string id;

    //version 06 added
    public bool isEvilMode;
    public bool loggedInWithGoogle;
    public string googleId;

   public RecordData()
    {
        gems = 0;
        weaponIndex = 0;
        weaponLevel = new int[10];
        weaponMaxFireRate = new float[10];
        weaponMinFireRate = new float[10];
        
        for(int i = 0; i < 10; i++)
        {
            
            weaponLevel[i] = 0;
            weaponMaxFireRate[i] = 40;
            weaponMinFireRate[i] = 10;
           
        }
        
    }
    
    public override string ToString() 
    {
        string s;
        s = "gems : " + gems + " id: " + id + " weaponLevel[1]: " + weaponLevel[1] + " weaponIndex: " + weaponIndex + " weaponAvailability[1]: " + weaponAvailability[1];
        return s;
    }
}
