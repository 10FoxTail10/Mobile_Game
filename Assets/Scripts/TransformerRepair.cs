
using UnityEngine;

public class TransformerRepair : MonoBehaviour
{
    [Header("Предметы")]
    [SerializeField] private ItemData requiredGloves;
    [SerializeField] private FuseColor requiredColor;
    [SerializeField] private Inventory inventory;
    [SerializeField] private PlayerCatch playerCatch;

    [Header("Состояние")]
    [SerializeField] private bool isRepaired;

    public bool IsRepaired => isRepaired;

    public void TryRepair()
    {
        if (isRepaired)
        {
            Debug.Log("Трансформатор уже отремонтирован.");
            return;
        }

        if (requiredGloves == null ||
            inventory == null ||
            playerCatch == null)
        {
            Debug.LogError(
                "Не назначены необходимые ссылки!",
                this
            );
            return;
        }

        if (!inventory.HasItem(requiredGloves))
        {
            Debug.LogWarning("Удар током! Нет защитных перчаток.");
            ElectricShock();
            return;
        }

        if (inventory.HasFuseColor(requiredColor))
        {
            if (inventory.RemoveFuseColor(requiredColor))
            {
                isRepaired = true;
                Debug.Log("Трансформатор успешно отремонтирован!");
            }

            return;
        }

        if (inventory.HasAnyFuse())
        {
            Debug.LogWarning("Удар током! Неправильный цвет предохранителя.");
            ElectricShock();
        }
        else
        {
            Debug.Log("Нет подходящего предохранителя.");
        }
    }

    private void ElectricShock()
    {
        playerCatch.ConsumeAttempt();
    }
}
