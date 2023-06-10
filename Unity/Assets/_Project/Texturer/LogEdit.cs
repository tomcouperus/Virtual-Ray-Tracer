using System.Collections;
using System.Collections.Generic;
using _Project.UI.Scripts.Control_Panel;
using UnityEngine;

public class LogEdit : FloatEdit {
    [SerializeField]
    private float logBase = 2;
    protected override float CorrectValue(float value) {
        float power = (float)System.Math.Round(Mathf.Log(value, logBase), Digits);
        return Mathf.Clamp(Mathf.Pow(2, power), MinValue, MaxValue);
    }
}
