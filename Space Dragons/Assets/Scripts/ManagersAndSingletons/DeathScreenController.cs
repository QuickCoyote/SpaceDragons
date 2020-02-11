using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class DeathScreenController : MonoBehaviour
{
    public EnemyWaveManager waveManager = null;
    public PlayerController playerController = null;
    public TextMeshProUGUI wavesSurvived = null;
    public TextMeshProUGUI moneyAccumulated = null;
    public TextMeshProUGUI returnText = null;

    float timer = 0;
    int timerMax = 20;

    public void Start()
    {
        waveManager = WorldManager.Instance.enemyWaveManager;
        playerController = WorldManager.Instance.PlayerController;
        wavesSurvived.text = "Waves Survived: " + waveManager.currentWave.ToString();
        moneyAccumulated.text = "Money Accumulated: " + playerController.money.ToString();
        LoadManager.Instance.ResetSaveData();
        timer = timerMax;
    }

    void Update()
    {
        returnText.text = "Tap the Button to Return to Main Menu or wait " + (int)timer + "s";
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
