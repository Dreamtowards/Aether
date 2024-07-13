
using UnityEngine;

// Would this cause masive Repaint?
//[ExecuteAlways]

public class DebugBasisView : MonoBehaviour
{
    public float m_Length = 16;

    private void OnDrawGizmos()
    {
        DrawBasis();
    }

    private void DrawBasis()
    {
        Color originalColor = Gizmos.color;

        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + transform.right * m_Length);

        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position, transform.position + transform.up * m_Length);

        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.position, transform.position + transform.forward * m_Length);

        Gizmos.color = originalColor;
    }
}