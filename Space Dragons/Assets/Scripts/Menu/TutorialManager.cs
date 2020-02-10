using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TutorialManager : Singleton<TutorialManager>
{
    [SerializeField] TextMeshProUGUI tipPrompt = null;
    [SerializeField] GameObject tipUI = null;
    [SerializeField] GameObject tipArrow = null;
    [SerializeField] List<TutorialOrder> Orders = null;

    int tipIndex = 0;
    int orderIndex = 0;

    [Serializable]
    struct TutorialOrder
    {
        public GameObject FocusedScreen;
        public List<TutorialPrompt> prompts;
    }
    [Serializable]
    struct TutorialPrompt
    {
        public string prompt;
        public Transform UIWaypoint;
        public Transform ArrowWaypoint;
    }

    void Start()
    {
        Orders[orderIndex].FocusedScreen.SetActive(true);
    }

    private void Update()
    {
        tipPrompt.text = Orders[orderIndex].prompts[tipIndex].prompt;
        tipUI.transform.position = Vector3.Lerp(tipUI.transform.position, Orders[orderIndex].prompts[tipIndex].UIWaypoint.position, 5.0f * Time.deltaTime);
        tipArrow.transform.position = Vector3.Lerp(tipArrow.transform.position, Orders[orderIndex].prompts[tipIndex].ArrowWaypoint.position, 5.0f * Time.deltaTime);
        tipArrow.transform.rotation = Quaternion.Lerp(tipArrow.transform.rotation, Orders[orderIndex].prompts[tipIndex].ArrowWaypoint.rotation, 5.0f * Time.deltaTime);
    }

    public void SkipTips()
    {
        tipUI.SetActive(false);
        //go to game or menu
        Debug.Log("Exit Tutorial");
    }
    public void NextTip()
    {
        tipIndex++;
        if (tipIndex == Orders[orderIndex].prompts.Count)
        {
            tipIndex = 0;
            Orders[orderIndex].FocusedScreen.SetActive(false);
            orderIndex++;
            if (orderIndex >= Orders.Count)
            {
                SkipTips();
            }
            else
            {
                Orders[orderIndex].FocusedScreen.SetActive(true);
            }
        } 
        

    }
    public void ResetTips()
    {
        tipIndex = 0;
        orderIndex = 0;
    }

}
