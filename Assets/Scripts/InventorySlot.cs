using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{
    [SerializeField] private Image icon;

    public void SetItem(Sprite itemIcon)
    {
        icon.sprite = itemIcon;
        icon.enabled = true;
    }

    public void Clear()
    {
        icon.sprite = null;
        icon.enabled = false;
    }
}