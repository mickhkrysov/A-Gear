using UnityEngine;

public class Chest : MonoBehaviour, IInteractable
{
    public bool isOpen {get; private set;}
    public string ChestID {get; private set;}
    public GameObject lootPrefab; // item that chest drops
    public Sprite openedSprite; // sprite to show when chest is opened
    void Start()
    {
        ChestID ??= GlobalHelper.GenerateUniqueID(gameObject);
    }

    public bool CanInteract()
    {
        return !isOpen;
    }

    public void Interact()
    {
        if (!CanInteract()) return;
        OpenChest();
    }

    public void OpenChest()
    {
        if (isOpen) return;
        SetOpened(true);

        //Drop Item
        if (lootPrefab != null)
        {
            GameObject loot = Instantiate(lootPrefab, transform.position + Vector3.left, Quaternion.identity);
        }
    }

    public void SetOpened(bool opened)
    {
        isOpen = opened;

        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        if (sr != null && openedSprite != null)
        {
            sr.sprite = openedSprite;
        }
    }


}
