using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class DeathScreenController : MonoBehaviour
{
    public EnemyWaveManager waveManager = null;
    public TextMeshProUGUI wavesSurvived = null;
    
    void Update()
    {
        wavesSurvived.text = "Waves Survived: " + waveManager.currentWave.ToString();
    }

    public void ReturnToMenu()
    {
        LoadManager.Instance.ResetSaveData();
        LoadingScreen.Instance.Show(SceneManager.LoadSceneAsync("Menu"));
    }
}
