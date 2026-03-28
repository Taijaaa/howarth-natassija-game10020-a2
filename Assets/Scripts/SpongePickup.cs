using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class SpongePickup : MonoBehaviour, IPickupable
{
    [SerializeField] private int storedWater = 0;

    public void SetStoredWater(int amount)
    {
        storedWater = Mathf.Max(0, amount);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player"))
            return;

        Pickup(other.gameObject);
    }

    public void Pickup(GameObject player)
    {
        PlayerSponge playerSponge = player.GetComponentInParent<PlayerSponge>();
        if (playerSponge == null)
            return;

        if (playerSponge.PickUpSponge())
        {
            playerSponge.soakedWaterCount = storedWater;
            Destroy(gameObject);
        }
    }
}