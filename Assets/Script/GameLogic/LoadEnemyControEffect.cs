using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadEnemyControEffect : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        RecordData data = SaveSystem.LoadGameState();
        int i = 1;
        foreach(Transform child in transform)
        {
            if (i++ != data.weaponIndex) 
                child.gameObject.SetActive(false);
        }
    }

   
}
