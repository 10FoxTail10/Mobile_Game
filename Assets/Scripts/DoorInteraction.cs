using UnityEngine;

public class DoorInteraction : MonoBehaviour
{
    [SerializeField] private Camera playerCamera;
    [SerializeField] private GameObject pickupButton;
    [SerializeField] private float interactionDistance = 2f;

    private void Awake()
    {
        pickupButton.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            Interact();
        }
    }

    public void Interact()
    {
        Ray ray = playerCamera.ViewportPointToRay(
            new Vector3(0.5f, 0.5f, 0f)
        );

        if (Physics.Raycast(ray, out RaycastHit hit, interactionDistance))
        {
            LockedDoor door = hit.transform.GetComponent<LockedDoor>();

            if (door != null)
            {
                door.Open();
            }
        }
    }
}