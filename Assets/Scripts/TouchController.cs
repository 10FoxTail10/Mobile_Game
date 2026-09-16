using UnityEngine;
using UnityEngine.EventSystems;

public class TouchController : MonoBehaviour, IDragHandler
{
    [SerializeField] private Transform playerCamera;
    [SerializeField] private Transform player;
    [SerializeField] private float sensitivity = 0.1f;

    private float xRotation = 0f;

    public void OnDrag(PointerEventData eventData)
    {
        float mouseX = eventData.delta.x;
        float mouseY = eventData.delta.y;

        player.Rotate(Vector3.up * mouseX * sensitivity);

        xRotation -= mouseY * sensitivity;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        playerCamera.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
    }
}
