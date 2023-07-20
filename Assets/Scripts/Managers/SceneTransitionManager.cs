using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;


public static class SceneTransitionManager
{
    public static bool IsCurrentSceneGame
    {
        get => SceneManager.GetActiveScene().name.Equals("Game");
    }
    public static void MoveToNextScene()
    {
        int index = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(index + 1);
    }

    public static void MoveToGameScene()
    {
        SceneManager.LoadScene("Game");
    }
}