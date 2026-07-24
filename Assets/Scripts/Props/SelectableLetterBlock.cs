using UnityEngine;
using UnityEngine.EventSystems;

public class SelectableLetterBlock : LetterBlock, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private Vector3 originalPosition;
    private Collider2D collider;

    private void Start()
    {
        originalPosition = transform.position;
        collider = GetComponent<Collider2D>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        collider.enabled = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector3 pos = Camera.main.ScreenToWorldPoint(eventData.position);
        pos.z = transform.position.z;
        transform.position = pos;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        transform.position = originalPosition;
        collider.enabled = true;
    }
}
