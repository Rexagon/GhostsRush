using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private VictoryCondition[] victoryConditions;

    void Awake()
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

    void Update()
    {
        if (victoryConditions != null)
        {
            foreach (VictoryCondition victoryCondition in victoryConditions)
            {
                if (victoryCondition.GameFinished())
                {
                    Debug.Log("GAME FINISHED: " + victoryCondition.GetDescription());
                }
            }
        }
    }
}
