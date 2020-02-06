using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI playtext = null;

    public void Start()
    {
        if (LoadManager.Instance.saveData == new LoadManager.SaveData())
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
        LoadingScreen.Instance.Show(SceneManager.LoadSceneAsync("SpaceWorld"));
    }

    public void ResetSave()
    {
        LoadManager.Instance.ResetSaveData();
        playtext.text = "New Game";
    }

}
