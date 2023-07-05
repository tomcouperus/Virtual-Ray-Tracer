using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/// <summary>
/// A UI class that generates its options based on a list.
/// Any events that should trigger have to be set in the nested TMP_Dropdown object.
/// </summary>
public class DropdownEdit : MonoBehaviour
{
    [SerializeField]
    private TMP_Dropdown dropdown;

    public void SetOptions(List<string> options) {
        dropdown.ClearOptions();
        dropdown.AddOptions(options);
    }

    public void Select(string value) {
        dropdown.value = dropdown.options.FindIndex(option => option.text == value);
    }

    public void Select(int index) {
        dropdown.value = index;
    }

    public int Value {
        get {return dropdown.value;}
    }
}
