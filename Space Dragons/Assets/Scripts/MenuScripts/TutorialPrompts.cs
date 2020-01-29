using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TutorialPrompts : Singleton<TutorialPrompts>
{
    [SerializeField] TextMeshProUGUI tipPrompt = null;
    [SerializeField] GameObject tipUI = null;
    [SerializeField] Slider tipTimerBar = null;
    public bool countdowntips = true;
    public float tipMax = 8.0f;
    public float tiptimer = 8.0f;
    public int tipIndex = 0;

    string[] prompts = {
        "Welcome to SPACE DRAGONS! Here's a few TIPS to get you started.",
        "You can always press CLOSE, and access the TIPS later from the PAUSE MENU.",
        "TAP or DRAG with your finger to pilot your own certified DRAGON Spaceship.",
        "While your finger is down, your DRAGON's weapons will fire.",
        "Behind your DRAGON, is a TURRET. This will automatically attack any ENEMIES for you.",
        "If you find an ASTEROID, you can fire at it to collect MATERIALS, which you can sell at a SPACEPORT.",
        "Follow the TRACKER arrow on your MINIMAP to find the nearest SPACEPORT.",
        "Certain SPACEPORTS will offer more currency for certain MATERIALS.",
    };

    void Start()
    {
        tipPrompt.text = prompts[tipIndex];
        tipTimerBar.maxValue = tipMax;
    }

    void Update()
    {
        if (countdowntips)
        {
            tiptimer -= Time.deltaTime;
            tipTimerBar.value = tipMax - tiptimer;
            if (tiptimer < 0.0f)
            {
                tiptimer = tipMax;
                tipIndex++;
                tipPrompt.text = prompts[tipIndex];

                if (tipIndex == prompts.Length)
                {
                    countdowntips = false;
                    tipUI.SetActive(false);
                }
            }
        }
    }

    public void SkipTips()
    {
        tipUI.SetActive(false);
        countdowntips = false;
    }
    public void NextTip()
    {
        tiptimer = tipMax;
        tipIndex++;
        if (tipIndex == prompts.Length)
        {
            countdowntips = false;
            tipUI.SetActive(false);
        }
        else
        {
            tipPrompt.text = prompts[tipIndex];
        }
    }
    public void ResetTips()
    {
        countdowntips = true;
        tiptimer = tipMax;
        tipIndex = 1;
        tipPrompt.text = prompts[tipIndex];

    }
}
