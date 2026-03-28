using UnityEngine;

public class WaterSpot : MonoBehaviour
{
    private bool playerInRange = false;
    private PlayerSponge playerSponge;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        playerSponge = other.GetComponentInParent<PlayerSponge>();

        if (playerSponge != null)
        {
            playerInRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        if (other.GetComponentInParent<PlayerSponge>() == playerSponge)
        {
            playerInRange = false;
            playerSponge = null;
        }
    }

    private void Update()
    {
        if (playerInRange && playerSponge != null && Input.GetKeyDown(KeyCode.E))
        {
            if (playerSponge.CanAbsorbWater())
            {
                playerSponge.AbsorbWater();
                Destroy(gameObject);
            }
            else
            {
                Debug.Log("Sponge is full!");
            }
        }
    }
}