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
            
            // ImGui.Text($"Hello, world {123}");
            // if (ImGui.Button("Save"))
            // {
            //     Debug.Log("Save");
            // }
            //
            // ImGui.InputText("string", ref _inputText, 100);
            // ImGui.SliderFloat("float", ref _sliderFloatValue, 0.0f, 1.0f);
        }

        private void OnInitialize(UImGui.UImGui obj)
        {
            // runs after UImGui.OnEnable();

            // var io = ImGui.GetIO();
            // var font = io.Fonts.AddFontFromFileTTF("Fonts/menlo.ttf", 24.0f);

            
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
        
        public void AddCustomFont(ImGuiIOPtr io)
        {
            // you can put on StreamingAssetsFolder and call from there like:
            //string fontPath = $"{Application.streamingAssetsPath}/NotoSansCJKjp - Medium.otf";
            string fontPath = $"{Application.streamingAssetsPath}/Fonts/menlo.ttf";
            io.Fonts.AddFontFromFileTTF(fontPath, 14);

            // you can create a configs and do a lot of stuffs
            //ImFontConfig fontConfig = default;
            //ImFontConfigPtr fontConfigPtr = new ImFontConfigPtr(&fontConfig);
            //fontConfigPtr.MergeMode = true;
            //io.Fonts.AddFontDefault(fontConfigPtr);
            //int[] icons = { 0xf000, 0xf3ff, 0 };
            //fixed (void* iconsPtr = icons)
            //{
            //	io.Fonts.AddFontFromFileTTF("fontawesome-webfont.ttf", 18.0f, fontConfigPtr, (System.IntPtr)iconsPtr);
            //}
        }
    }
}