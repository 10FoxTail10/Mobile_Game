
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventorySlot : MonoBehaviour
{
    [SerializeField] private Image icon;
    [SerializeField] private TMP_Text itemNameText;

    public void SetItem(ItemData item)
    {
        if (item == null)
        {
            Clear();
            return;
        }

        if (icon != null)
        {
            icon.sprite = item.icon;
            icon.enabled = item.icon != null;
        }

        if (itemNameText != null)
        {
            itemNameText.text = item.itemName;
        }

        gameObject.SetActive(true);
    }

    public void Clear()
    {
        if (icon != null)
        {
            icon.sprite = null;
            icon.enabled = false;
        }

        if (itemNameText != null)
        {
            itemNameText.text = "";
        }

        gameObject.SetActive(false);
    }
}
