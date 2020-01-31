﻿using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : Singleton<PauseMenu>
{
    [SerializeField] GameObject pauseUI = null;
    [SerializeField] GameObject optionsUI = null;

    public void ToggleOptions()
    {
        optionsUI.SetActive(!optionsUI.activeSelf);
    }

    public void ReturnToMenu()
    {
        LoadingScreen.Instance.Show(SceneManager.LoadSceneAsync("Menu"));
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