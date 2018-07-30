using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Networking;
using UnityEngine.Networking.NetworkSystem;

public class MainNetworkManager : NetworkManager
{
    public struct Connection
    {
        public NetworkConnection networkConnection;
        public short playerControllerId;
        public int selectedPlayerPrefab;
    }
    
    [Header("Gameplay")]
    public GameController gameController;
    
    private List<Connection> connections = new List<Connection>();


    public void Start()
    {
        Assert.IsNotNull(gameController);

        if (PlayerPrefs.HasKey("is_server") && PlayerPrefs.GetInt("is_server") == 1)
        {
            StartHost();
        }
        else if (PlayerPrefs.HasKey("network_address"))
        {
            networkAddress = PlayerPrefs.GetString("network_address");
            StartClient();
        }
        else
        {
            gameController.ExitToMenu("Ip address must be specified");
        }
    }

    public override void OnClientConnect(NetworkConnection connection)
    {
        base.OnClientConnect(connection);

        int selectedPlayerPrefab = 0;
        IntegerMessage message = new IntegerMessage(selectedPlayerPrefab);
        ClientScene.AddPlayer(connection, 0, message);
    }

    public override void OnServerAddPlayer(NetworkConnection networkConnection, short playerControllerId, NetworkReader extraData)
    {
        if (gameController.IsRunning || connections.Count >= 2) return;

        // Create connection object
        Connection connection = new Connection
        {
            networkConnection = networkConnection,
            playerControllerId = playerControllerId,
            selectedPlayerPrefab = extraData != null ? extraData.ReadMessage<IntegerMessage>().value : 0
        };
        connections.Add(connection);
        
        // Start game if there are enough connections
        if (connections.Count == gameController.playerCount)
        {
            StartGame();
        }
    }

    public override void OnServerDisconnect(NetworkConnection connection)
    {
        base.OnServerDisconnect(connection);

        Debug.Log("Client disconnected");

        gameController.StopGame();
        StopHost();
    }

    private void StartGame()
    {
        Assert.IsNotNull(connections);

        // Shuffle connections
        int count = connections.Count;
        int last = count - 1;
        for (int i = 0; i < last; ++i)
        {
            int r = Random.Range(i, count);

            Connection temp = connections[i];
            connections[i] = connections[r];
            connections[r] = temp;
        }

        // Start game
        gameController.StartGame(connections);
    }
}
