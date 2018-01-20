using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

[RequireComponent(typeof(NetworkIdentity))]
public class PlayerResources : NetworkBehaviour
{
    [SerializeField]
    [SyncVar] private int meal;

    [SerializeField]
    [SyncVar] private int mana;

    [SyncVar] public int mealLimit;
    [SyncVar] public int manaLimit;

    public int startMeal = 200;
    public int startMana = 0;

    private void Start()
    {
        if (!isServer)
            return;

        meal = startMeal;
        mana = startMana;
    }

    public int GetMeal()
    {
        return meal;
    }

    public void AddMeal(int value)
    {
        if (!isServer)
            return;

        meal = Mathf.Clamp(meal + value, 0, mealLimit);
    }

    public int GetMana()
    {
        return mana;
    }

    public void AddMana(int value)
    {
        if (!isServer)
            return;

        mana = Mathf.Clamp(mana + value, 0, manaLimit);
    }
}
