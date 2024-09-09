
using UnityEngine;

namespace Aether.Debug
{
    public class DebugTextInfo : MonoBehaviour
    {
        // https://docs.unity3d.com/Manual/gui-Controls.html
        private void OnGUI()
        {
            GUI.Label(new Rect (0,32,500,500),
                $"fps: {1.0f / Time.deltaTime:0.00}, {1.0f / Time.smoothDeltaTime:0.00}; dt: {Time.deltaTime*1000.0:0.00}ms; time: {Time.realtimeSinceStartup:0.00}; t-scale: {Time.timeScale:0.00}");
        }
    }
}