
using UnityEngine;

public class Inventory : MonoBehaviour
{
    [SerializeField] private int slotCount = 5;
    [SerializeField] private InventorySlot[] slots;

    private ItemData[] items;

    private void Awake()
    {
        items = new ItemData[slotCount];

        if (slots.Length != slotCount)
        {
            Debug.LogWarning(
                "Количество UI-слотов не совпадает с размером инвентаря!",
                this
            );
        }

        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i] != null)
            {
                slots[i].Clear();
            }
        }
    }

    public bool AddItem(ItemData item)
    {
        if (item == null)
            return false;

        for (int i = 0; i < items.Length; i++)
        {
            if (items[i] == null)
            {
                items[i] = item;

                if (i < slots.Length && slots[i] != null)
                {
                    slots[i].SetItem(item);
                }

                return true;
            }
        }

        return false;
    }

    public bool HasItem(ItemData item)
    {
        if (item == null)
            return false;

        for (int i = 0; i < items.Length; i++)
        {
            if (items[i] == item)
                return true;
        }

        return false;
    }

    public bool RemoveItem(ItemData item)
    {
        if (item == null)
            return false;

        for (int i = 0; i < items.Length; i++)
        {
            if (items[i] != item)
                continue;

            items[i] = null;

            if (i < slots.Length && slots[i] != null)
                slots[i].Clear();

            return true;
        }

        return false;
    }

    public bool HasFuseColor(FuseColor color)
    {
        for (int i = 0; i < items.Length; i++)
        {
            if (items[i] != null &&
                items[i].isFuse &&
                items[i].fuseColor == color)
            {
                return true;
            }
        }

        return false;
    }

    public bool HasAnyFuse()
    {
        for (int i = 0; i < items.Length; i++)
        {
            if (items[i] != null && items[i].isFuse)
            {
                return true;
            }
        }

        return false;
    }

    public bool RemoveFuseColor(FuseColor color)
    {
        for (int i = 0; i < items.Length; i++)
        {
            if (items[i] != null &&
                items[i].isFuse &&
                items[i].fuseColor == color)
            {
                items[i] = null;

                if (i < slots.Length && slots[i] != null)
                {
                    slots[i].Clear();
                }

                return true;
            }
        }

        return false;
    }

    public bool RemoveFuseColor(FuseColor color, out ItemData removedItem)
    {
        removedItem = null;

        for (int i = 0; i < items.Length; i++)
        {
            if (items[i] == null ||
                !items[i].isFuse ||
                items[i].fuseColor != color)
            {
                continue;
            }

            removedItem = items[i];
            items[i] = null;

            if (i < slots.Length && slots[i] != null)
            {
                slots[i].Clear();
            }

            return true;
        }

        return false;
    }

}
