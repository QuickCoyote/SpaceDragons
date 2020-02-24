using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UIBaseClass : MonoBehaviour
{
    [Header("UIBaseClass")]

    public GameObject UICanvas;
    public bool PauseOnly;
    public HelpScreenManager.eHelpScreens helpScreen = HelpScreenManager.eHelpScreens.NONE;

    public void Open()
    {
        if (PauseOnly) UIManager.Instance.PauseTimeScale();
        HelpScreenManager.Instance.CheckFirstTimeHelpScreen(helpScreen);
        UICanvas.SetActive(true);
    }
    public void Close()
    {
        if (PauseOnly) UIManager.Instance.ResumeTimeScale();
        UICanvas.SetActive(false);
        HelpScreenManager.Instance.CloseAllHelpScreens();
    }

    public void OpenHelpScreen()
    {
        HelpScreenManager.Instance.OpenHelpScreen(helpScreen);
    }

    public void ToggleUI()
    {
        if (UICanvas.activeSelf)
        {
            if (UIManager.Instance.CurrentlyOpen == this)
            {
                UIManager.Instance.CurrentlyOpen = null;
            }
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
