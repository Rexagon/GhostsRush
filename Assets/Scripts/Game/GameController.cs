using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

[RequireComponent(typeof(NetworkIdentity))]
public class GameController : NetworkBehaviour
{
    public bool isRunning { get; private set; }

    public Player[] players { get; set; }
    private VictoryCondition[] victoryConditions;

    private void Awake()
    {
        isRunning = false;
    }

    void Update()
    {
        if (isServer && isRunning && victoryConditions != null)
        {
            foreach (VictoryCondition victoryCondition in victoryConditions)
            {
                if (victoryCondition.GameFinished())
                {
                    Debug.Log("GAME FINISHED: " + victoryCondition.GetDescription());
                    
                    isRunning = false;
                    return;
                }
            }
        }
    }

    public void StartGame()
    {
        victoryConditions = GetComponents<VictoryCondition>();
        if (victoryConditions != null)
        {
            Player[] players = FindObjectsOfType<Player>();

            foreach (VictoryCondition victoryCondition in victoryConditions)
            {
                victoryCondition.players = players;
            }
        }
    }

    public void StopGame()
    {
        isRunning = false;
    }
}
