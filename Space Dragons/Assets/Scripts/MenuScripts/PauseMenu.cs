using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : Singleton<PauseMenu>
{
    [SerializeField] GameObject pauseUI = null;

    public void ToggleOptions()
    {
        AudioManager.Instance.ToggleUIDisplay();
    }

    public void ReturnToMenu()
    {
        SceneManager.LoadScene("Menu");
    }

    public void RestartTutorial()
    {
        TutorialPrompts.Instance.ResetTips();
    }

    public void PauseGame()
    {
        pauseUI.SetActive(true);
        Time.timeScale = 0;
    }

    public void ResumeGame()
    {
        Time.timeScale = 1;
        pauseUI.SetActive(false);
    }
}
