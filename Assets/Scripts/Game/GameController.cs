using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(NetworkIdentity))]
public class GameController : NetworkBehaviour
{    
    [Header("Players config")]
    public Player[] playerPrefabs;
    public PlayerResources resourcesPrefab;
    public ushort playerCount = 2;

    private SpawnPoint[] spawnPoints;
    private VictoryCondition[] victoryConditions;
    
    public bool IsRunning { get; private set; }
    public List<Player> Players { get; private set; }


    private void Start()
    {
        spawnPoints = FindObjectsOfType<SpawnPoint>();
        Assert.AreEqual(playerCount, spawnPoints.Length);
    }

    void Update()
    {
        if (isServer && IsRunning && victoryConditions != null)
        {
            foreach (VictoryCondition victoryCondition in victoryConditions)
            {
                if (victoryCondition.GameFinished())
                {                    
                    IsRunning = false;
                    return;
                }
            }
        }
    }

    public bool StartGame(List<MainNetworkManager.Connection> connections)
    {
        if (!isServer) return false;

        int connectionCount = connections.Count;

        // Initialize players
        Assert.AreNotEqual(0, playerPrefabs.Length);

        Players = new List<Player>();

        int currentConnectionIndex = 0;
        foreach (SpawnPoint spawnPoint in spawnPoints)
        {
            if (currentConnectionIndex >= connectionCount) break;

            MainNetworkManager.Connection connection = connections[currentConnectionIndex];

            // Select player prefab
            Player playerPrefab;
            if (connection.selectedPlayerPrefab < playerPrefabs.Length)
            {
                playerPrefab = playerPrefabs[connection.selectedPlayerPrefab];
            }
            else return false;

            // Spawn player prefab
            Transform playerPosition = spawnPoint.transform;            
            Player player = Instantiate(playerPrefab, playerPosition.position, playerPosition.rotation);
            if (!NetworkServer.AddPlayerForConnection(connection.networkConnection, player.gameObject, connection.playerControllerId))
            {
                Debug.Log("Successfully spawned player");
            }
            player.RpcSetOpponentId((byte)spawnPoint.opponentId);

            // Assign resources to player
            PlayerResources resources = Instantiate(resourcesPrefab);
            NetworkServer.Spawn(resources.gameObject);
            player.RpcSetResources(resources.gameObject);

            // Set castle owner
            spawnPoint.castle.RpcSetOwner(player.gameObject);

            // assign and iterate
            Players.Add(player);
            ++currentConnectionIndex;
        }

        // Initialize victory conditions
        victoryConditions = GetComponents<VictoryCondition>();
        if (victoryConditions != null)
        {
            foreach (VictoryCondition victoryCondition in victoryConditions)
            {
                victoryCondition.players = Players;
            }
        }

        // Start game
        IsRunning = true;

        return true;
    }

    public void StopGame()
    {
        IsRunning = false;
    }

    public void ExitToMenu(string message)
    {
        PlayerPrefs.SetString("last_message", message);
        SceneManager.LoadScene("main_menu");
    }
}
