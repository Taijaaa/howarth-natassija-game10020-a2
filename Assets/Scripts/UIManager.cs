using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    public PlayerSponge playerSponge;
    public PlayerBattery playerBattery;
    public BatteryCharger batteryCharger;

    public TextMeshProUGUI spongeText;
    public TextMeshProUGUI batteryText;

    private void Start()
    {
        playerSponge.OnSpongeUpdate.AddListener(UpdateSpongeText); //listen for sponge update event
        playerBattery.OnBatteryUpdate.AddListener(UpdateBatteryText); //listen for battery update event
        batteryCharger.OnBatteryCharge.AddListener(UpdateBatteryTextCharging); //listen for battery charge event
    }

    public void UpdateSpongeText(int amount)
    {
        spongeText.text = "Sponge Fill \n Amount:  " + amount;
    }

    public void UpdateBatteryText(bool isCharged)
    {
        if (isCharged)
        {
            batteryText.text = "Battery: \n Charged";
        }
        else
        {
            batteryText.text = "Battery: \n Empty";
        }
    }

    public void UpdateBatteryTextCharging()
    {
        batteryText.text = "Battery: \n Charging";
    }
}
