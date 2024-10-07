#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

public class ShaderConverter : EditorWindow
{
    GameObject selectedObject;
    Shader newShader;
    float metallic = 0f;
    float smoothness = 0.5f;

    [MenuItem("Tools/ShaderConverter")]
    public static void ShowWindow()
    {
        GetWindow<ShaderConverter>("Shader Converter");
    }

    private void OnGUI()
    {
        GUILayout.Label("Shader Converter", EditorStyles.boldLabel);

        selectedObject = EditorGUILayout.ObjectField("Target GameObject", selectedObject, typeof(GameObject), true) as GameObject;
        newShader = EditorGUILayout.ObjectField("New Shader", newShader, typeof(Shader), false) as Shader;

        metallic = EditorGUILayout.Slider("Metallic", metallic, 0f, 1f);
        smoothness = EditorGUILayout.Slider("Smoothness", smoothness, 0f, 1f);

        if (GUILayout.Button("Convert Shaders"))
        {
            ConvertShaders(selectedObject, newShader);
        }
    }

    private void ConvertShaders(GameObject target, Shader shader)
    {
        if (target == null || shader == null)
        {
            Debug.LogError("Target or Shader is not specified.");
            return;
        }

        Renderer[] renderers = target.GetComponentsInChildren<Renderer>();
        foreach (var renderer in renderers)
        {
            foreach (var material in renderer.sharedMaterials)
            {
                if (material == null) continue;

                // Preserve texture
                Texture mainTexture = material.mainTexture;

                // Change shader
                material.shader = shader;

                // Set new shader properties
                if (material.HasProperty("_Metallic"))
                {
                    material.SetFloat("_Metallic", metallic);
                }
                if (material.HasProperty("_Smoothness"))
                {
                    material.SetFloat("_Smoothness", smoothness);
                }

                // Reassign texture
                material.mainTexture = mainTexture;
            }
        }
    }
}
#endif