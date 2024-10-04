using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Aether
{
    [CreateAssetMenu(fileName = "Data", menuName = "Aether/Recipe Registry")]
    public class RecipeRegistry : ScriptableObject
    {
        public List<Recipe> entries = new(); 
    }
}