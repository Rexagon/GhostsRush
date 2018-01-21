using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BuildingSelectionButton : MonoBehaviour
{
    public Text costLabel;
    public Building building;

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

        if (building != null)
        {
            costLabel.text = building.cost.ToString();
        }
    }

    public void Select()
    {
        if (inputController == null)
        {
            return;
        }

        BuildingSelectionButton[] selectionButtons = FindObjectsOfType<BuildingSelectionButton>();
        foreach (BuildingSelectionButton selectionButton in selectionButtons)
        {
            selectionButton.button.colors = normalColors;
        }

        button.colors = selectedColors;
        inputController.SelectBuilding(building);
    }

    public void Deselect()
    {
        button.colors = normalColors;
    }
}
