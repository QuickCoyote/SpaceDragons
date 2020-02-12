using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI playtext = null;
    [SerializeField] Button playbutton = null;

    public void Update()
    {
        if (LoadManager.Instance.saveData.CurrentWave == 0)
        {
            playtext.text = "NEW GAME";
        }
        else
        {
            playtext.text = "PLAY";
        }
    }

    public void StartGame()
    {
        playbutton.interactable = false;
        LoadingScreen.Instance.Show(SceneManager.LoadSceneAsync("SpaceWorld"));
    }

    public void ViewTutorial()
    {
        LoadingScreen.Instance.Show(SceneManager.LoadSceneAsync("Tutorial"));
    }

    public void ResetSave()
    {
        LoadManager.Instance.ResetSaveData();
       // playtext.text = "New Game";
    }

}
