
using UnityEngine;

public class FuseSocket : MonoBehaviour
{
    [Header("Настройки")]
    [SerializeField] private FuseColor requiredColor;
    [SerializeField] private ItemData requiredGloves;
    [SerializeField] private Inventory inventory;
    [SerializeField] private PlayerCatch playerCatch;

    [Header("Визуал")]
    [SerializeField] private GameObject installedFuseVisual;


    [Header("Эффекты ошибки")]
    [SerializeField] private GameObject wrongFuseEffect;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip wrongFuseSound;


    private ItemData _installedItem;

    public bool IsOccupied => _installedItem != null;
    public FuseColor RequiredColor => requiredColor;

    public string ActionName => IsOccupied ? "Забрать" : "Установить";

    private void Awake()
    {
        UpdateFuseVisual();
    }


    public void Interact()
    {
        if (inventory == null)
        {
            Debug.LogError("Не назначен Inventory!", this);
            return;
        }

        // Если разъём занят, возвращаем предохранитель.
        if (IsOccupied)
        {
            if (!inventory.AddItem(_installedItem))
            {
                Debug.LogWarning("Инвентарь заполнен!");
                return;
            }

            _installedItem = null;
            UpdateFuseVisual();

            Debug.Log("Предохранитель возвращён в инвентарь.");
            return;
        }

        if (requiredGloves == null || playerCatch == null)
        {
            Debug.LogError("Не назначены перчатки или PlayerCatch!", this);
            return;
        }

        // Нет предохранителей: нет взаимодействия и удара.
        if (!inventory.HasAnyFuse())
        {
            Debug.Log("В инвентаре нет предохранителей.");
            return;
        }

        bool hasGloves = inventory.HasItem(requiredGloves);
        bool hasCorrectFuse = inventory.HasFuseColor(requiredColor);

        // Неправильный предохранитель.

        if (!hasCorrectFuse)
        {
            PlayWrongFuseEffects();

            if (hasGloves)
            {
                ConsumeGloves();

                Debug.LogWarning(
                    "Перчатки защитили от ошибки, но пришли в негодность."
                );
            }
            else
            {
                ElectricShock();
            }

            return;
        }


        // Предохранитель правильный: устанавливаем его.
        if (!inventory.RemoveFuseColor(
            requiredColor, out ItemData removedItem))
        {
            Debug.LogWarning("Не удалось извлечь предохранитель.");
            return;
        }

        _installedItem = removedItem;
        UpdateFuseVisual();

        if (hasGloves)
        {
            ConsumeGloves();
            Debug.Log("Предохранитель установлен. Перчатки использованы.");
        }
        else
        {
            PlayWrongFuseEffects();

            Debug.LogWarning(
                "Предохранитель установлен, но игрок получил удар током!"
            );

            ElectricShock();
        }
    }


    private void ElectricShock()
    {
        playerCatch.ConsumeAttempt();
        Debug.LogWarning("Удар током! Потеряна попытка.");
    }

    private bool ConsumeGloves()
    {
        if (inventory == null || requiredGloves == null)
            return false;

        return inventory.RemoveItem(requiredGloves);
    }

    private void UpdateFuseVisual()
    {
        if (installedFuseVisual != null)
        {
            installedFuseVisual.SetActive(IsOccupied);
        }
    }

    private void PlayWrongFuseEffects()
    {
        // Создаём эффект в месте разъёма.
        if (wrongFuseEffect != null)
        {
            Instantiate(
                wrongFuseEffect,
                transform.position,
                transform.rotation
            );
        }

        // Проигрываем звук ошибки.
        if (audioSource != null && wrongFuseSound != null)
        {
            audioSource.PlayOneShot(wrongFuseSound);
        }
    }

}
