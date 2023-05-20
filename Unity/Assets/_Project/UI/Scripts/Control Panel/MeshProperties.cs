using _Project.Ray_Tracer.Scripts;
using _Project.Ray_Tracer.Scripts.RT_Scene;
using _Project.UI.Scripts;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace _Project.UI.Scripts.Control_Panel
{
    /// <summary>
    /// A UI class that provides access to the properties of an <see cref="RTMesh"/>. Any changes made to the shown
    /// properties will be applied to the mesh.
    /// </summary>
    public class MeshProperties : MonoBehaviour
    {
        private RTMesh mesh;
        private InteractableMesh iMesh;
        private TextureSampler texSampler;

        [Header("Model settings")]
        [SerializeField]
        private Vector3Edit positionEdit;
        [SerializeField]
        private Vector3Edit rotationEdit;
        [SerializeField]
        private Vector3Edit scaleEdit;
        [SerializeField]
        private BoolEdit showVerticesToggle;
        [SerializeField]
        private BoolEdit showEdgesToggle;
        
        [Header("Material settings")]
        [SerializeField] 
        private TextureEdit textureEdit;
        [SerializeField]
        private DropdownEdit samplingModeEdit;
        [SerializeField]
        private BoolEdit samplingEdit;
        [SerializeField]
        private ColorEdit colorEdit;
        [SerializeField]
        private FloatEdit ambientEdit;
        [SerializeField]
        private FloatEdit diffuseEdit;
        [SerializeField]
        private FloatEdit specularEdit;
        [SerializeField]
        private FloatEdit shininessEdit;
        [SerializeField]
        private TMP_Dropdown typeDropdown; 
        [SerializeField]
        private FloatEdit refractiveIndexEdit;

        [Serializable]
        public class ExternalChange : UnityEvent { };
        public ExternalChange OnExternalTranslationChange, OnExternalRotationChange, OnExternalScaleChange;

        /// <summary>
        /// Show the mesh properties for <paramref name="mesh"/>. These properties can be changed via the shown UI.
        /// </summary>
        /// <param name="mesh"> The <see cref="RTMesh"/> whose properties will be shown. </param>
        public void Show(RTMesh mesh)
        {
            gameObject.SetActive(true);
            this.mesh = mesh;
            mesh.transform.hasChanged = false;

            positionEdit.Value = mesh.Position;
            rotationEdit.Value = mesh.Rotation;
            scaleEdit.Value = mesh.Scale;

            this.texSampler = mesh.GetComponent<TextureSampler>();
            ShowTextureEdit();
            ShowSamplingEdits();

            this.iMesh = mesh.GetComponent<InteractableMesh>();
            ShowInteractibleMesh();

            colorEdit.Color = mesh.Color;
            ambientEdit.Value = mesh.Ambient;
            diffuseEdit.Value = mesh.Diffuse;
            specularEdit.Value = mesh.Specular;
            shininessEdit.Value = mesh.Shininess;
            
            typeDropdown.value = typeDropdown.options.FindIndex(option => option.text == mesh.Type.ToString());
            refractiveIndexEdit.gameObject.SetActive(mesh.Type == RTMesh.ObjectType.Transparent);
            refractiveIndexEdit.Value = mesh.RefractiveIndex;
        }

        private void ShowTextureEdit() {
            if (!texSampler) {
                textureEdit.gameObject.SetActive(false);
                return;
            }
            textureEdit.UpdateTexturePreview(texSampler.CreateTexturePreview());
            textureEdit.gameObject.SetActive(true);
            
        }

        private void ShowSamplingEdits() {
            if (!texSampler || !texSampler.Texture) {
                samplingModeEdit.gameObject.SetActive(false);
                samplingEdit.gameObject.SetActive(false);
                return;
            }
            samplingModeEdit.SetOptions(new List<string>(typeof(SamplingMode).GetEnumNames()));
            samplingModeEdit.Select((int)texSampler.Mode);
            samplingModeEdit.gameObject.SetActive(true);

            // TODO: better layout? For now we're just showing the sampled color.
            samplingEdit.gameObject.SetActive(true);
            samplingEdit.IsOn = texSampler.IsSampling;
        }

        private void ShowInteractibleMesh() {
            if (!iMesh) {
                showVerticesToggle.gameObject.SetActive(false);
                showEdgesToggle.gameObject.SetActive(false);
                return;
            }
            showVerticesToggle.gameObject.SetActive(true);
            showEdgesToggle.gameObject.SetActive(true);

            showVerticesToggle.IsOn = iMesh.ShowVertices;
            showEdgesToggle.IsOn = iMesh.ShowEdges;
        }

        /// <summary>
        /// Hide the shown mesh properties.
        /// </summary>
        public void Hide()
        {
            gameObject.SetActive(false);
            mesh = null;
            iMesh = null;
        }

        private void ChangeObjectType(RTMesh.ObjectType type)
        {
            if (type == mesh.Type) return;
            refractiveIndexEdit.gameObject.SetActive(type == RTMesh.ObjectType.Transparent);
            mesh.ChangeObjectType(type);
            Show(mesh);
        }

        public void UpdateTextureSamplingMode() {
            texSampler.Mode = (SamplingMode)samplingModeEdit.Value;
        }
        
        private void Awake()
        {
            positionEdit.OnValueChanged.AddListener((value) => { mesh.Position = value; });
            rotationEdit.OnValueChanged.AddListener((value) => { mesh.Rotation = value; });
            scaleEdit.OnValueChanged.AddListener((value) => { mesh.Scale = value; });

            showVerticesToggle.OnValueChanged.AddListener((value) => {iMesh.ShowVertices = value; });
            showEdgesToggle.OnValueChanged.AddListener((value) => {iMesh.ShowEdges = value; });

            colorEdit.OnValueChanged.AddListener((value) => { mesh.Color = value; });
            ambientEdit.OnValueChanged.AddListener((value) => { mesh.Ambient = value; });
            diffuseEdit.OnValueChanged.AddListener((value) => { mesh.Diffuse = value; });
            specularEdit.OnValueChanged.AddListener((value) => { mesh.Specular = value; });
            shininessEdit.OnValueChanged.AddListener((value) => { mesh.Shininess = value; });

            typeDropdown.onValueChanged.AddListener( type => ChangeObjectType( (RTMesh.ObjectType) type));

            refractiveIndexEdit.OnValueChanged.AddListener((value) => { mesh.RefractiveIndex = value; });
        }

        private void FixedUpdate()
        {
            // Update the UI based on external changes to the mesh transform (e.g. through the transformation gizmos).
            bool inUI = EventSystem.current.currentSelectedGameObject != null; // Only update if we are not in the UI.
            bool draggingEdit = positionEdit.IsDragging() || rotationEdit.IsDragging() || scaleEdit.IsDragging();
            if (gameObject.activeSelf && mesh.transform.hasChanged && !inUI && !draggingEdit)
            {
                if (positionEdit.Value != mesh.transform.position)
                {
                    positionEdit.Value = mesh.transform.position;
                    OnExternalTranslationChange?.Invoke();
                }
                if (rotationEdit.Value != mesh.transform.eulerAngles)
                {
                    rotationEdit.Value = mesh.transform.eulerAngles;
                    OnExternalRotationChange?.Invoke();
                }
                if (scaleEdit.Value != mesh.transform.localScale)
                {
                    scaleEdit.Value = mesh.transform.localScale;
                    OnExternalScaleChange?.Invoke();
                }
                mesh.transform.hasChanged = false;
            }
        }

        private void Update()
        {
            mesh.transform.hasChanged = false;   // Do this in Update to let other scripts also check
        }
    }
}
