using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHUD : UIBaseClass
{
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

    private Ship ship;

    private void Start()
    {
        controlToggle.isOn = (PlayerPrefs.GetInt("JoystickControls") == 0);
        JoystickControls.SetActive(PlayerPrefs.GetInt("JoystickControls") == 0);
        TouchControls.SetActive(PlayerPrefs.GetInt("JoystickControls") != 0);

        ship = WorldManager.Instance.Ship;
    }
    private void FixedUpdate()
    {
        JoystickControls.SetActive(PlayerPrefs.GetInt("JoystickControls") == 0);
        TouchControls.SetActive(PlayerPrefs.GetInt("JoystickControls") != 0);
        if (UICanvas.activeSelf)
        {
            SetDetails();
        }
    }

    public void SetDetails()
    {
        ShipIcon.sprite = WorldManager.Instance.Head.GetComponentInChildren<SpriteRenderer>().sprite;

        controlToggle.isOn = (PlayerPrefs.GetInt("JoystickControls") == 0);
        thrusterToggle.isOn = ship.thrustersOn;
        HUD_Money_Text.text = WorldManager.Instance.PlayerController.ReturnMoney();
        HUD_Fuel_Text.text = "Fuel: " + ship.boostFuel + "/" + ship.boostFuelMAX;
        HUD_Distance_Text.text = Mathf.CeilToInt(TrackingManager.Instance.ReturnDistanceToTracker()).ToString() + "au";
        if (TrackingManager.Instance.ReturnETA() > 1000000 || TrackingManager.Instance.ReturnETA() < 0)
        {
            HUD_ETA_Text.text = "[REDACTED]";
        }
        else
        {
            HUD_ETA_Text.text = TrackingManager.Instance.ReturnETA() + "s";
        }

        switch (ship.motherShip)
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

    public void ToggleJoystickControls(bool toggled)
    {
        PlayerPrefs.SetInt("JoystickControls", (toggled) ? 0 : 1);
        controlToggle.isOn = (toggled);
    }
}
