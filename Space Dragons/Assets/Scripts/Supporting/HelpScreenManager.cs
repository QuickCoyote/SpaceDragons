using System;
using System.Collections.Generic;
using UnityEngine;

public class HelpScreenManager : Singleton<HelpScreenManager>
{
    [SerializeField] List<helpScreen> screens = new List<helpScreen>();

    [Serializable]
    public class helpScreen
    {
        public GameObject panel;
        public bool opened = true;
        public eHelpScreens screenID;
    }

    public enum eHelpScreens
    {
        NONE,
        SHIPYARD,
        OUTPOST,
        MAP
    }

    public void OpenHelpScreen(eHelpScreens screen)
    {
        for (int i = 0; i < screens.Count; i++)
        {
            if (screens[i].screenID == screen)
            {
                screens[i].panel.SetActive(true);
                screens[i].opened = true;
            }
        }
    }

    public void CheckFirstTimeHelpScreen(eHelpScreens screen)
    {
        for (int i = 0; i < screens.Count; i++)
        {
            if (screens[i].screenID == screen && !screens[i].opened)
            {
                screens[i].panel.SetActive(true);
                screens[i].opened = true;
            }
        }
    }

    public void CloseAllHelpScreens()
    {
        foreach (helpScreen s in screens)
        {
            s.panel.SetActive(false);
        }
    }

}
