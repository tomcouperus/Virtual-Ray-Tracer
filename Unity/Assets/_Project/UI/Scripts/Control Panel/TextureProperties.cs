using _Project.Ray_Tracer.Scripts.RT_Scene;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace _Project.UI.Scripts.Control_Panel
{
    /// <summary>
    /// Texture properties
    /// </summary>
    public class TextureProperties : MonoBehaviour
    {
        private RTMesh mesh;
        public enum Mode
        {
            Vertex,
            Edge,
            Face
        }

        [SerializeField]
        private TMP_Dropdown viewModeDropdown;

        private Mode viewMode = Mode.Vertex;
        public Mode ViewMode { get; set; }


        void Awake() {
            viewModeDropdown.onValueChanged.AddListener(mode => {viewMode = (Mode) mode;});
        }
        /// <summary>
        /// Hide the shown mesh properties.
        /// </summary>
        public void Hide()
        {
            gameObject.SetActive(false);
            mesh = null;
        }

        public void Show(RTMesh mesh)
        {
            gameObject.SetActive(true);
            this.mesh = mesh;
            mesh.transform.hasChanged = false;

            viewModeDropdown.value = viewModeDropdown.options.FindIndex(option => option.text == viewMode.ToString());
        }
    }
}