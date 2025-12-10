using TMPro;
using UnityEngine;
using UnityEngine.UI;

interface IInteractable
{
    public void Interact();
    public string GetDescription();
}

public class Interactor : MonoBehaviour
{
    public Transform InteractorSource;
    public float InteractRange = 4f;

    [Header("UI")]
    public Sprite defaultIcon;
    public Sprite interactIcon;

    private Image crosshairImage;
    private GameObject interactionPanel;
    private bool isInteractable = false;

    void Start()
    {
        GameObject crosshairObj = GameObject.FindGameObjectWithTag("Crosshair");

        if (crosshairObj != null)
        {
            crosshairImage = crosshairObj.GetComponent<Image>();
            crosshairImage.sprite = defaultIcon;
        }

        interactionPanel = GameObject.FindGameObjectWithTag("InteractionText");

        if (interactionPanel != null)
        {
            interactionPanel.SetActive(false);
        }
    }

    void Update()
    {
        Ray r = new Ray(InteractorSource.position, InteractorSource.forward);
        bool hitSomething = Physics.Raycast(r, out RaycastHit hitInfo, InteractRange);
        bool foundInteractable = hitSomething && hitInfo.collider.gameObject.TryGetComponent(out IInteractable interactObj);

        if (foundInteractable != isInteractable)
        {
            isInteractable = foundInteractable;

            if (isInteractable)
            {
                if (interactionPanel != null) interactionPanel.SetActive(true);
                if (crosshairImage != null) crosshairImage.sprite = interactIcon;
            }
            else
            {
                if (interactionPanel != null) interactionPanel.SetActive(false);
                if (crosshairImage != null) crosshairImage.sprite = defaultIcon;
            }
        }

        if (isInteractable && Input.GetKeyDown(KeyCode.E))
        {
            hitInfo.collider.GetComponent<IInteractable>().Interact();
        }
    }
}