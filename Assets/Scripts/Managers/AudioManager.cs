using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] AudioClip successSound;
    [SerializeField] AudioClip failSound;
    AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        GameEvents.PlayerMadeMatch.AddListener(PlaySuccessSound);
        GameEvents.PlayerFailedMatch.AddListener(PlayFailureSound);
    }

    public void PlaySuccessSound()
    {
        audioSource.PlayOneShot(successSound);
    }

    public void PlayFailureSound()
    {
        audioSource.PlayOneShot(failSound);
    }


    
}
