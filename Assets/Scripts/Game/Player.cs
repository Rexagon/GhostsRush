using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

[System.Serializable]
public enum OpponentId : byte
{
    First,
    Second
}

public class Player : NetworkBehaviour
{
    public OpponentId opponentId { get; private set; }

    public PlayerResources resources;
    
    private InputController inputController;
    

    private void OnDestroy()
    {
        if (!isLocalPlayer) return;

        //PlayerPrefs.SetString("last_message", "Disconnected from game");
        //SceneManager.LoadScene("main_menu");
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
        
        inputController = GameObject.FindWithTag("InputController").GetComponent<InputController>();
        if (inputController == null) return;

        inputController.SetLobbyEnabled(false);

        Camera playerCamera = GetComponentInChildren<Camera>();
        playerCamera.enabled = true;
        AudioListener audioListener = playerCamera.GetComponent<AudioListener>();
        if (audioListener != null)
        {
            audioListener.enabled = true;
        }
        inputController.mainCamera = playerCamera;
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
    public void RpcSetOpponentId(byte opponentId)
    {
        this.opponentId = (OpponentId)opponentId;
    }

    [ClientRpc]
    public void RpcSetResources(GameObject resources)
    {
        this.resources = resources.GetComponent<PlayerResources>();
    }
}
