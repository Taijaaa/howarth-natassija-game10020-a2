using UnityEngine;

public class BatteryDoor : MonoBehaviour
{
    private bool playerInRange = false;
    private PlayerBattery playerBattery;

    public GameObject doorVisual;
    public Collider2D doorCollider;

    private bool isUnlocked = false;

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

        if (other.GetComponentInParent<PlayerBattery>() == playerBattery)
        {
            playerInRange = false;
        }
    }

    private void Update()
    {
        if (isUnlocked) return;

        if (playerInRange && playerBattery != null && Input.GetKeyDown(KeyCode.Z))
        {
            if (playerBattery.HasChargedBattery())
            {
                UnlockDoor();
                playerBattery.UseBattery();
            }
            else
            {
                Debug.Log("You need a charged battery to unlock this door.");
            }
        }
    }

    private void UnlockDoor()
    {
        isUnlocked = true;

        if (doorVisual != null)
            doorVisual.SetActive(false);

        if (doorCollider != null)
            doorCollider.enabled = false;

        Debug.Log("Door unlocked!");
    }
}