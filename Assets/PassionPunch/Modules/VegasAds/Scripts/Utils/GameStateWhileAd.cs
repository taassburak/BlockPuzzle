using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStateWhileAd : MonoBehaviour
{

    public AudioSource[] audioSources;
    public float[] lastVolumes;

    public void PauseGame()
    {
        Time.timeScale = 0;
        audioSources = FindObjectsOfType<AudioSource>();
        if (audioSources.Length > 0)
        {
            lastVolumes = new float[audioSources.Length];

            for (int i = 0; i < lastVolumes.Length; i++)
            {
                lastVolumes[i] = audioSources[i].volume;
                audioSources[i].volume = 0f;
            }
        }
    }

    public void ResumeGame()
    {
        Time.timeScale = 1;
        if (audioSources != null && audioSources.Length > 0)
        {
            for (int i = 0; i < lastVolumes.Length; i++)
            {
                if (audioSources[i] != null)
                {
                    audioSources[i].volume = lastVolumes[i];
                }
            }
            audioSources = null;
            lastVolumes = null;
        }
    }
    /*
    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.P))
        {
            PauseGame();
        }
        else if(Input.GetKeyUp(KeyCode.R))
        {
            ResumeGame();
        }
    }
    */
}
