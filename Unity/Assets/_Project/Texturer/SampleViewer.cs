using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// A UI class that displays the current sampling method of a location.
/// Has multiple SampleViews
/// </summary>
public class SampleViewer : MonoBehaviour {

    /// <summary>
    /// A UI class for a specific sampling method
    /// </summary>
    public abstract class SampleView : MonoBehaviour {
        /// <summary>
        /// Sets the view to the given sample.
        /// 
        /// Triggered as an event.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="data">Must be of type SampleData</param>
        public abstract void SetSample(Component sender, object data);
    }

    [Header("Components")]
    [SerializeField]
    private Button closeButton;
    [SerializeField]
    private SampleView emptySampleView;
    [SerializeField]
    private SampleView pointSampleView;

    [SerializeField]
    private SampleView bilinearSampleView;

    [Header("Events")]
    [SerializeField]
    private GameEvent onDisableSampling;

    private void Awake() {
        closeButton.onClick.AddListener(() => onDisableSampling.Raise(closeButton, null));
    }

    /// <summary>
    /// Enables and disables the relevant <see cref="SampleView">
    /// </summary>
    /// <param name="mode">Which mode to activate</param>
    private void ActivateMode(SamplingMode? mode) {
        emptySampleView.gameObject.SetActive(mode == null);
        pointSampleView.gameObject.SetActive(mode == SamplingMode.Point);
        bilinearSampleView.gameObject.SetActive(mode == SamplingMode.Bilinear);
    }

    /// <summary>
    /// Activates ActivateMode from an event
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="data"></param>
    public void ActivateMode(Component sender, object data) {
        if (!(data is SamplingMode) && data != null) return;
        ActivateMode((SamplingMode?) data);
    }
}
