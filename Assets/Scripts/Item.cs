
using UnityEngine;

public class Item : MonoBehaviour
{
    [Header("Данные предмета")]
    [SerializeField] private ItemData itemData;

    [Header("Поведение при подборе")]
    [SerializeField] private bool destroyOnPickup = true;

    public ItemData Data => itemData;
    public bool DestroyOnPickup => destroyOnPickup;
}
