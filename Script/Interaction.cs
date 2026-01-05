using UnityEngine;

public class Interaction : MonoBehaviour
{
    private IInteractable interactableInRange;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && interactableInRange != null && interactableInRange.CanInteract())
        {
            interactableInRange.Interact();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out IInteractable interactable) && interactable.CanInteract())
        {
            interactableInRange = interactable;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out IInteractable interactable) && interactable == interactableInRange)
        {
            interactableInRange = null;
        }
    }
}
