using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class BatteryPickup : MonoBehaviour, IPickupable
{
    [SerializeField] private bool isCharged = false;

    public void SetChargedState(bool charged)
    {
        isCharged = charged;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player"))
            return;

        Pickup(other.gameObject);
    }

    public void Pickup(GameObject player)
    {
        PlayerBattery playerBattery = player.GetComponentInParent<PlayerBattery>();
        if (playerBattery == null)
            return;

        if (playerBattery.PickUpBattery(isCharged))
        {
            Destroy(gameObject);
        }
    }
}