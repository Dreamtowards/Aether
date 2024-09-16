using Sirenix.OdinInspector;
using Sirenix.Utilities;
using Unity.Mathematics;
using UnityEngine;

namespace Aether
{
    public class DebugVoxelInfo : MonoBehaviour
    {
        public ChunkSystem m_ChunkSystem;
        
        public bool m_RepositionToCursor;

        public bool m_DrawChunkBound;

        public int3 m_LastPosition;

        [ShowInInspector]
        public Vox m_Vox;

        [Button]
        public void Remesh() {
            m_ChunkSystem.MarkChunkMeshDirty(Chunk.ChunkPos(transform.position));
        }
        [Button]
        public void RemeshAll() {
            m_ChunkSystem.m_Chunks.Keys.ForEach(e => {
                m_ChunkSystem.MarkChunkMeshDirty(e);
            });
        }
        
        void OnDrawGizmos()
        {
            var p = Maths.Floor(transform.position);
            if (math.any(m_LastPosition != p))
            {
                m_LastPosition = p;
                m_ChunkSystem.GetVoxel(p, out m_Vox);
            }
            Vox vox = m_Vox;

            Gizmos.color = vox.IsNil() ? Color.gray : Color.green;
            Gizmos.DrawWireCube((float3)p + 0.5f, (float3)1);

            if (m_DrawChunkBound)
            {
                Gizmos.color = Color.gray * 0.5f;
                Gizmos.DrawCube((float3)Chunk.ChunkPos(p) + 8.0f, Vector3.one * 16.0f);
            }
            
            // if (vox.IsObaque())
            // {
            //     if (Maths.IsFinite(cell.FeaturePoint))
            //     {
            //         Gizmos.color = Color.green;
            //         Gizmos.DrawSphere(base_p + cell.FeaturePoint, 0.05f);
            //     }
            //     if (Maths.IsFinite(cell.Normal))
            //     {
            //         float3 from = base_p + cell.FeaturePoint;
            //         Gizmos.color = Color.yellow;
            //         Gizmos.DrawLine(from, from + cell.Normal);
            //     }
            // }
            // if (m_DbgShowTextNormalValue)
            // {
            //     string mtl = "nil";
            //     if (cell.Mtl != null)
            //     {
            //         mtl = $"{cell.Mtl.RegistryId} {cell.Mtl.Id}";
            //     }
            //     Handles.Label(Maths.IsFinite(cell.FeaturePoint) ? base_p + cell.FeaturePoint + 0.1f : base_p + 0.5f,
            //         $"\nP: {cell.FeaturePoint}\nN: {cell.Normal}\nMtl: {mtl}");
            // }
            // foreach (float3 v in ChunkMesher.SN_VERT)
            // {
            //     float3 p = base_p + v;
            //     Cell c = world.GetCell(p);
            //
            //     if (c.Value > 0)
            //     {
            //         Gizmos.color = Color.gray;
            //         Gizmos.DrawSphere(p, 0.05f);
            //     }
            //
            //     Gizmos.color = Color.white;
            //     Handles.Label(p + 0.1f, c.Value.ToString());
            // }
            // if (m_RepositionToCursor)
            // {
            //     if (Input.GetMouseButtonDown(0))
            //         m_RepositionToCursor = false;
            //
            //     SceneView sceneView = SceneView.lastActiveSceneView;
            //     if (sceneView != null)
            //     {
            //         Camera camera = sceneView.camera;
            //         Ray ray = camera.ScreenPointToRay(Input.mousePosition);
            //         RaycastHit hit;
            //         if (Physics.Raycast(ray, out hit))
            //         {
            //             Transform objectHit = hit.transform;
            //             transform.position = objectHit.position;
            //         }
            //     }
            // }
        }
    }
}