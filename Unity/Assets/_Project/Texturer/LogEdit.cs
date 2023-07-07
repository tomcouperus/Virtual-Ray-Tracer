using System.Collections;
using System.Collections.Generic;
using _Project.UI.Scripts.Control_Panel;
using UnityEngine;

/// <summary>
/// A UI class that edits a floating point value as a slider, using logarithmic steps.
/// </summary>
public class LogEdit : FloatEdit {
    [SerializeField]
    private float logBase = 2;
    protected override float CorrectValue(float value) {
        float power = (float)System.Math.Round(Mathf.Log(value, logBase), Digits);
        return Mathf.Clamp(Mathf.Pow(2, power), MinValue, MaxValue);
    }
}
