using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowMatchesButton : MonoBehaviour
{
    [SerializeField] GameObject matchesContainer;

    void Start()
    {
        matchesContainer.SetActive(false);
        Button button = GetComponent<Button>();
        button.onClick.AddListener(() => matchesContainer.SetActive(true));
    }
}
