using _Project.Ray_Tracer.Scripts.RT_Scene;
using _Project.Texturer.Scripts;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace _Project.UI.Scripts.Control_Panel
{
    /// <summary>
    /// Texture properties
    /// </summary>
    public class TextureProperties : MonoBehaviour
    {
        private TextureManager textureManager;
        private RTMesh mesh;

        [SerializeField]
        private TMP_Dropdown viewModeDropdown;
        [SerializeField]
        private FloatEdit wireframeWidthEdit;
        [SerializeField]
        private BoolEdit toggleUVEdit;
        [SerializeField]
        private FloatEdit UVScaleEdit;



        void Awake() {
            viewModeDropdown.onValueChanged.AddListener(mode => {textureManager.VFEViewer.ViewMode = (VFEViewer.Mode) mode;});
            wireframeWidthEdit.OnValueChanged.AddListener(width => {textureManager.VFEViewer.WireframeWidth = width;});
            toggleUVEdit.OnValueChanged.AddListener((value) => {textureManager.UVProjection.DisplayUV = value; UVScaleEdit.gameObject.SetActive(value);});
            UVScaleEdit.OnValueChanged.AddListener(width => {textureManager.UVProjection.UVScale = width;});
        }
        /// <summary>
        /// Hide the shown mesh properties.
        /// </summary>
        public void Hide()
        {
            gameObject.SetActive(false);
            mesh = null;
            if (textureManager != null)
            {
                textureManager.DeleteEnvironment();
                textureManager = null;
            }
        }

        public void Show(RTMesh mesh)
        {
            gameObject.SetActive(true);
            this.mesh = mesh;
            mesh.transform.hasChanged = false;
            if (mesh == null)
                Debug.LogError("Mesh is null");
            
            textureManager = TextureManager.Get();
            if (textureManager == null)
                Debug.LogError("TextureManager is null");
            textureManager.InstantiateEnvironment(mesh);

            viewModeDropdown.value = viewModeDropdown.options.FindIndex(option => option.text == textureManager.VFEViewer.ViewMode.ToString());
            wireframeWidthEdit.Value = textureManager.VFEViewer.WireframeWidth;
            toggleUVEdit.IsOn = textureManager.UVProjection.DisplayUV;
            UVScaleEdit.Value = textureManager.UVProjection.UVScale;
        }
    }
}