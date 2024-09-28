using System;
using UnityEngine;
using UnityEngine.UI;

namespace Aether
{
    public class UIChat : MonoBehaviour
    {
        public InputField m_ChatInput;

        public GameObject m_MessagesContainer;
        public GameObject m_MessagesView;

        public GameObject m_PrefabChatMessage;

        public float m_TimeLastMessage;
        public float m_TimeAutoHide;

        void Start()
        {
            m_ChatInput.onSubmit.AddListener(line =>
            {
                AppendMessage(line);
                FocusInput();
            });
        }

        void Update()
        {
            bool showInput = UIManager.CurrentScreen == gameObject;
            m_ChatInput.gameObject.SetActive(showInput);

            bool showMessages = m_TimeLastMessage + m_TimeAutoHide > Time.time;
            m_MessagesView.gameObject.SetActive(showMessages || showInput);
        }

        void AppendMessage(string text)
        {
            var msg = Instantiate(m_PrefabChatMessage, m_MessagesContainer.transform);
            msg.GetComponent<Text>().text = text;
            m_TimeLastMessage = Time.time;
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