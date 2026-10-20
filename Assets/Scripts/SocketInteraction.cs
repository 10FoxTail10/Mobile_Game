
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class SocketInteraction : MonoBehaviour
{
    [Header("Камера")]
    [SerializeField] private Camera playerCamera;
    [SerializeField] private float interactionDistance = 2f;

    [Header("Мобильная кнопка")]
    [SerializeField] private Button useButton;
    [SerializeField] private TMP_Text buttonText;

    private FuseSocket _currentSocket;

    private void Awake()
    {
        if (useButton != null)
        {
            useButton.gameObject.SetActive(false);
            useButton.onClick.AddListener(InteractMobile);
        }
    }

    private void OnDestroy()
    {
        if (useButton != null)
        {
            useButton.onClick.RemoveListener(InteractMobile);
        }
    }

    private void Update()
    {
        FindSocket();

        // Клавиатура остаётся для тестирования на ПК.
        if (_currentSocket != null &&
            Input.GetKeyDown(KeyCode.E))
        {
            UseCurrentSocket();
        }
    }

    private void FindSocket()
    {
        _currentSocket = null;

        Ray ray = playerCamera.ViewportPointToRay(
            new Vector3(0.5f, 0.5f, 0f)
        );

        if (Physics.Raycast(
            ray,
            out RaycastHit hit,
            interactionDistance))
        {
            FuseSocket socket =
                hit.collider.GetComponentInParent<FuseSocket>();

            if (socket != null && !socket.IsOccupied)
            {
                _currentSocket = socket;
            }
        }

        bool canUse = _currentSocket != null;

        if (useButton != null &&
            useButton.gameObject.activeSelf != canUse)
        {
            useButton.gameObject.SetActive(canUse);
        }

        if (canUse && buttonText != null)
        {
            buttonText.text = "Использовать";
        }
    }

    public void InteractMobile()
    {
        UseCurrentSocket();
    }

    private void UseCurrentSocket()
    {
        if (_currentSocket == null)
        {
            return;
        }


        // Обновляем кнопку после установки.
        FindSocket();
    }
}
