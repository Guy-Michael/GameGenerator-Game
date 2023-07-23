using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundControls : MonoBehaviour
{
    [SerializeField] Sprite onSprite;
    [SerializeField] Sprite offSprite;
    AudioSource backgroundMusic;
    Button button;
    Image image;
    bool isOn;

    void Start()
    {
        isOn = true;

        button = GetComponent<Button>();
        button.onClick.AddListener(Toggle);

        image = GetComponent<Image>();
        backgroundMusic = GetComponent<AudioSource>();
        // backgroundMusic.Play();
    }

    public void Toggle()
    {
        isOn = !isOn;
        
        image.sprite = isOn ? onSprite : offSprite;
        Camera.main.GetComponent<AudioListener>().enabled = isOn;
        
        // if(isOn) 
        //     backgroundMusic.Play();
        // else 
        //     backgroundMusic.Stop();
        
        
    }
}
