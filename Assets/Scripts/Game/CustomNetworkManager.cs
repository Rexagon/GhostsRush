using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class CustomNetworkManager : NetworkManager
{
    public SpawnPoint[] spawnPoints;
    private bool[] occupiedSeats = new bool[2];

    public PlayerResources resourcesPrefab;

    public Player playerPrefabDefault;
    public Player playerPrefabVR;

    public void Awake()
    {
        spawnPoints = FindObjectsOfType<SpawnPoint>();
        if (spawnPoints.Length < 2)
        {
            Debug.LogError("There is not enough spawn points!");
        }
    }

    public override void OnServerAddPlayer(NetworkConnection connection, short playerControllerId)
    {
        int seat;

        if (occupiedSeats[0]) seat = 1;
        else if (occupiedSeats[1]) seat = 0;
        else seat = Random.Range(0, 2);

        SpawnPoint spawnPoint = spawnPoints[seat];
        Transform playerPosition = spawnPoint.transform;
        occupiedSeats[seat] = true;

        Player player = Instantiate(playerPrefabDefault, playerPosition.position, playerPosition.rotation);
        NetworkServer.AddPlayerForConnection(connection, player.gameObject, playerControllerId);

        PlayerResources resources = Instantiate(resourcesPrefab);
        NetworkServer.Spawn(resources.gameObject);

        player.RpcSetResources(resources.gameObject);
        player.colorId = spawnPoint.colorId;

        spawnPoint.castle.RpcSetOwner(player.gameObject);
    }
}
