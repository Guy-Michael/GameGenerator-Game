using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseButton : MonoBehaviour
{
    [SerializeField] GameObject pauseScreen;

    void Start()
    {
        Button button = GetComponent<Button>();
        button.onClick.AddListener(OnPause);    
    }

    void OnPause()
    {
        pauseScreen.SetActive(true);
        pauseScreen.GetComponent<PauseScreen>().Pause();
    }
}
