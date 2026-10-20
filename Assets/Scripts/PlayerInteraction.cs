
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerInteraction : MonoBehaviour
{
    [Header("Камера")]
    [SerializeField] private Camera playerCamera;
    [SerializeField] private float interactionDistance = 2f;

    [Header("Инвентарь")]
    [SerializeField] private Inventory inventory;

    [Header("Кнопка")]
    [SerializeField] private Button useButton;
    [SerializeField] private TMP_Text buttonText;

    [Header("Эффект подбора")]
    [SerializeField] private GameObject pickupEffect;

    private Item _currentItem;
    private FuseSocket _currentSocket;

    private void Awake()
    {
        if (useButton != null)
        {
            useButton.gameObject.SetActive(false);
            useButton.onClick.AddListener(Interact);
        }
    }

    private void OnDestroy()
    {
        if (useButton != null)
        {
            useButton.onClick.RemoveListener(Interact);
        }
    }

    private void Update()
    {
        FindTarget();

        if (Input.GetKeyDown(KeyCode.E))
        {
            Interact();
        }
    }

    private void FindTarget()
    {
        _currentItem = null;
        _currentSocket = null;

        Ray ray = playerCamera.ViewportPointToRay(
            new Vector3(0.5f, 0.5f, 0f)
        );

        if (Physics.Raycast(
            ray, out RaycastHit hit, interactionDistance))
        {
            _currentSocket =
                hit.collider.GetComponentInParent<FuseSocket>();

            // Сокет имеет приоритет перед обычным предметом.
            if (_currentSocket == null)
            {
                _currentItem =
                    hit.collider.GetComponentInParent<Item>();
            }

            if (_currentItem != null &&
                _currentItem.Data == null)
            {
                _currentItem = null;
            }
        }

        bool canInteract =
            _currentSocket != null || _currentItem != null;

        if (useButton != null &&
            useButton.gameObject.activeSelf != canInteract)
        {
            useButton.gameObject.SetActive(canInteract);
        }

        if (buttonText != null && canInteract)
        {
            buttonText.text = _currentSocket != null
                ? _currentSocket.ActionName
                : "Подобрать";
        }
    }

    public void Interact()
    {
        if (_currentSocket != null)
        {
            _currentSocket.Interact();
        }
        else if (_currentItem != null)
        {
            PickupItem(_currentItem);
        }

        FindTarget();
    }


    private void PickupItem(Item item)
    {
        if (item == null || item.Data == null || inventory == null)
            return;

        ItemData data = item.Data;

        // Не разрешаем повторно брать один и тот же предмет,
        // если он уже есть в инвентаре.
        if (inventory.HasItem(data))
        {
            Debug.Log($"Предмет «{data.itemName}» уже есть в инвентаре.");
            return;
        }

        if (!inventory.AddItem(data))
        {
            Debug.Log("Инвентарь заполнен!");
            return;
        }

        if (pickupEffect != null)
        {
            Instantiate(
                pickupEffect,
                item.transform.position,
                Quaternion.identity
            );
        }

        // Уничтожаем объект только при включённой настройке.
        if (item.DestroyOnPickup)
        {
            Destroy(item.gameObject);
        }
    }

}
