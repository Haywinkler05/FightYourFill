using UnityEngine;
using Unity.Collections;

namespace LowPolyWater
{
    public class LowPolyWater : MonoBehaviour
    {
        public float waveHeight = 0.5f;
        public float waveFrequency = 0.5f;
        public float waveLength = 0.75f;
        public Vector3 waveOriginPosition = new Vector3(0.0f, 0.0f, 0.0f);

        MeshFilter meshFilter;
        Mesh mesh;
        Vector3[] vertices;
        private NativeArray<Vector3> nativeVertices;

        private void Awake()
        {
            meshFilter = GetComponent<MeshFilter>();
        }

        void Start()
        {
            CreateMeshLowPoly(meshFilter);
            mesh.MarkDynamic();
            // Initialize NativeArray once
            nativeVertices = new NativeArray<Vector3>(vertices.Length, Allocator.Persistent);
        }

        void OnDestroy()
        {
            // MUST dispose NativeArray or it leaks
            if (nativeVertices.IsCreated)
                nativeVertices.Dispose();
        }

        MeshFilter CreateMeshLowPoly(MeshFilter mf)
        {
            mesh = Instantiate(mf.sharedMesh);
            mf.mesh = mesh;
            Vector3[] originalVertices = mesh.vertices;
            int[] triangles = mesh.triangles;
            Vector3[] vertices = new Vector3[triangles.Length];
            for (int i = 0; i < triangles.Length; i++)
            {
                vertices[i] = originalVertices[triangles[i]];
                triangles[i] = i;
            }
            mesh.vertices = vertices;
            mesh.SetTriangles(triangles, 0);
            mesh.RecalculateBounds();
            mesh.RecalculateNormals();
            this.vertices = mesh.vertices;
            return mf;
        }

        void Update()
        {
            GenerateWaves();
        }

        void GenerateWaves()
        {
            for (int i = 0; i < vertices.Length; i++)
            {
                Vector3 v = vertices[i];
                v.y = 0.0f;
                float distance = Vector3.Distance(v, waveOriginPosition);
                distance = (distance % waveLength) / waveLength;
                v.y = waveHeight * Mathf.Sin(Time.time * Mathf.PI * 2.0f * waveFrequency
                    + (Mathf.PI * 2.0f * distance));
                vertices[i] = v;
                nativeVertices[i] = v;
            }
            // Use SetVertices with NativeArray - zero allocation
            mesh.SetVertices(nativeVertices);
        }
    }
}