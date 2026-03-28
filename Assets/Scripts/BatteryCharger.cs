using UnityEngine;
using UnityEngine.Events;

public class BatteryCharger : MonoBehaviour
{
    public UnityEvent OnBatteryCharge; //event to notify UI of battery charge and charge sound

    private bool playerInRange = false;
    private PlayerBattery playerBattery;

    private bool isCharging = false;
    private float chargeTimer = 0f;
    public float chargeDuration = 5f;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        playerBattery = other.GetComponentInParent<PlayerBattery>();

        if (playerBattery != null)
        {
            playerInRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        if (playerBattery != null)
        {
            playerInRange = false;
            isCharging = false;
            chargeTimer = 0f;
        }
    }

    private void Update()
    {
        if (playerInRange && playerBattery != null)
        {
            // Start charging if player is holding an uncharged battery
            if (Input.GetKeyDown(KeyCode.Z) && playerBattery.IsHoldingBattery() && !playerBattery.isBatteryCharged)
            {
                isCharging = true;
                chargeTimer = 0f;
                OnBatteryCharge.Invoke();
                Debug.Log("Charging started...");
            }

            if (isCharging)
            {
                // Stop charging if the player drops the battery while charging
                if (!playerBattery.IsHoldingBattery())
                {
                    isCharging = false;
                    chargeTimer = 0f;
                    Debug.Log("Charging interrupted: battery dropped.");
                    return;
                }

                chargeTimer += Time.deltaTime;

                if (chargeTimer >= chargeDuration)
                {
                    playerBattery.ChargeBattery();
                    isCharging = false;
                    Debug.Log("Charging complete!");
                }
            }
        }
    }
}