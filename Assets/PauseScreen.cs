using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseScreen : MonoBehaviour
{
    Button resumeButton;
    void Start()
    {
        resumeButton = transform.Find("Resume Button").GetComponentInChildren<Button>();
        resumeButton.onClick.AddListener(Resume);
    }

    public void Pause()
    {
        Time.timeScale = 0;
        gameObject.SetActive(true);
    }

    void Resume()
    {
        Time.timeScale = 1;
        gameObject.SetActive(false);
    }
}
