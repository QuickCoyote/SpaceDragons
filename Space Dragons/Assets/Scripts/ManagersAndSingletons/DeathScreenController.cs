using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class DeathScreenController : MonoBehaviour
{
    public TextMeshProUGUI wavesSurvived = null;
    public TextMeshProUGUI moneyAccumulated = null;
    public TextMeshProUGUI returnText = null;

    float timer = 0;
    int timerMax = 20;

    public void Start()
    {
        wavesSurvived.text = "Waves Survived: " + LoadManager.Instance.saveData.CurrentWave.ToString();
        moneyAccumulated.text = "Money Accumulated: " + LoadManager.Instance.saveData.PlayerMoney.ToString();
        LoadManager.Instance.ResetSaveData();
        timer = timerMax;
    }

    void Update()
    {
        returnText.text = "Continuing in ... " + (int)timer + "s";
        timer -= 1 * Time.unscaledDeltaTime;
        if (timer <= 0)
        {
            ReturnToMenu();
            timer = timerMax;
        }
    }

    public void ReturnToMenu()
    {
        LoadingScreen.Instance.Show(SceneManager.LoadSceneAsync("Menu"));
    }
}
