using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TutorialManager : Singleton<TutorialManager>
{
    [SerializeField] TextMeshProUGUI tipPrompt = null;
    [SerializeField] GameObject tipUI = null;
    [SerializeField] GameObject tipArrow = null;
    [SerializeField] List<TutorialOrder> Orders = null;
    [SerializeField] GameObject Next = null;
    [SerializeField] GameObject Back = null;

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
        CheckForEnds();
        tipPrompt.text = Orders[orderIndex].prompts[tipIndex].PromptText;
        tipUI.transform.localPosition = Vector3.Lerp(tipUI.transform.localPosition, Orders[orderIndex].prompts[tipIndex].UIWaypoint, 5.0f * Time.deltaTime);
        tipArrow.transform.localPosition = Vector3.Lerp(tipArrow.transform.localPosition, Orders[orderIndex].prompts[tipIndex].ArrowWaypoint, 5.0f * Time.deltaTime);
        tipArrow.transform.rotation = Quaternion.Lerp(tipArrow.transform.rotation, Quaternion.Euler(Orders[orderIndex].prompts[tipIndex].ArrowRotationWaypoint.x, Orders[orderIndex].prompts[tipIndex].ArrowRotationWaypoint.y, Orders[orderIndex].prompts[tipIndex].ArrowRotationWaypoint.z), 5.0f * Time.deltaTime);
    }

    public void SkipTips()
    {
        tipUI.SetActive(false);
        LoadingScreen.Instance.Show(SceneManager.LoadSceneAsync("Menu"));

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
    public void PreviousTip()
    {
        tipIndex--;
        if (tipIndex < 0)
        {
            tipIndex = 0;
            Orders[orderIndex].FocusedScreen.SetActive(false);
            orderIndex--;
            orderIndex = orderIndex < 0 ? 0 : orderIndex;

            tipIndex = Orders[orderIndex].prompts.Count-1;
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
    public void CheckForEnds()
    {
        if(tipIndex == 0 && orderIndex == 0)
        {
            Back.GetComponent<Button>().interactable = false;
        }
        else if(Orders[Orders.Count-1].FocusedScreen.activeInHierarchy && tipIndex == Orders[Orders.Count-1].prompts.Count-1)
        {
            Next.GetComponent<Button>().interactable = false;
        }
        else
        {
            Back.GetComponent<Button>().interactable = true;
            Next.GetComponent<Button>().interactable = true;
        }
    }
    public void ResetTips()
    {
        tipIndex = 0;
        orderIndex = 0;
    }

}
