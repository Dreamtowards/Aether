using ImGuiNET;
using UImGui;
using Unity.Mathematics;
using UnityEngine;

namespace Aether
{
    public class Imgui : MonoBehaviour
    {
        public bool m_ShowDockspace = true;
        
        public bool m_ShowDemoWindow = false;

        public bool m_ShowDebugTextInfo = true;

        private void OnLayout(UImGui.UImGui obj)
        {
            if (m_ShowDockspace)
                ShowDockspaceAndMainMenubar();
            if (m_ShowDebugTextInfo)
                ShowDebugTextInfoOverlay();
            
            if (m_ShowDemoWindow)
                ImGui.ShowDemoWindow();
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
        
        
        public void ShowDockspaceAndMainMenubar()
        {
            ImGui.PushStyleVar(ImGuiStyleVar.WindowBorderSize, 0);
            ImGui.PushStyleVar(ImGuiStyleVar.WindowPadding, Vector2.zero);
            ImGui.PushStyleVar(ImGuiStyleVar.TabBarBorderSize, 0);   // No Separator under Dock Window Tab .
            
            ImGui.PushStyleColor(ImGuiCol.MenuBarBg, new Vector4(0.373f, 0.157f, 0.467f, 0.7f));
            
            ImGui.DockSpaceOverViewport(null, ImGuiDockNodeFlags.PassthruCentralNode);
            
            ImGui.PopStyleColor();
            ImGui.PopStyleVar(3);
            
            ShowMainMenubar();
        }

        public void ShowMainMenubar()
        {
            if (ImGui.BeginMainMenuBar())
            {
                if (ImGui.BeginMenu("System"))
                {
                    if (ImGui.BeginMenu("Servers"))
                    {
                        if (ImGui.MenuItem("Connect to server..")) { }
                        ImGui.Separator();
                        ImGui.TextDisabled("Servers:");
                        if (ImGui.SmallButton("+")) {}
                        if (ImGui.IsItemHovered())
                            ImGui.SetTooltip("Add server");
                        ImGui.EndMenu();
                    }
                    ImGui.Separator();
                    if (ImGui.BeginMenu("Open World"))
                    {
                        if (ImGui.MenuItem("New World..")) { }
                        if (ImGui.MenuItem("Open World..")) { }
                        ImGui.Separator();
                        ImGui.TextDisabled("Saves:");
                        if (ImGui.IsItemHovered())
                            ImGui.SetTooltip("click to open default save directory");
                        
                        ImGui.EndMenu();
                    }
                    if (ImGui.MenuItem("Edit World..")) { }
                    if (ImGui.MenuItem("Save World")) { }
                    if (ImGui.MenuItem("Close World")) { }
                    ImGui.Separator();
                    if (ImGui.MenuItem("Settings..")) { }
                    if (ImGui.MenuItem("Mods..", "0 mods loaded")) { }
                    if (ImGui.MenuItem("Assets")) { }
                    if (ImGui.MenuItem("Controls")) { }
                    if (ImGui.MenuItem("About")) { }
                    ImGui.Separator();
                    if (ImGui.MenuItem("Terminate")) { }
                    
                    ImGui.EndMenu();
                }
                if (ImGui.BeginMenu("World"))
                {
                    
                    ImGui.EndMenu();
                }
                if (ImGui.BeginMenu("Graphics"))
                {
                    
                    ImGui.EndMenu();
                }
                if (ImGui.BeginMenu("Audio"))
                {
                    
                    ImGui.EndMenu();
                }
                if (ImGui.BeginMenu("View"))
                {
                    ImGui.SeparatorText("Debug");
                    ImGui.MenuItem("Debug Text Info", null, ref m_ShowDebugTextInfo);
                    ImGui.MenuItem("ImGui Demo Window", null, ref m_ShowDemoWindow);
                    ImGui.MenuItem("Game Controls", null, ref InputManager.instance.enabledGameInputs);
                    ImGui.EndMenu();
                }
                
                ImGui.EndMainMenuBar();
            }
        }

        private void ShowDebugTextInfoOverlay()
        {
            ImGui.SetNextWindowPos(new(-8, 40));
            ImGui.SetNextWindowBgAlpha(0.0f);
            ImGui.Begin("DebugTextInfoOverlay", ImGuiWindowFlags.NoDecoration | ImGuiWindowFlags.NoDocking | ImGuiWindowFlags.AlwaysAutoResize | ImGuiWindowFlags.NoNav);
            
            ImGui.Text($"fps: {1.0f / Time.deltaTime:0.00}, {1.0f / Time.smoothDeltaTime:0.00}; dt: {Time.deltaTime*1000.0:0.00}ms; time: {Time.realtimeSinceStartup:0.00}; t-scale: {Time.timeScale:0.00}");

            ImGui.NewLine();
            
            var world = World.instance;
            var wi = world.Info;
            ImGui.Text($"World: '{wi.Name}', seed: {wi.Seed}; daytime: {wi.DayTime:0.00} {StrDayTime(wi.DayTime)}, len {wi.DayTimeLength}s ({(wi.DayTimeLength/60):0.00}min); inhabited: {wi.TimeInhabited}");

            var cs = FindFirstObjectByType<ChunkSystem>();
            ImGui.Text($"Chunk: loaded: {cs.NumChunks}, loading: {cs.m_ChunksLoading.Count}, mesh_dirty: {cs.m_ChunksMeshDirty.Count}, meshing: {cs.m_ChunksMeshing.Count} ");
            
            ImGui.End();
        }

        public string StrDayTime(float daytime)
        {
            // 0=6am, 0.25=12am, 0.5=6pm
            float hr = daytime * 24 + 6;
            float mn = math.frac(hr) * 60;
            float s = math.frac(mn) * 60;
            
            return $"{math.floor(hr):00}:{math.floor(mn):00}:{math.floor(s):00}";
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