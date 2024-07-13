
using UnityEngine;

public class DebugBasisView : MonoBehaviour
{
    public float m_Length = 16;

    void Update()
    {

        Debug.DrawLine(transform.position, transform.right * 16, Color.red);
        Debug.DrawLine(transform.position, transform.up * 16, Color.green);
        Debug.DrawLine(transform.position, transform.forward * 16, Color.blue);
    }
}