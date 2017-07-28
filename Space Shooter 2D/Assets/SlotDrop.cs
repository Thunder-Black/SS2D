using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

public class SlotDrop : MonoBehaviour, IDropHandler {
    public GameObject item
    {
        get
        {
            if (transform.childCount > 0)
                return transform.GetChild(0).gameObject;
            return null;
        }
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (!item) // Если поставили в пустую ячейку
        {
            DragHandler.itemBeingDragged.transform.SetParent(transform);
        }

        if (item.Equals(DragHandler.itemBeingDragged)) // Если в начальную ячейку
            DragHandler.lastDropped = true;
    }
}
