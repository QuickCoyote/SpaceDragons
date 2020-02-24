using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class HelpScreenManager : Singleton<HelpScreenManager>
{
    [SerializeField] List<helpScreen> screens;

    [Serializable]
    public struct helpScreen
    {
        public GameObject panel;
        public bool opened;
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
        screens.First(e => e.screenID == screen).panel.SetActive(true);
    }
    public void CheckFirstTimeHelpScreen(eHelpScreens screen)
    {
       if (!screens.First(e => e.screenID == screen).opened) screens.First(e => e.screenID == screen).panel.SetActive(true);
    }

    public void CloseAllHelpScreens()
    {
        foreach(helpScreen s in screens)
        {
            s.panel.SetActive(false);
        }
    }

}
