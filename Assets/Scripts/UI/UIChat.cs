using System;
using UnityEngine;
using UnityEngine.UI;

namespace Aether
{
    public class UIChat : MonoBehaviour
    {
        public InputField m_ChatInput;

        public GameObject m_MessagesContainer;

        public GameObject m_PrefabChatMessage;

        void Start()
        {
            m_ChatInput.onSubmit.AddListener(line =>
            {
                var msg = Instantiate(m_PrefabChatMessage, m_MessagesContainer.transform);
                msg.GetComponent<Text>().text = line;

                FocusInput();
            });
        }

        void Update()
        {
            m_ChatInput.gameObject.SetActive(UIManager.CurrentScreen == gameObject);

            
        }

        void FocusInput()
        {
            m_ChatInput.Select();
            m_ChatInput.ActivateInputField();
        }

        void OnEnable()
        {
            FocusInput();
        }

    }
}