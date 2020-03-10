using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UIBaseClass : MonoBehaviour
{
    [Header("UIBaseClass")]

    public GameObject UICanvas;
    public bool PauseOnly;
    public bool IsShop;
    public HelpScreenManager.eHelpScreens helpScreen = HelpScreenManager.eHelpScreens.NONE;

    public void Open()
    {
        if (PauseOnly)
        {
            UIManager.Instance.PauseTimeScale();
        }
        if (IsShop)
        {
            AudioManager.Instance.StopAll();
            AudioManager.Instance.Play("ShopEntrance");
            AudioManager.Instance.PlayRandomMusic("Shop Music");
        }
        AndroidManager.HapticFeedback();
        HelpScreenManager.Instance.CheckFirstTimeHelpScreen(helpScreen);
        UICanvas.SetActive(true);
    }
    public void Close()
    {
        if (UIManager.Instance.CurrentlyOpen == this)
        {
            UIManager.Instance.CurrentlyOpen = null;
        }
        if (IsShop)
        {
            AudioManager.Instance.StopAll();
            AudioManager.Instance.PlayRandomMusic("Battle Music");
        }
        if (PauseOnly) UIManager.Instance.ResumeTimeScale();
        UICanvas.SetActive(false);
        HelpScreenManager.Instance.CloseAllHelpScreens();
        AndroidManager.HapticFeedback();
    }

    public void OpenHelpScreen()
    {
        HelpScreenManager.Instance.OpenHelpScreen(helpScreen);
    }

    public void ToggleUI()
    {
        if (UICanvas.activeSelf)
        {
            Close();
        } else
        {
            UIManager.Instance.SetCurrentOpen(this);
        }
    }

    public void HideOnly()
    {
        UICanvas.SetActive(false);
        HelpScreenManager.Instance.CloseAllHelpScreens();
    }
}
