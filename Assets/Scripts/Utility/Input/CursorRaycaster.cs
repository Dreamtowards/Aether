using System;
using Sirenix.OdinInspector;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

namespace Aether
{
    public struct HitResult
    {
        public bool isHit;
        public Vector3 point, normal;
        public float distance;
        
        public Ray ray;
        public RaycastHit hitInfo;

        public void Reset()
        {
            isHit = false;
        }
    }
    
    public class CursorRaycaster : MonoBehaviour
    {
        public static CursorRaycaster instance;
        
        [ShowInInspector]
        public HitResult hitResult;
        
        public Material m_TerrainMaterial;

        public float m_ModifyRadius = 2;
        public float m_Intensity = 2;
        public int m_TexId = 2;

        void Start()
        {
            instance = this;
        }

        void Update()
        {
            hitResult.Reset();
            
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            hitResult.ray = ray;
            if (Physics.Raycast(ray, out var hit)) 
            {
                hitResult.isHit = true;
                hitResult.hitInfo = hit;
                hitResult.point = hit.point;
                hitResult.normal = hit.normal;
                hitResult.distance = hit.distance;
            }


            if (InputManager.IsPlayingInput)
            {
                m_TerrainMaterial.SetVector("_HighlightPosRadius", new float4(hitResult.point, m_ModifyRadius));

                var LMB = Input.GetMouseButtonDown((int)MouseButton.Left);
                var RMB = Input.GetMouseButtonDown((int)MouseButton.Right);
                if (LMB || RMB)
                {
                    var cs = ChunkSystem.instance;
                        cs.ModifySphere(hitResult.point, m_ModifyRadius, LMB ? -m_Intensity : m_Intensity, m_TexId);
                }
            }
        }
    }
}