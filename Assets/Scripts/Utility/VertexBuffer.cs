using Aether;
using System;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class VertexBuffer
{
    public struct Vertex
    {
        public Vector3 pos;
        public Vector2 uv;
        public Vector3 norm;

        public override bool Equals(object obj)
        {
            if (obj is Vertex vertex)
            {
                return pos.Equals(vertex.pos) && uv.Equals(vertex.uv) && norm.Equals(vertex.norm);
            }
            return false;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 17;
                hash = hash * 31 + pos.GetHashCode();
                hash = hash * 31 + uv.GetHashCode();
                hash = hash * 31 + norm.GetHashCode();
                return hash;
            }
        }

        public Vertex SetNorm(Vector3 norm)
        {
            this.norm = norm;
            return this;
        }
    }

    public List<Vertex> Vertices;
    public List<Int32> Indices;

    public VertexBuffer(int initialCapacity = 0)
    {
        Vertices = new List<Vertex>(initialCapacity);
        Indices = new List<int>(initialCapacity);
    }

    public void PushVertex(float3 pos, float2 uv, float3 norm)
    {
        Vertices.Add(new Vertex { pos=pos, uv=uv, norm=norm });
    }
    public void RemoveVertex(int index, int count = 1)
    {
        Vertices.RemoveRange(index, count);
    }
    public void Clear()
    {
        Vertices.Clear();
        Indices.Clear();
    }

    public bool IsIndexed()
    {
        return Indices.Count > 0;
    }

    public int VertexCount()
    {
        return IsIndexed() ? Indices.Count : Vertices.Count;
    }

    public int NumTriangles() => 
        VertexCount() / 3;

    public Vertex GetVertex(int idx)
    {
        return IsIndexed() ? Vertices[Indices[idx]] : Vertices[idx];
    }

    public void ComputeNormalsFlat() 
    {
        int numTris = NumTriangles();
        for (int i = 0;i < numTris; i++) {
            int v = i * 3;
            var p0 = Vertices[v].pos;
            var p1 = Vertices[v+1].pos;
            var p2 = Vertices[v+2].pos;

            var n = Vector3.Cross(p1 - p0, p2 - p0).normalized;

            Vertices[v] = Vertices[v].SetNorm(n); 
            Vertices[v+1] = Vertices[v+1].SetNorm(n); 
            Vertices[v+2] = Vertices[v+2].SetNorm(n); 
        }
    }
    
    // HOLY SH*T, this performance would be terrible. compared to Rust semantic.
    // public void ComputeNormalsSmooth()
    // {
    //     float SCALE = 100;
    //
    //     var pos2norm = new Dictionary<int3, float3>();
    //     
    //     int numTris = NumTriangles();
    //     for (int tri_i = 0;tri_i < numTris; tri_i++) {
    //         var p0 = GetVertex(tri_i * 3).pos;
    //         var p1 = GetVertex(tri_i * 3 + 1).pos;
    //         var p2 = GetVertex(tri_i * 3 + 2).pos;
    //     
    //         var n = Vector3.Cross(p1 - p0, p2 - p0).normalized;
    //     
    //         var a0 = Vector3.Angle(p1 - p0, p2 - p0);
    //         var a1 = Vector3.Angle(p2 - p1, p0 - p1);
    //         var a2 = Vector3.Angle(p0 - p2, p1 - p2);
    //
    //         // THIS is SH*T
    //         pos2norm.TryAdd(new int3(p0 * SCALE), float3.zero);
    //         pos2norm[new int3(p0 * SCALE)] += new float3(n * a0);
    //         pos2norm.TryAdd(new int3(p1 * SCALE), float3.zero);
    //         pos2norm[new int3(p1 * SCALE)] += new float3(n * a1);
    //         pos2norm.TryAdd(new int3(p2 * SCALE), float3.zero);
    //         pos2norm[new int3(p2 * SCALE)] += new float3(n * a2);
    //     }
    //     
    //     // wasted
    //     foreach (var key in new List<int3>(pos2norm.Keys)) {
    //         pos2norm[key] = math.normalize(pos2norm[key]);
    //     }
    //     // foreach (var norm in pos2norm) {   // modify is not allowed while iterating
    //     //     pos2norm[norm.Key] = math.normalize(norm.Value);
    //     // }
    //     
    //     for (int i = 0;i < Vertices.Count; i++) {
    //         Vertices[i] = Vertices[i].SetNorm(pos2norm[new int3(Vertices[i].pos * SCALE)]);
    //     }
    // }

    public VertexBuffer ComputeIndexed()
    {
        VertexBuffer indexed = new();
        Dictionary<Vertex, int> vertex2index = new();

        foreach (Vertex vert in Vertices)
        {
            if (vertex2index.TryGetValue(vert, out int index))
            {
                indexed.Indices.Add(index);
            }
            else
            {
                index = indexed.Vertices.Count;
                vertex2index[vert] = index;
                indexed.Indices.Add(index);
                indexed.Vertices.Add(vert);
            }
        }
        return indexed;
    }

    public Mesh ToMesh()
    {
        Mesh mesh = new Mesh();

        // Populate the vertices, normals, and UVs
        Vector3[] positions = new Vector3[Vertices.Count];
        Vector3[] normals = new Vector3[Vertices.Count];
        Vector2[] uvs = new Vector2[Vertices.Count];

        for (int i = 0; i < Vertices.Count; i++)
        {
            positions[i] = Vertices[i].pos;
            normals[i] = Vertices[i].norm;
            uvs[i] = Vertices[i].uv;
        }

        mesh.vertices = positions;
        mesh.normals = normals;
        mesh.uv = uvs;

        if (IsIndexed())
        {
            mesh.triangles = Indices.ToArray();
        }
        else
        {
            // Use non-indexed geometry, create a sequential index buffer
            mesh.triangles = Maths.Sequence(Vertices.Count);
        }

        mesh.RecalculateBounds();
        // mesh.RecalculateTangents(); // WARNING: Dont use. this will cause normal dirty ring artifacts.

        return mesh;
    }
}