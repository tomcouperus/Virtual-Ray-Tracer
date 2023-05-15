using System;
using System.Linq;
using UnityEngine;
using _Project.Ray_Tracer.Scripts.RT_Scene;

namespace _Project.Texturer.Scripts
{
    public class VFEViewer
    {
        private RTMesh mesh;
        private Renderer[] renderers;
        private Material vfeMaterial;
        private Shader vfeShader;

        public enum Mode{ Vertex, Edge, Face }
        private Mode viewMode = Mode.Vertex;
        public Mode ViewMode { 
            get{return viewMode;}
            set{
                viewMode = value;
                UpdateViewMode();
            }
        }

        // [SerializeField, Range(0f,0.2f)]
        private float wireframeWidth;
        public float WireframeWidth
        {
            get{return wireframeWidth;}
            set{
                wireframeWidth = value;
                vfeMaterial.SetFloat("_WireframeWidth", wireframeWidth);
            }
        }

        public VFEViewer(RTMesh mesh)
        {
            this.mesh = mesh;
            InstantiateResources();
        }

        void InstantiateResources()
        {

            renderers = mesh.GetComponentsInChildren<Renderer>();

            vfeMaterial = UnityEngine.Object.Instantiate(Resources.Load<Material>(@"Materials/VFEView"));
            vfeShader = UnityEngine.Object.Instantiate(Resources.Load<Shader>(@"Shaders/VFEViewer"));
            vfeMaterial.shader = vfeShader;
            vfeMaterial.EnableKeyword("VERTEX");

            AddMaterial();
        }

        void AddMaterial() {
            foreach (var renderer in renderers) {
                var materials = renderer.sharedMaterials.ToList();
                materials.Add(vfeMaterial);
                renderer.materials = materials.ToArray();
            }
        }

        void UpdateViewMode()
        {
            foreach (string mode in System.Enum.GetNames(typeof(Mode))){
                if (mode == viewMode.ToString()){
                    vfeMaterial.EnableKeyword(mode.ToUpper());
                } else {
                    vfeMaterial.DisableKeyword(mode.ToUpper());
                }
            }
        }

        public void DestroyResources()
        {
            RemoveMaterial();
            UnityEngine.Object.Destroy(vfeShader);
            UnityEngine.Object.Destroy(vfeMaterial);
        }

        void RemoveMaterial() {
            foreach (var renderer in renderers) {
                var materials = renderer.sharedMaterials.ToList();
                materials.Remove(vfeMaterial);
                renderer.materials = materials.ToArray();
            }
        }   
    }
}