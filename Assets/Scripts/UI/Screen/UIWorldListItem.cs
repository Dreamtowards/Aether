using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Aether
{
    public class UIWorldListItem : MonoBehaviour, IPointerClickHandler
    {
        public Text m_Title, m_Description;
        
        public void OnPointerClick(PointerEventData evt)
        {
            SceneManager.LoadScene("Scenes/World");
        }

        public void Setup(WorldInfo worldInfo)
        {
            m_Title.text = worldInfo.Name;

            worldInfo.TimeMidified = (ulong)DateTime.Now.ToFileTimeUtc();
            var strTimePlayed = Utility.StrTimeDuration((int)worldInfo.TimeInhabited, txHr:"hr ", txMin:"min ", txSec:"sec", forceUnits:false);
            var strTimeModified = DateTime.FromFileTimeUtc((long)worldInfo.TimeMidified).ToString("yyyy-MM-dd  HH:mm"); 
            m_Description.text = $"{strTimePlayed}  ·  {strTimeModified}";
        }
    }
}