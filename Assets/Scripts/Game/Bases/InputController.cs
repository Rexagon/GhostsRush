using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public abstract class InputController : MonoBehaviour
{
    public Camera MainCamera { get; set; }

    protected GameUnit currentPlaceableUnit;
    protected GameUnit selectedUnit;

    private void OnDestroy()
    {
        NetworkManager.singleton.StopClient();
        NetworkManager.singleton.StopHost();
    }

    // Lobby
    public abstract void SetLobbyEnabled(bool lobbyEnabled);

    // General input

    public abstract FieldCell GetHoveredCell();

    public abstract GameUnit GetHoveredUnit();

    public abstract bool AcceptButtonPressed();

    public abstract bool RejectButtonPressed();

    public abstract bool SelectButtonPressed();

    public abstract bool ExitButtonPressed();


    // UI input

    public abstract void SetMealAmount(int amount);

    public abstract void SetManaAmount(int amount);

    public virtual void SelectPlaceableUnit(GameUnit unit)
    {
        currentPlaceableUnit = unit;
    }

    public virtual GameUnit GetPlaceableUnit()
    {
        return currentPlaceableUnit;
    }

    public virtual void SelectUnit(GameUnit unit)
    {
        if (unit == selectedUnit)
        {
            return;
        }

        if (selectedUnit != null)
        {
            selectedUnit.SetHighlighted(false);
        }

        selectedUnit = unit;

        if (selectedUnit != null)
        {
            selectedUnit.SetHighlighted(true);
        }
    }

    public virtual GameUnit GetSelectedUnit()
    {
        return selectedUnit;
    }
}
