using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MainMenuManager : MonoBehaviour
{

    string[] gameCodes;
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
            SceneTransitionManager.MoveToGameScene();
        }
    }
}
