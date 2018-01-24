using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

[RequireComponent(typeof(NetworkIdentity))]
public class GameController : NetworkBehaviour
{
    private VictoryCondition[] victoryConditions;
    private bool isGameRunning = false;


    void Update()
    {
        if (isServer && isGameRunning && victoryConditions != null)
        {
            foreach (VictoryCondition victoryCondition in victoryConditions)
            {
                if (victoryCondition.GameFinished())
                {
                    Debug.Log("GAME FINISHED: " + victoryCondition.GetDescription());

                    isGameRunning = false;
                    return;
                }
            }
        }
    }

    public void StartGame()
    {
        victoryConditions = FindObjectsOfType<VictoryCondition>();
        if (victoryConditions != null)
        {
            Player[] players = FindObjectsOfType<Player>();

            foreach (VictoryCondition victoryCondition in victoryConditions)
            {
                victoryCondition.players = players;
            }
        }
    }
}
