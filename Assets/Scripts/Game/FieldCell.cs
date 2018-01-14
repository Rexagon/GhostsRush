using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class FieldCell : MonoBehaviour
{
    public Vector2Int position;

    private bool highlighted = false;

    private MeshRenderer meshRenderer;

    void Awake()
    {
        meshRenderer = GetComponent<MeshRenderer>();

        foreach (Transform child in transform)
        {
            DestroyImmediate(child.gameObject);
        }
    }

    public void PlaceBuilding(Building buildingPrefab, Player player)
    {
        if (transform.childCount != 0)
        {
            return;
        }

        Building building = Instantiate(buildingPrefab);
        building.SetOwner(player);
        building.transform.parent = transform;
        building.transform.position = transform.position;
    }

    public void Highlight()
    {
        if (transform.childCount != 0)
        {
            return;
        }

        highlighted = true;
    }

    public void LateUpdate()
    {
        meshRenderer.enabled = highlighted;
        highlighted = false;
    }
}
