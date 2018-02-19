using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UnitSelectionButton : MonoBehaviour
{
    public Text costLabel;
    public GameUnit gameUnit;

    [HideInInspector]
    public InputController inputController;
    
    private Button button;

    private ColorBlock normalColors;
    private ColorBlock selectedColors;

    void Awake()
    {
        button = GetComponent<Button>();

        normalColors = button.colors;
        selectedColors = normalColors;
        selectedColors.normalColor = normalColors.pressedColor;
        selectedColors.highlightedColor = normalColors.pressedColor;

        button.onClick.AddListener(Select);

        if (gameUnit != null)
        {
            costLabel.text = gameUnit.cost.ToString();
        }
    }

    public void Select()
    {
        if (inputController == null)
        {
            return;
        }

        UnitSelectionButton[] selectionButtons = FindObjectsOfType<UnitSelectionButton>();
        foreach (UnitSelectionButton selectionButton in selectionButtons)
        {
            selectionButton.button.colors = normalColors;
        }

        button.colors = selectedColors;
        inputController.SelectPlaceableUnit(gameUnit);
    }

    public void Deselect()
    {
        button.colors = normalColors;
    }
}
