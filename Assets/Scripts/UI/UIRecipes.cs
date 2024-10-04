using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Aether
{
    public class UIRecipes : MonoBehaviour
    {
        public GameObject prefabRecipeItem;
        
        public Transform recipesContainer;

        private void Start()
        {
            Refresh();
        }

        [Button]
        public void Refresh()
        {
            recipesContainer.DestroyChildren();
            
            ItemManager.instance.recipes.entries.ForEach(e =>
            {
                var recipeItem = Instantiate(prefabRecipeItem, recipesContainer).GetComponent<UIRecipeItem>();
                recipeItem.UpdateRecipe(e);
            });
        }
    }
}