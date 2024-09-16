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

    public Vertex GetVertex(int idx)
    {
        return IsIndexed() ? Vertices[Indices[idx]] : Vertices[idx];
    }

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
        // mesh.RecalculateTangents(); // Optional

        return mesh;
    }
}