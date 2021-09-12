using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    private void Awake()
    {
        instance = this;
    }
    public AudioClip music_themer1, music_themer2;

    public GameObject currentMusicObject;

    public AudioClip sfx_landing, sdx_cherry;
    //Sound Object
    public GameObject soundObject;

    public void PlaySFX(string sfxName)
    {
        switch(sfxName)
        {
            case "landing":    
                SoundObjectCreation(sfx_landing);
                break;
            case "cherry":
                SoundObjectCreation(sdx_cherry);
                break;
            default:
                break;
        }    
    }    
    void SoundObjectCreation(AudioClip clip)
    {
        //Create SoundsObject gameObject
        GameObject newObject = Instantiate(soundObject, transform);
        //Assign audio to its audiosource
        newObject.GetComponent<AudioSource>().clip = clip;
        //Play the audio
        newObject.GetComponent<AudioSource>().Play();
       
    }

    public void PlayMusic(string musicName)
    {
        switch (musicName)
        {
            case "musictheme1":
                MusicObjectCreation(music_themer1);
                break;

            case "musictheme2":
                MusicObjectCreation(music_themer2);
                break;

                default:
                break;
        }
    }

     void MusicObjectCreation(AudioClip clip)
    {
        //Check if there an existing music object, if so delete it
        if (currentMusicObject)
            Destroy(currentMusicObject);
        //Create SoundsObject gameObject
        currentMusicObject = Instantiate(soundObject, transform);
        //Assign audio to its audiosource
        currentMusicObject.GetComponent<AudioSource>().clip = clip;
        //Audio loop
        currentMusicObject.GetComponent<AudioSource>().loop = true;
        //Play the audio
        currentMusicObject.GetComponent<AudioSource>().Play();
        
    }


}
