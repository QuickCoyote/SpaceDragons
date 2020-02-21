using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIManager : Singleton<UIManager>
{
    List<UIBaseClass> AllUI = new List<UIBaseClass>();
    public UIBaseClass CurrentlyOpen = null;

    private void Start()
    {
        AllUI = FindObjectsOfType<UIBaseClass>().ToList();
        foreach (UIBaseClass ui in AllUI)
        {
            ui.HideOnly();
        }
    }

    private void Update()
    {
        if (LoadingScreen.Instance.IsLoadingOpen)
        {
            AllUI = FindObjectsOfType<UIBaseClass>().ToList();
            foreach (UIBaseClass ui in AllUI)
            {
                ui.HideOnly();
            }
        }
    }

    public void ResumeTimeScale()
    {
        Time.timeScale = 1;
        foreach(UIBaseClass ui in AllUI.Where(ui=> ui.PauseOnly))
        {
            ui.HideOnly();
        }
    }

    public void PauseTimeScale()
    {
        Time.timeScale = 0;
    }

    public void SetCurrentOpen(UIBaseClass other)
    {
        if (!LoadingScreen.Instance.IsLoadingOpen)
        {
            if (CurrentlyOpen)
            {
                CurrentlyOpen.Close();
            }
            CurrentlyOpen = other;
            other.Open();
        }
    }

    public bool IsPointerOverUIObject()
    {
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
        return results.Count > 0;
    }
}
