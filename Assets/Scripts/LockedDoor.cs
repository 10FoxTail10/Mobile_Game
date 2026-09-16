using UnityEngine;

public class LockedDoor : MonoBehaviour
{
    [SerializeField] private ItemData requiredItem;
    [SerializeField] private Inventory inventory;

    private bool _isOpen = false;

    public void Open()
    {
        if (inventory.HasItem(requiredItem))
        {
            if (_isOpen)
            {
                transform.Rotate(0f, -90f, 0f);
                _isOpen = false;
            }
            else
            {
                transform.Rotate(0f, 90f, 0f);
                _isOpen = true;
            }
        }
    }
}