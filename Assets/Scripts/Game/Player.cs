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

    private bool sessionCompleted = false;

    void Start()
    {
        units = new List<GameUnit>();
    }

    private void OnDestroy()
    {
        if (!sessionCompleted)
        {
            PlayerPrefs.SetString("last_message", "Disconnected from game");
            SceneManager.LoadScene("main_menu");
        }
    }

    void Update()
    {
        if (!isLocalPlayer) return;

        // Unit placing logic
        GameUnit placeableUnit = inputController.GetPlaceableUnit();
        if (placeableUnit != null)
        {            
            FieldCell cell;
            if (placeableUnit.cost <= resources.GetMeal() &&
                (cell = inputController.GetHoveredCell()) != null)
            {
                cell.Highlight();

                if (inputController.AcceptButtonPressed())
                {
                    if (placeableUnit.tag == "Building")
                    {
                        CmdPlaceBuilding(cell.gameObject, placeableUnit.GetComponent<NetworkIdentity>().assetId);
                    }
                    else if (placeableUnit.tag == "Pawn")
                    {
                        
                    }
                }
            }
        }

        // Selecting units logic
        if (inputController.SelectButtonPressed())
        {
            GameUnit unit = inputController.GetHoveredUnit();
            if (unit != null)
            {
                inputController.SelectUnit(unit);
            }
        }

        // Deselecting buildings logic
        if (inputController.RejectButtonPressed())
        {
            inputController.SelectPlaceableUnit(null);
        }

        // Exit logic
        if (inputController.ExitButtonPressed())
        {
            StopNetworkManager();
        }

        // Update input controller
        if (isLocalPlayer && resources != null)
        {
            inputController.SetMealAmount(resources.GetMeal());
            inputController.SetManaAmount(resources.GetMana());
        }
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
            inputController.SetLobbyEnabled(false);
        }
    }

    private void StopNetworkManager()
    {
        if (isServer)
        {
            NetworkManager.singleton.StopHost();
        }
        else if (isLocalPlayer)
        {
            NetworkManager.singleton.StopClient();
        }
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
        sessionCompleted = true;
        Debug.Log("GAME FINISHED: " + description);
    }

    [ClientRpc]
    public void RpcLoseGame(string description)
    {
        sessionCompleted = true;
        Debug.Log("GAME FINISHED: " + description);
    }
}
