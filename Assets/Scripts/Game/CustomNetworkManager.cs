using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class CustomNetworkManager : NetworkManager
{
    public Transform[] playerPositions = new Transform[2];
    private bool[] occupiedSeats = new bool[2];

    public PlayerResources resourcesPrefab;

    public Player playerPrefabMouse;
    public Player playerPrefabVR;
    
    public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerId)
    {
        int seat;

        if (occupiedSeats[0]) seat = 1;
        else if (occupiedSeats[1]) seat = 0;
        else seat = Random.Range(0, 2);

        Transform playerPosition = playerPositions[seat];
        occupiedSeats[seat] = true;

        Player player = Instantiate(playerPrefabMouse, playerPosition.position, playerPosition.rotation);
        NetworkServer.AddPlayerForConnection(conn, player.gameObject, playerControllerId);

        Debug.Log(resourcesPrefab == null);

        PlayerResources resources = Instantiate(resourcesPrefab);
        NetworkServer.Spawn(resources.gameObject);
        player.RpcSetResources(resources.gameObject);
    }
}
