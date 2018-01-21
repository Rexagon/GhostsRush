using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Player : NetworkBehaviour
{
    [HideInInspector]
    public PlayerResources resources;

    [HideInInspector]
    public List<GameUnit> units;
    
    private InputController inputController;

    void Start()
    {
        units = new List<GameUnit>();
    }

    void Update()
    {
        if (!isLocalPlayer) return;

        // Placing buildings logic
        Building building = inputController.GetSelectedBuilding();
        if (building != null)
        {            
            FieldCell cell;
            if (building.cost <= resources.GetMeal() &&
                (cell = inputController.GetSelectedCell()) != null)
            {
                cell.Highlight();

                if (inputController.AcceptButtonPressed())
                {
                    CmdPlaceBuilding(cell.gameObject, building.GetComponent<NetworkIdentity>().assetId);
                }
            }
        }

        // Deselecting buildings logic
        if (inputController.RejectButtonPressed())
        {
            inputController.SelectBuilding(null);
        }

        UpdateUI();
    }

    public override void OnStartLocalPlayer()
    {
        base.OnStartLocalPlayer();

        Camera playerCamera = GetComponent<Camera>();
        playerCamera.enabled = true;

        GameObject mainObject = GameObject.FindWithTag("Main");
        if (mainObject == null)
        {
            Debug.LogError("There is no Main object on the scene");
        }

        inputController = mainObject.GetComponent<InputController>();
        if (inputController == null)
        {
            Debug.LogError("There is no Input Controller assigned to player");
        }
        else
        {
            inputController.MainCamera = playerCamera;
        }
    }
    
    private void UpdateUI()
    {
        if (!isLocalPlayer || resources == null) return;

        inputController.SetMealAmount(resources.GetMeal());
        inputController.SetManaAmount(resources.GetMana());
    }

    [Command]
    private void CmdPlaceBuilding(GameObject cellObject, NetworkHash128 buildingId)
    {
        FieldCell cell = cellObject.GetComponent<FieldCell>();
        Building building = ClientScene.prefabs[buildingId].GetComponent<Building>();

        if (cell == null || building == null) return;

        cell.PlaceBuilding(building, this);
    }

    [ClientRpc]
    public void RpcSetResources(GameObject resources)
    {
        this.resources = resources.GetComponent<PlayerResources>();
    }
}
