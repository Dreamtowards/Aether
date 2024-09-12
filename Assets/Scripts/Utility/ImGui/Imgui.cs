using ImGuiNET;
using UImGui;
using UnityEngine;

namespace Aether
{
    public class Imgui : MonoBehaviour
    {
        
        [SerializeField]
        public float _sliderFloatValue = 1;

        [SerializeField]
        public string _inputText;

        private void OnLayout(UImGui.UImGui obj)
        {
            // Unity Update method. 
            // Your code belongs here! Like ImGui.Begin... etc.
            
            ImGui.Text($"Hello, world {123}");
            if (ImGui.Button("Save"))
            {
                Debug.Log("Save");
            }

            ImGui.InputText("string", ref _inputText, 100);
            ImGui.SliderFloat("float", ref _sliderFloatValue, 0.0f, 1.0f);
        }

        private void OnInitialize(UImGui.UImGui obj)
        {
            // runs after UImGui.OnEnable();
        }

        private void OnDeinitialize(UImGui.UImGui obj)
        {
            // runs after UImGui.OnDisable();
        }

        private void OnEnable()
        {
            UImGuiUtility.Layout += OnLayout;
            UImGuiUtility.OnInitialize += OnInitialize;
            UImGuiUtility.OnDeinitialize += OnDeinitialize;
        }
        private void OnDisable()
        {
            UImGuiUtility.Layout -= OnLayout;
            UImGuiUtility.OnInitialize -= OnInitialize;
            UImGuiUtility.OnDeinitialize -= OnDeinitialize;
        }
    }
}