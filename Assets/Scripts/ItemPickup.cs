using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    [SerializeField] private Camera playerCamera;
    [SerializeField] private GameObject pickupButton;
    [SerializeField] private Inventory inventory;
    [SerializeField] private GameObject pickupEffect;
    [SerializeField] private float pickupDistance = 2f;

    private void Awake()
    {
        pickupButton.SetActive(false);
    }

    private void Update()
    {
        PickupPC();

        Ray ray = playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));

        if (Physics.Raycast(ray, out RaycastHit hit, pickupDistance))
        {
            if (hit.transform.CompareTag("Item"))
            {
                pickupButton.SetActive(true);
                return;
            }
            if (hit.transform.CompareTag("Door"))
            {
                pickupButton.SetActive(true);
                return;
            }
        }

        pickupButton.SetActive(false);
    }

    private void PickupPC()
    {
        Ray ray = playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));

        if (Physics.Raycast(ray, out RaycastHit hit, pickupDistance))
        {
            if (hit.transform.CompareTag("Item"))
            {
                if (Input.GetKeyDown(KeyCode.E))
                {
                    Item item = hit.transform.GetComponent<Item>();

                    if (item != null)
                    {
                        if (inventory.AddItem(item.Data))
                        {
                            Debug.Log("Подобран: " + hit.transform.name);
                            Instantiate(pickupEffect, hit.transform.position, Quaternion.identity);
                            Destroy(hit.transform.gameObject);
                        }
                    }
                }
            }
        }
    }

    public void PickupMobile()
    {
        Ray ray = playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));

        if (Physics.Raycast(ray, out RaycastHit hit, pickupDistance))
        {
            if (hit.transform.CompareTag("Item"))
            {
                Item item = hit.transform.GetComponent<Item>();

                if (item != null)
                {
                    if (inventory.AddItem(item.Data))
                    {
                        Debug.Log("Подобран: " + hit.transform.name);
                        Instantiate(pickupEffect, hit.transform.position, Quaternion.identity);
                        Destroy(hit.transform.gameObject);
                    }
                }

            }
        }
    }
}
