using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExitButton : MonoBehaviour
{
    void Start()
    {
        Button b = GetComponent<Button>();
        b.onClick.AddListener(() => SceneTransitionManager.MoveToScene(SceneNames.MainMenu));

    }
}
