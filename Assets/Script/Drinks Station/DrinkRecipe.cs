using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Adds an option to the Create menu, with the menu name Drink Recipe
// Creates a new ScriptableObject with default name New Drink Recipe
[CreateAssetMenu(fileName = "New Drink Recipe", menuName = "Drink Recipe")]
public class DrinkRecipe : ScriptableObject
{
    public DrinkType drinkName;
    public List<DrinkIngredient> allowedIngredients = new List<DrinkIngredient>();
    
    public bool QualifiesForRecipe(List<DrinkIngredient> ingredientsToTest)
    {
        // Ingredients may be a mix of any of the allowedIngredients 
        // totalling to 8 units with no other ingredients
        if(ingredientsToTest.Count < 8)
        {
            return false;
        }

        for (int i = 0; i < allowedIngredients.Count; i++)
        {
            // If one of the ingredients is missing, recipe fails
            if(!ingredientsToTest.Contains(allowedIngredients[i]))
            {
                return false;
            }
        }

        for (int i = 0; i < ingredientsToTest.Count; i++)
        {
            // If the ingredient is not allowed, the recipe fails
            if(!allowedIngredients.Contains(ingredientsToTest[i]))
            {
                return false;
            }
        }

        // If all passed, recipe succeeds
        return true;
    }
}
