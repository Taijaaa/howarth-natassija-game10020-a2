using UnityEngine;

public class PlayerHeldItem : MonoBehaviour
{
    public enum HeldItemType
    {
        None,
        Sponge,
        Battery
    }

    private HeldItemType currentHeldItem = HeldItemType.None;

    public HeldItemType CurrentHeldItem => currentHeldItem;

    public bool IsHoldingNothing()
    {
        return currentHeldItem == HeldItemType.None;
    }

    public bool IsHoldingSponge()
    {
        return currentHeldItem == HeldItemType.Sponge;
    }

    public bool IsHoldingBattery()
    {
        return currentHeldItem == HeldItemType.Battery;
    }

    public bool TrySetHeldItem(HeldItemType itemType)
    {
        if (currentHeldItem != HeldItemType.None)
            return false;

        currentHeldItem = itemType;
        return true;
    }

    public void ClearHeldItem(HeldItemType itemType)
    {
        if (currentHeldItem == itemType)
        {
            currentHeldItem = HeldItemType.None;
        }
    }

    public void ForceClear()
    {
        currentHeldItem = HeldItemType.None;
    }
}