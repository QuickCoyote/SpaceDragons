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
        foreach(helpScreen hpScreen in screens)
        {
            if(hpScreen.screenID == screen)
            {
                hpScreen.panel.SetActive(true);
                hpScreen.opened = true;
                break;
            }
        }
    }

    public void CheckFirstTimeHelpScreen(eHelpScreens screen)
    {
        foreach (helpScreen hpScreen in screens)
        {
            if (hpScreen.screenID == screen && !hpScreen.opened)
            {
                hpScreen.panel.SetActive(true);
                hpScreen.opened = true;
                break;
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
