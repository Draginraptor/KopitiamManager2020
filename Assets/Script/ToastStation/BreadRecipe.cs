using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Adds an option to the Create menu, with the menu name Bread Recipe
// Creates a new ScriptableObject with default name New Bread Recipe
[CreateAssetMenu(fileName = "New Bread Recipe", menuName = "Bread Recipe")]
public class BreadRecipe : ScriptableObject
{
    public BreadType breadName;
    public List<BreadIngredient> allowedIngredients = new List<BreadIngredient>();
    public bool isToasted = false;
    
    public bool QualifiesForRecipe(BreadFSM bread, List<BreadIngredient> fillings)
    {
        // Check for the toasted quality
        if(bread.HasBeenToasted != isToasted)
        {
            return false;
        }

        for (int i = 0; i < allowedIngredients.Count; i++)
        {
            // If one of the ingredients is missing, recipe fails
            if(!fillings.Contains(allowedIngredients[i]))
            {
                return false;
            }
        }

        for (int i = 0; i < fillings.Count; i++)
        {
            // If the ingredient is not allowed, the recipe fails
            if(!allowedIngredients.Contains(fillings[i]))
            {
                return false;
            }
        }

        // If all passed, recipe succeeds
        return true;
    }
}
