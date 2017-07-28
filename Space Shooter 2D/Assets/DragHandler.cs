using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class DragHandler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public static GameObject itemBeingDragged;
    public static bool lastDropped = false;
    public GameObject weapon;
    bool deleteOnDrop;
    Vector3 startPosition;
    Transform startParent;

    public void OnBeginDrag(PointerEventData eventData)
    {
        deleteOnDrop = this.transform.parent.GetComponent<SlotScript>().GetDeleteOnDrop();
        lastDropped = false;
        if (!deleteOnDrop)
        {
            itemBeingDragged = Instantiate(this.gameObject);
            itemBeingDragged.transform.SetParent(this.transform.parent);
        }
        else itemBeingDragged = this.gameObject;
        startPosition = transform.position;
        startParent = transform.parent;
        GetComponent<CanvasGroup>().blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        itemBeingDragged.transform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        GetComponent<CanvasGroup>().blocksRaycasts = true;
        if (itemBeingDragged.transform.parent != startParent || lastDropped)
            itemBeingDragged.transform.position = startPosition;
        else
        {
            Destroy(itemBeingDragged);
        }
    }
}
