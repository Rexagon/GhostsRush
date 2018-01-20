using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

[RequireComponent(typeof(BoxCollider))]
[RequireComponent(typeof(NetworkIdentity))]
public class FieldCell : NetworkBehaviour
{
    [HideInInspector]
    public Vector2Int position;

    private bool highlighted = false;

    private MeshRenderer meshRenderer;

    void Start()
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
        NetworkServer.Spawn(building.gameObject);
        RpcAddBuilding(building.gameObject, player.gameObject);

        player.resources.AddMeal(-building.cost);
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

    [ClientRpc]
    private void RpcAddBuilding(GameObject building, GameObject owner)
    {
        building.transform.parent = transform;
        building.transform.position = transform.position;
    }
}
