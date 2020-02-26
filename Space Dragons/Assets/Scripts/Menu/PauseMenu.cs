using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseMenu : UIBaseClass
{
    [SerializeField] GameObject optionsUI = null;
    [SerializeField] Button menuButton = null;
    [SerializeField] Toggle controlToggle = null;

    private void OnApplicationPause(bool pause)
    {
        UIManager.Instance.SetCurrentOpen(this);
        LoadManager.Instance.Save();
    }
    private void OnApplicationQuit()
    {
        LoadManager.Instance.Save();
    }

    public void ToggleJoystickControls(bool toggled)
    {
        PlayerPrefs.SetInt("JoystickControls", (toggled) ? 0 : 1);
        AndroidManager.HapticFeedback();

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
        AndroidManager.HapticFeedback();

    }

    public new void Open()
    {
        base.Open();
        controlToggle.isOn = (PlayerPrefs.GetInt("JoystickControls") == 0);
    }

}
