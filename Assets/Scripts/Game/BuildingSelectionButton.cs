using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BuildingSelectionButton : MonoBehaviour
{
    public Building building;

    public UIController uiController { get; set; }
    
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

        button.onClick.AddListener(SetSelected);
    }

    private void SetSelected()
    {
        if (uiController == null)
        {
            return;
        }

        BuildingSelectionButton[] selectionButtons = FindObjectsOfType<BuildingSelectionButton>();
        foreach (BuildingSelectionButton selectionButton in selectionButtons)
        {
            selectionButton.button.colors = normalColors;
        }

        button.colors = selectedColors;
        uiController.SelectBuilding(building);
    }
}
