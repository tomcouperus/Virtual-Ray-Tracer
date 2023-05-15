using System;
using UnityEngine;
using _Project.Ray_Tracer.Scripts.RT_Scene;

namespace _Project.Texturer.Scripts
{
    public class UVProjection
    {
        private RTMesh mesh;
        private RTMesh uvMesh;

        private bool displayUV;
        public bool DisplayUV
        {
            get { return displayUV; }
            set
            {
                displayUV = value;
                uvMesh.gameObject.SetActive(displayUV);
            }
        }

        private float uvScale;
        public float UVScale
        {
            get { return uvScale; }
            set
            {
                uvScale = value;
                ScaleUV(uvScale);
            }
        }

        public UVProjection(RTMesh mesh)
        {
            this.mesh = mesh;
            InstantiateResources();
        }

        void InstantiateResources()
        {
            uvMesh = GameObject.Instantiate(mesh, mesh.transform, true);
            uvMesh.name = mesh.name + "_UV";
            uvMesh.gameObject.SetActive(false);
            
            MapUvToVertices();
            uvMesh.transform.Translate((uvMesh.transform.right + uvMesh.transform.forward) * 2f);
            uvMesh.transform.LookAt(Camera.main.transform, Vector3.right);
        }


        // function that changes the coordinates of the vertices to the UV coordinates

        void MapUvToVertices()
        {   
            Mesh newMesh = new Mesh();
            MeshFilter filter = uvMesh.GetComponent<MeshFilter>();
            Mesh oldMesh = filter.mesh;

            Vector3[] vertices = oldMesh.vertices;
            for (int i = 0; i < vertices.Length; i++) {
                Vector2 uv = oldMesh.uv[i];
                vertices[i] = new Vector3(1-uv.x, 0, 1-uv.y);
            }
            newMesh.vertices = vertices;
            newMesh.triangles = oldMesh.triangles;
            newMesh.uv = oldMesh.uv;
            newMesh.RecalculateNormals();

            filter.mesh = newMesh;
        }

        void ScaleUV(float value)
        {
            uvMesh.transform.localScale = Vector3.one * value;
        }

        public void DestroyResources()
        {
            RemoveChildren();
            // UnityEngine.Object.Destroy( uvMesh);
            mesh = null;
        }

        void RemoveChildren()
        {
            UnityEngine.Object.Destroy(uvMesh.gameObject);
            // mesh.transform.DetachChildren();
        }
    }
}