using UnityEngine;
using UnityEngine.EventSystems;


//this class allows dregging items in inventory
public class ItemDragHandler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    Transform originalParent; //to store the original parent of the item being dragged
    CanvasGroup canvasGroup; //to manage the item's interactivity during drag

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        originalParent = transform.parent; //store original parent
        transform.SetParent(transform.root); //move item to top level in hierarchy to ensure visibility
        canvasGroup.blocksRaycasts = false; //disable raycast blocking to allow drop detection
        canvasGroup.alpha = 0.6f; //make item semi-transparent during drag
    }

    public void OnDrag(PointerEventData eventData)  //A DRAG QUEEN????!!!! THE QUEEN OF DRAG!!!
    {
        transform.position = eventData.position; //follow the pointer position
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = true; 
        canvasGroup.alpha = 1.0f; //restore full opacity

        Slot dropSlot = eventData.pointerEnter?.GetComponent<Slot>(); //slot item dropped

        if (dropSlot == null)
        {
            GameObject item = eventData.pointerEnter; //get the item being pointed at
            if(item != null)
            {
                dropSlot = item.GetComponentInParent<Slot>();
            }
        }

        Slot originalSlot = originalParent.GetComponent<Slot>(); //original slot of the item

        //repositioning items if in there's any item in the next slot
        if(dropSlot != null)   
        {
            if(dropSlot.currentItem != null) //swaping items
            {
                dropSlot.currentItem.transform.SetParent(dropSlot.transform);
                originalSlot.currentItem = dropSlot.currentItem;
            }

            else
            {
                originalSlot.currentItem = null; //original slot is now empty
            }

            //move item
            transform.SetParent(dropSlot.transform);
            dropSlot.currentItem = gameObject; //update drop slot's current item
        }
        else
        {
            transform.SetParent(originalParent);
        }

        GetComponent<RectTransform>().anchoredPosition = Vector2.zero; //center item in slot again

    }
   
}