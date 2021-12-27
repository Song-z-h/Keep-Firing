using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMSelector : MonoBehaviour
{
    AudioSource audiosource;
    
    // Start is called before the first frame update
    void Awake()
    {
        audiosource = GetComponent<AudioSource>();
       
        switch(Random.Range(0, 2))
        {
            case 0:
                audiosource.clip = Resources.Load<AudioClip>("Music/BGM/10 - buffy - old fashion - outro party");
                audiosource.volume = 1.0f;
                break;
            case 1:
                audiosource.clip = Resources.Load<AudioClip>("Music/BGM/BGM_Main");
                audiosource.volume = 0.7f;
                break;

        }

        audiosource.Play();
    }

}
