using UnityEngine;
using UnityEngine.Events;

public class PlayerSponge : MonoBehaviour
{
    public UnityEvent<int> OnSpongeUpdate; //event to notify UI of sponge update and sponge sound

    public int soakedWaterCount = 0;
    public int maxWaterCapacity = 3;

    public GameObject spongeVisual;
    public GameObject spongePrefab;

    public GameObject waterPrefab;
    public Grid grid;
    public LayerMask blockingLayers;

    private Vector2 facingDirection = Vector2.down;
    private PlayerHeldItem playerHeldItem;

    private void Start()
    {
        playerHeldItem = GetComponent<PlayerHeldItem>();

        if (spongeVisual != null)
        {
            spongeVisual.SetActive(false);
        }
    }

    public bool CanPickUpSponge()
    {
        return playerHeldItem != null && playerHeldItem.IsHoldingNothing();
    }

    public bool PickUpSponge()
    {
        if (!CanPickUpSponge())
            return false;

        if (!playerHeldItem.TrySetHeldItem(PlayerHeldItem.HeldItemType.Sponge))
            return false;

        if (spongeVisual != null)
        {
            spongeVisual.SetActive(true);
        }

        Debug.Log("Sponge picked up!");
        return true;
    }

    public bool IsHoldingSponge()
    {
        return playerHeldItem != null && playerHeldItem.IsHoldingSponge();
    }

    public bool CanAbsorbWater()
    {
        return IsHoldingSponge() && soakedWaterCount < maxWaterCapacity;
    }

    public void AbsorbWater()
    {
        if (!IsHoldingSponge())
            return;

        if (soakedWaterCount < maxWaterCapacity)
        {
            soakedWaterCount++;
            OnSpongeUpdate.Invoke(soakedWaterCount);
            Debug.Log("Water absorbed. Current amount: " + soakedWaterCount);
        }
        else
        {
            Debug.Log("Sponge is full!");
        }
    }

    public bool CanUnloadWater()
    {
        return IsHoldingSponge() && soakedWaterCount > 0;
    }

    public void UnloadWater()
    {
        if (soakedWaterCount <= 0)
            return;

        soakedWaterCount--;
        OnSpongeUpdate.Invoke(soakedWaterCount);
    }

    public void SetFacingDirection(Vector2 newDirection)
    {
        if (newDirection != Vector2.zero)
        {
            facingDirection = newDirection.normalized;
        }
    }

    public void DropSponge(Vector2 dropPosition)
    {
        if (!IsHoldingSponge())
            return;

        GameObject droppedSponge = Instantiate(spongePrefab, dropPosition, Quaternion.identity);

        SpongePickup pickup = droppedSponge.GetComponent<SpongePickup>();
        if (pickup != null)
        {
            pickup.SetStoredWater(soakedWaterCount);
        }

        soakedWaterCount = 0;
        playerHeldItem?.ClearHeldItem(PlayerHeldItem.HeldItemType.Sponge);

        if (spongeVisual != null)
        {
            spongeVisual.SetActive(false);
        }

        Debug.Log("Sponge dropped.");
    }

    private void Update()
    {
        if (!IsHoldingSponge())
            return;

        if (Input.GetKeyDown(KeyCode.Q))
        {
            Vector3Int playerCell = grid.WorldToCell(transform.position);
            Vector3Int targetCell = playerCell + new Vector3Int((int)facingDirection.x, (int)facingDirection.y, 0);
            Vector3 dropPosition = grid.GetCellCenterWorld(targetCell);

            DropSponge(dropPosition);
            return;
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            TryPlaceWater();
        }
    }

    private void TryPlaceWater()
    {
        if (!CanUnloadWater())
        {
            Debug.Log("No water to unload.");
            return;
        }

        Vector3Int playerCell = grid.WorldToCell(transform.position);
        Vector3Int targetCell = playerCell + new Vector3Int((int)facingDirection.x, (int)facingDirection.y, 0);
        Vector3 cellCenter = grid.GetCellCenterWorld(targetCell);

        Collider2D[] hits = Physics2D.OverlapBoxAll(cellCenter, new Vector2(0.8f, 0.8f), 0f, blockingLayers);

        foreach (Collider2D hit in hits)
        {
            if (!hit.transform.IsChildOf(transform))
            {
                Debug.Log("Can't place water there. Blocked by: " + hit.name);
                return;
            }
        }

        Instantiate(waterPrefab, cellCenter, Quaternion.identity);
        UnloadWater();
    }
}