using UnityEngine;
using UnityEngine.Events;

public class PlayerBattery : MonoBehaviour
{
    public UnityEvent<bool> OnBatteryUpdate; //event to notify UI of battery update and door sound

    public bool isBatteryCharged = false;

    public GameObject batteryVisual;
    public GameObject batteryPrefab;

    public Grid grid;
    private Vector2 facingDirection = Vector2.down;

    private PlayerHeldItem playerHeldItem;

    private void Start()
    {
        playerHeldItem = GetComponent<PlayerHeldItem>();

        if (batteryVisual != null)
        {
            batteryVisual.SetActive(false);
        }
    }

    public void SetFacingDirection(Vector2 newDirection)
    {
        if (newDirection != Vector2.zero)
        {
            facingDirection = newDirection.normalized;
        }
    }

    public bool CanPickUpBattery()
    {
        return playerHeldItem != null && playerHeldItem.IsHoldingNothing();
    }

    public bool PickUpBattery(bool chargedState = false)
    {
        if (!CanPickUpBattery())
            return false;

        if (!playerHeldItem.TrySetHeldItem(PlayerHeldItem.HeldItemType.Battery))
            return false;

        isBatteryCharged = chargedState;

        if (batteryVisual != null)
        {
            batteryVisual.SetActive(true);
        }

        return true;
    }

    public bool IsHoldingBattery()
    {
        return playerHeldItem != null && playerHeldItem.IsHoldingBattery();
    }

    public void ChargeBattery()
    {
        if (!IsHoldingBattery())
            return;

        isBatteryCharged = true;
        OnBatteryUpdate.Invoke(isBatteryCharged);
        Debug.Log("Battery charged!");
    }

    public bool HasChargedBattery()
    {
        return IsHoldingBattery() && isBatteryCharged;
    }

    public void UseBattery()
    {
        if (!IsHoldingBattery())
            return;

        isBatteryCharged = false;

        OnBatteryUpdate.Invoke(isBatteryCharged);
        Debug.Log("Battery used.");
    }

    public void DropBattery(Vector2 dropPosition)
    {
        if (!IsHoldingBattery())
            return;

        if (batteryPrefab == null)
        {
            Debug.LogError("Battery prefab is not assigned on PlayerBattery.");
            return;
        }

        GameObject droppedBattery = Instantiate(batteryPrefab, dropPosition, Quaternion.identity);

        BatteryPickup pickup = droppedBattery.GetComponent<BatteryPickup>();
        if (pickup != null)
        {
            pickup.SetChargedState(isBatteryCharged);
        }

        isBatteryCharged = false;
        playerHeldItem?.ClearHeldItem(PlayerHeldItem.HeldItemType.Battery);

        if (batteryVisual != null)
        {
            batteryVisual.SetActive(false);
        }

        Debug.Log("Battery dropped.");
    }

    private void Update()
    {
        if (IsHoldingBattery() && Input.GetKeyDown(KeyCode.Q))
        {
            if (grid == null)
            {
                Debug.LogError("Grid is not assigned on PlayerBattery.");
                return;
            }

            Vector3Int playerCell = grid.WorldToCell(transform.position);
            Vector3Int targetCell = playerCell + new Vector3Int((int)facingDirection.x, (int)facingDirection.y, 0);
            Vector3 dropPosition = grid.GetCellCenterWorld(targetCell);

            DropBattery(dropPosition);
        }
    }
}