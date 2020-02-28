using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class DeathScreenController : MonoBehaviour
{
    public TextMeshProUGUI wavesSurvived = null;
    public TextMeshProUGUI moneyAccumulated = null;
    public TextMeshProUGUI returnText = null;

    public Button button = null;

    float timer = 0;
    int timerMax = 20;
    bool continued = false;
    public void Start()
    {
        wavesSurvived.text = "Waves Survived: " + LoadManager.Instance.saveData.CurrentWave.ToString();
        moneyAccumulated.text = "Money Accumulated: " + LoadManager.Instance.saveData.PlayerMoney.ToString();
        LoadManager.Instance.ResetSaveData();
        timer = timerMax;
    }

    void Update()
    {
        if (!continued)
        {
            returnText.text = "Continuing in ... " + (int)timer + "s";
            timer -= 1 * Time.unscaledDeltaTime;
            if (timer <= 0)
            {
                ReturnToMenu();
                continued = true;
            }
        }
    }

    public void ReturnToMenu()
    {
        continued = true;
        button.interactable = false;
        LoadingScreen.Instance.Show(SceneManager.LoadSceneAsync("Menu"));
    }
}
