using UnityEngine;
using UnityEngine.EventSystems;

public class SelectableLetterBlock : LetterBlock, IDragHandler, IEndDragHandler
{
    public void OnDrag(PointerEventData eventData)
    {
        Vector3 pos = Camera.main.ScreenToWorldPoint(eventData.position);
        pos.z = transform.position.z;
        transform.position = pos;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        // Handle drop logic here
        Debug.Log("Dropped on: " + eventData.position);
    }
}
