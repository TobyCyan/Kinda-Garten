using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class SelectableLetterBlock : LetterBlock, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private Vector3 originalPosition;
    private Collider2D collider;
    [SerializeField] private SpriteRenderer blockRenderer;
    [SerializeField] private TextMeshPro textMesh;
    private int originalBlockLayerOrder;
    private int originalTextLayerOrder;

    private void Start()
    {
        originalPosition = transform.position;
        collider = GetComponent<Collider2D>();
        originalBlockLayerOrder = blockRenderer.sortingOrder;
        originalTextLayerOrder = textMesh.sortingOrder;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        collider.enabled = false;
        ElevateSortingOrder();
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
        ResetSortingOrder();
    }

    private void ElevateSortingOrder()
    {
        blockRenderer.sortingOrder = 100;
        textMesh.sortingOrder = 101;
    }

    private void ResetSortingOrder()
    {
        blockRenderer.sortingOrder = originalBlockLayerOrder;
        textMesh.sortingOrder = originalTextLayerOrder;
    }
}
