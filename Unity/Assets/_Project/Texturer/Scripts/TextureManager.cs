using System.Collections;
using UnityEngine;
using _Project.Ray_Tracer.Scripts.RT_Scene;

namespace _Project.Texturer.Scripts
{
    public class TextureManager : MonoBehaviour
    {
        private static TextureManager instance;
        private VFEViewer vfeViewer;
        public VFEViewer VFEViewer { get; set; }
        private UVProjection uvProjection;
        public UVProjection UVProjection { get; set; }


        public static TextureManager Get()
        {
            return instance;
        }

        void Awake()
        {
            instance = this;
        }

        public void InstantiateEnvironment(RTMesh mesh)
        {
            VFEViewer = new VFEViewer(mesh);
            UVProjection = new UVProjection(mesh);
        }

        public void DeleteEnvironment()
        {
            // UnityEngine.Object.Destroy(VFEViewer.Mesh.gameObject);
            // VFEViewer.DestroyResources();
            UVProjection.DestroyResources();
        }
    }
}