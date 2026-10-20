
using UnityEngine;

public enum FuseColor
{
    None,
    Red,
    Yellow,
    Purple
}

[CreateAssetMenu(fileName = "NewItem", menuName = "Items/Item Data")]
public class ItemData : ScriptableObject
{
    public string itemName;
    public Sprite icon;

    [Header("Тип предмета")]
    public bool isFuse;

    [Header("Цвет предохранителя")]
    public FuseColor fuseColor = FuseColor.None;
}
