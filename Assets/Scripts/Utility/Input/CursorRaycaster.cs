using System;
using Sirenix.OdinInspector;
using Unity.Mathematics;
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

            m_TerrainMaterial.SetVector("_HighlightPosRadius", new float4(hitResult.point, 2));
        }
    }
}