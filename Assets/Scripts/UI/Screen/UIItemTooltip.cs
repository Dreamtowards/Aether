using UnityEngine;
using UnityEngine.UI;

namespace Aether
{
    public class UIItemTooltip : MonoBehaviour
    {
        public Text title;
        public Text quantity;
        public Text description;
        public Image itemIcon;

        public Vector3 offset = new(30, -30, 0);

        public void UpdateItemStack(ItemStack itemStack)
        {
            title.text = itemStack.item.name;
            quantity.text = itemStack.count.ToString();
            description.text = itemStack.item.description;
            itemIcon.sprite = itemStack.item.icon;
        }
    }
}