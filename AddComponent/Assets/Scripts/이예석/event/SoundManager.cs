using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    private static SoundManager instance;
    public static SoundManager Instance { get { return instance; } }




    public AudioClip[] soundeffect = new AudioClip[10];

    public AudioSource effectsource;

    


    private void Awake()
    {
        if (instance)
        {
            DestroyImmediate(gameObject);
            return;
        }
        instance = this;
    }



    public void SoundFx(int t)
    {
        
      //  soundeffect[t]

    }




}
