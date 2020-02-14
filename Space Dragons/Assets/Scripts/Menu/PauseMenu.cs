using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseMenu : Singleton<PauseMenu>
{
    [SerializeField] GameObject pauseUI = null;
    [SerializeField] GameObject optionsUI = null;
    [SerializeField] Button menuButton = null;
    [SerializeField] Toggle controlToggle = null;

    public bool JoystickControls;

    public void Start()
    {
        JoystickControls = (PlayerPrefs.GetInt("JoystickControls") == 0);
        controlToggle.isOn = JoystickControls;
    }

    private void OnApplicationPause(bool pause)
    {
        LoadManager.Instance.Save();
    }
    private void OnApplicationQuit()
    {
        LoadManager.Instance.Save();
    }

    public void ToggleJoystickControls(bool toggled)
    {
        JoystickControls = toggled;
        PlayerPrefs.SetInt("JoystickControls", (JoystickControls) ? 0 : 1);

    }
    public void ToggleOptions()
    {
        optionsUI.SetActive(!optionsUI.activeSelf);
    }

    public void ReturnToMenu()
    {
        menuButton.interactable = false;
        LoadManager.Instance.Save();
        LoadingScreen.Instance.Show(SceneManager.LoadSceneAsync("Menu"));
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
