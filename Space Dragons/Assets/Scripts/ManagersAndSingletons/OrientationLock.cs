using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrientationLock : Singleton<OrientationLock>
{
    private new void Awake()
    {
        base.Awake();
        Application.targetFrameRate = 60;
    }

    public enum eOrientationType
    {
        LANDSCAPE_LEFT,
        PORTRAIT_UPSIDE_DOWN,
        PORTRAIT,
        AUTOROTATE
    }

    public eOrientationType orientationType = eOrientationType.LANDSCAPE_LEFT;

    void Start()
    {
        switch (orientationType)
        {
            case eOrientationType.LANDSCAPE_LEFT:
                Screen.orientation = ScreenOrientation.LandscapeLeft;
                break;
            case eOrientationType.PORTRAIT_UPSIDE_DOWN:
                Screen.orientation = ScreenOrientation.PortraitUpsideDown;
                break;
            case eOrientationType.PORTRAIT:
                Screen.orientation = ScreenOrientation.Portrait;
                break;
            case eOrientationType.AUTOROTATE:
                Screen.orientation = ScreenOrientation.AutoRotation;
                break;
        }
    }
}
