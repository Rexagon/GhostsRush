using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ResourceType
{
    Meal,
    Mana
}

public class Player : MonoBehaviour
{
    private Dictionary<ResourceType, int> resources, resourceLimits;

    [HideInInspector]
    public List<GameUnit> units;
    
    public int startMealLimit = 0;
    public int startManaLimit = 30;
    
    public int startMeal = 0;
    public int startMana = 0;

    private InputController inputController;
    private UIController uiController;
    
    void Awake()
    {
        resources = new Dictionary<ResourceType, int>();
        resources.Add(ResourceType.Meal, startMeal);
        resources.Add(ResourceType.Mana, startMana);

        resourceLimits = new Dictionary<ResourceType, int>();
        resourceLimits.Add(ResourceType.Meal, startMealLimit);
        resourceLimits.Add(ResourceType.Mana, startManaLimit);

        inputController = GetComponent<InputController>();
        if (inputController == null)
        {
            Debug.LogError("There is no Input Controller assigned to player");
        }

        uiController = GetComponent<UIController>();
        if (uiController == null)
        {
            Debug.LogError("There is no UI Controller assigned to player");
        }
    }

    void Start()
    {
        units = new List<GameUnit>();
    }

    void Update()
    {
        Building selectedBuilding = uiController.GetSelectedBuilding();
        if (selectedBuilding != null)
        {
            Building building = selectedBuilding.GetComponent<Building>();
            
            FieldCell cell;
            if (building.cost <= GetResourceAmount(ResourceType.Meal) &&
                (cell = inputController.GetSelectedCell()) != null)
            {
                cell.Highlight();

                if (inputController.AcceptButtonPressed())
                {
                    AddResource(ResourceType.Meal, -building.cost);
                    cell.PlaceBuilding(building, this);
                }
            }
        }

        UpdateUI();
    }

    public void AddResource(ResourceType resourceType, int value)
    {
        resources[resourceType] = Mathf.Clamp(resources[resourceType] + value, 0, resourceLimits[resourceType]);
    }

    public int GetResourceAmount(ResourceType resourceType)
    {
        return resources[resourceType];
    }

    private void UpdateUI()
    {
        uiController.SetMealAmount(resources[ResourceType.Meal]);
        uiController.SetManaAmount(resources[ResourceType.Mana]);
    }
}
