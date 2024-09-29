using System;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Aether
{
    public class UIWorldList : MonoBehaviour
    {
        public WorldInfo[] m_WorldInfos;

        public Transform m_UiListContainer;
        public GameObject m_PrefabWorldListItem;

        private void Start()
        {
            Refresh();
        }

        [Button]
        public void Refresh()
        {
            m_UiListContainer.GetComponentsInChildren<UIWorldListItem>().ForEach(e =>
            {
                Object.Destroy(e.gameObject);
            });

            m_WorldInfos.ForEach(e =>
            {
                var item = Instantiate(m_PrefabWorldListItem, m_UiListContainer).GetComponent<UIWorldListItem>();
                item.Setup(e);
            });
        }
    }
}