using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TutorialPrompts : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI tipPrompt = null;
    [SerializeField] GameObject tipUI = null;
    public bool countdowntips = true;
    public float tiptimer = 5.0f;
    public int tipIndex = 0;

    [SerializeField] string[] prompts = {
        "Welcome to SPACE DRAGONS! Here's a few TIPS to get you started. You can always press CLOSE, and access the TIPS later from the PAUSE MENU.",
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
    }

    void Update()
    {
        if (countdowntips)
        {
            tiptimer -= Time.deltaTime;
            if (tiptimer < 0.0f)
            {
                tiptimer = 5.0f;
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
        tiptimer = 5.0f;
        tipIndex++;
        if (tipIndex == prompts.Length)
        {
            countdowntips = false;
            tipUI.SetActive(false);
        }
        tipPrompt.text = prompts[tipIndex];
    }
    public void ResetTips()
    {
        countdowntips = true;
        tiptimer = 5.0f;
        tipIndex = 1;
        tipPrompt.text = prompts[tipIndex];

    }
}
