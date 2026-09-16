using NUnit.Framework.Interfaces;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    [SerializeField] private int slotCount = 5;
    [SerializeField] private InventorySlot[] slots;

    private ItemData[] items;

    private void Awake()
    {
        items = new ItemData[slotCount];
    }

    public bool AddItem(ItemData item)
    {
        for (int i = 0; i < items.Length; i++)
        {
            if (items[i] == null)
            {
                items[i] = item;

                slots[i].SetItem(item.icon);

                return true;
            }
        }

        return false;
    }

    public bool HasItem(ItemData item)
    {
        for (int i = 0; i < items.Length; i++)
        {
            if (items[i] == item)
            {
                return true;
            }
        }

        return false;
    }
}