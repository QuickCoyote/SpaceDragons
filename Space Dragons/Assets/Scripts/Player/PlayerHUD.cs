using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHUD : UIBaseClass 
{
    [Header("UIBaseClass")]
    public GameObject UICanvas;
    public bool PauseOnly;

    [SerializeField] GameObject JoystickControls = null;
    [SerializeField] GameObject TouchControls = null;

    [SerializeField] Toggle controlToggle = null;
    [SerializeField] Toggle thrusterToggle = null;
    [SerializeField] TextMeshProUGUI HUD_Money_Text = null;
    [SerializeField] TextMeshProUGUI HUD_Fuel_Text = null;
    [SerializeField] TextMeshProUGUI HUD_Mothership_Text = null;
    [SerializeField] TextMeshProUGUI HUD_ETA_Text = null;
    [SerializeField] TextMeshProUGUI HUD_Distance_Text = null;
    [SerializeField] Image CaptainIcon = null;
    [SerializeField] Image ShipIcon = null;



    private void Start()
    {
        controlToggle.isOn = (PlayerPrefs.GetInt("JoystickControls") == 0);
        JoystickControls.SetActive(PlayerPrefs.GetInt("JoystickControls") == 0);
        TouchControls.SetActive(PlayerPrefs.GetInt("JoystickControls") != 0);
    }
    private void Update()
    {
        JoystickControls.SetActive(PlayerPrefs.GetInt("JoystickControls") == 0);
        TouchControls.SetActive(PlayerPrefs.GetInt("JoystickControls") != 0);
        if (UICanvas.activeSelf)
        {
            ShipIcon.sprite = WorldManager.Instance.Head.GetComponentInChildren<SpriteRenderer>().sprite;

            controlToggle.isOn = (PlayerPrefs.GetInt("JoystickControls") == 0);
            thrusterToggle.isOn = WorldManager.Instance.Ship.thrustersOn;
            HUD_Money_Text.text = WorldManager.Instance.PlayerController.ReturnMoney();
            HUD_Fuel_Text.text = "Fuel: " + WorldManager.Instance.Ship.boostFuel + "/" + WorldManager.Instance.Ship.boostFuelMAX;
            HUD_Distance_Text.text = Mathf.CeilToInt(TrackingManager.Instance.ReturnDistanceToTracker()).ToString() + "au";
            HUD_ETA_Text.text = "Uncalculated";

            switch (WorldManager.Instance.Ship.motherShip)
            {
                case Ship.eMotherShip.BASIC:
                    HUD_Mothership_Text.text = "Race: Animal, Abilities: Basic, Description: Made of Orange Ore";
                    break;
                case Ship.eMotherShip.FLAMETHROWER:
                    HUD_Mothership_Text.text = "Race: Orc, Abilities: FlameThrower, Description: Made of neither Blood or Oranges";
                    break;
                case Ship.eMotherShip.LIGHTNING:
                    HUD_Mothership_Text.text = "Race: Human, Abilities: Lighting, Description: Made of Blue Ore";
                    break;
                case Ship.eMotherShip.HEALING:
                    HUD_Mothership_Text.text = "Race: Elf, Abilities: Healing, Description: Made of Green Ore";
                    break;
                case Ship.eMotherShip.GUARD_DRONE:
                    HUD_Mothership_Text.text = "Race: Fairy, Abilities: Guard Drones, Description: Made of Purple Ore";
                    break;
            }
        }
    }


    public void ToggleJoystickControls(bool toggled)
    {
        PlayerPrefs.SetInt("JoystickControls", (toggled) ? 0 : 1);
        controlToggle.isOn = (toggled);
    }


}
