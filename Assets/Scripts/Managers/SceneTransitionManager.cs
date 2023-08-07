using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public static class SceneNames
{
    public static readonly string MainMenu = "Main Menu";
    public static readonly string PlayerSetup = "Player Setup";
    public static readonly string Game = "Game";
    public static readonly string PauseScreen = "Pause Screen"; 
    public static readonly string FeedbackScreen = "Feedback Screen";
}

public static class SceneTransitionManager
{
    public static void MoveToNextScene()
    {
        int index = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(index + 1);
    }

    public static void MoveToScene(string name)
    {
        SceneManager.LoadScene(name);
    }
}