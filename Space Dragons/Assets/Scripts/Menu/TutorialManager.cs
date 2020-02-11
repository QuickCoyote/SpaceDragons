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
        public List<TutorialPromptData> prompts;
    }

    void Start()
    {
        Orders[orderIndex].FocusedScreen.SetActive(true);
    }

    private void Update()
    {
        tipPrompt.text = Orders[orderIndex].prompts[tipIndex].PromptText;
        tipUI.transform.localPosition = Vector3.Lerp(tipUI.transform.localPosition, Orders[orderIndex].prompts[tipIndex].UIWaypoint, 5.0f * Time.deltaTime);
        tipArrow.transform.localPosition = Vector3.Lerp(tipArrow.transform.localPosition, Orders[orderIndex].prompts[tipIndex].ArrowWaypoint, 5.0f * Time.deltaTime);
        tipArrow.transform.rotation = Quaternion.Lerp(tipArrow.transform.rotation, Quaternion.Euler(Orders[orderIndex].prompts[tipIndex].ArrowRotationWaypoint.x, Orders[orderIndex].prompts[tipIndex].ArrowRotationWaypoint.y, Orders[orderIndex].prompts[tipIndex].ArrowRotationWaypoint.z), 5.0f * Time.deltaTime);
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
                orderIndex--;

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
