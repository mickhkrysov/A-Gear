using UnityEngine;

// Interface
public interface IInteractable
{
    void Interact();
}

public class Interactor : MonoBehaviour
{
    [Header("Interact Settings")]
    [SerializeField] private Transform interactSource;
    [SerializeField] private float interactRange = 1.0f;
    [SerializeField] private LayerMask interactLayerMask;

    private void Update()
    {
        if (!Input.GetKeyDown(KeyCode.E)) return;
        if (interactSource == null) return;

        Vector2 origin = interactSource.position;
        Vector2 direction = interactSource.right; // player facing direction

        RaycastHit2D hit = Physics2D.Raycast(
            origin,
            direction,
            interactRange,
            interactLayerMask
        );

        Debug.DrawRay(origin, direction * interactRange, Color.yellow, 1.5f);

        if (!hit) return;

        // Find a component that implements IInteractable
        MonoBehaviour[] components = hit.collider.GetComponents<MonoBehaviour>();
        foreach (MonoBehaviour component in components)
        {
            if (component is IInteractable interactable)
            {
                interactable.Interact();
                break;
            }
        }
    }
}
