using UnityEngine;

public class Selectable : MonoBehaviour
{
    [SerializeField] private Material selectionMaterial;

    private SpriteRenderer[] spriteRenderers;
    private Material[] originalMaterials;

    private void CacheRenderers()
    {
        if (spriteRenderers != null && spriteRenderers.Length > 0)
            return;

        spriteRenderers = GetComponentsInChildren<SpriteRenderer>(true);
        originalMaterials = new Material[spriteRenderers.Length];

        for (int i = 0; i < spriteRenderers.Length; i++)
        {
            if (spriteRenderers[i] != null)
                originalMaterials[i] = spriteRenderers[i].sharedMaterial;
        }
    }

    public void Select()
    {
        CacheRenderers();

        if (selectionMaterial == null)
            return;

        foreach (var r in spriteRenderers)
        {
            if (r != null)
                r.sharedMaterial = selectionMaterial;
        }
    }

    public void Deselect()
    {
        CacheRenderers();

        if (spriteRenderers == null || originalMaterials == null)
            return;

        for (int i = 0; i < spriteRenderers.Length; i++)
        {
            if (spriteRenderers[i] != null && originalMaterials[i] != null)
                spriteRenderers[i].sharedMaterial = originalMaterials[i];
        }
    }
}