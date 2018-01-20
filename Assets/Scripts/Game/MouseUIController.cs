using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MouseUIController : UIController
{
    public Text mealCounterText;
    public Text manaCounterText;

    public override void SetMealAmount(int amount)
    {
        mealCounterText.text = amount.ToString();
    }

    public override void SetManaAmount(int amount)
    {
        manaCounterText.text = amount.ToString();
    }
}
