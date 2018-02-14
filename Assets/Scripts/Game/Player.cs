using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

[System.Serializable]
public enum ColorId : byte
{
    First,
    Second
}

public class Player : NetworkBehaviour
{
    [HideInInspector]
    public PlayerResources resources;

    [HideInInspector]
    public List<GameUnit> units;
    
    public ColorId colorId;

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

        // Exit logic
        if (inputController.ExitButtonPressed())
        {
            if (isServer)
            {
                Debug.Log("Stop server");
                NetworkManager.singleton.StopHost();
                LeaveGame();
            }
            else if (isLocalPlayer)
            {
                Debug.Log("Stop client");
                NetworkManager.singleton.StopClient();
                LeaveGame();
            }
        }

        UpdateUI();
    }

    public override void OnStartLocalPlayer()
    {
        base.OnStartLocalPlayer();

        GameObject lobbyCamera = GameObject.FindWithTag("Lobby Camera");
        if (lobbyCamera != null)
        {
            lobbyCamera.GetComponent<Camera>().enabled = false;
        }

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

    public void LeaveGame()
    {
        SceneManager.LoadScene("main_menu");
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

    [ClientRpc]
    public void RpcSetColor(byte colorIdData)
    {
        colorId = (ColorId)colorIdData;
    }

    [ClientRpc]
    public void RpcWonGame(string description)
    {
        Debug.Log("GAME FINISHED: " + description);
    }

    [ClientRpc]
    public void RpcLoseGame(string description)
    {
        Debug.Log("GAME FINISHED: " + description);
    }
}
