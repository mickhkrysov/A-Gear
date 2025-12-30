using UnityEngine;

public class OnInteracted : MonoBehaviour, IInteractable
{
    public void Interact()
    {
        Destroy(gameObject);
        Debug.Log("Object interacted with!");
    }
}
