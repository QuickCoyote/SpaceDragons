using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelpScreen : MonoBehaviour
{
    [SerializeField] GameObject HelpScreenObject = null;

    public void ToggleHelpScreen()
    {
        HelpScreenObject.SetActive(!HelpScreenObject.activeSelf);
    }

    public void CloseHelpScreen()
    {
        HelpScreenObject.SetActive(false);
    }

}
