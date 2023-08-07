using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MainMenuManager : MonoBehaviour
{

    string[] gameCodes;
    [SerializeField] GameObject warningText;

    void Start()
    {
        IAssetImporter importer = GetComponent<LocalAssetImporter>();
        this.gameCodes = importer.GetGameCodes();    

        GameEvents.GameTypeSelected.AddListener(OnGameTypeSelected);
    }

    void OnGameTypeSelected(string gameCode)
    {
        if(gameCodes.ToList().Contains(gameCode))
        {
            AnalyticsManager.gameCode = gameCode;
            SceneTransitionManager.MoveToScene(SceneNames.PlayerSetup);
        }

        else
        {
            warningText.SetActive(true);
            Timer.Fire(3, ()=> warningText.SetActive(false));
        }
    }
}
