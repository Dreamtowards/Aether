using Sirenix.OdinInspector;
using UnityEngine;

namespace Aether
{
    public class UIRecipeItem : MonoBehaviour
    {
        public UIItemSlot slotInput;
        public UIItemSlot slotOutput;

        [Button]
        public void UpdateRecipe(Recipe recipe)
        {
            slotOutput.SetItemStack(recipe.output);
            
            slotInput.SetItemStack(recipe.inputs[0]);

            var inputsContainer = slotInput.transform.parent;
            inputsContainer.DestroyChildren(delIf: t => t != slotInput.transform);  // Delete other input slots
            
            for (int i = 1; i < recipe.inputs.Count; i++)
            {
                var slot = Instantiate(slotInput.gameObject, inputsContainer).GetComponent<UIItemSlot>();
                slot.SetItemStack(recipe.inputs[i]);
            }
        }

    }
}