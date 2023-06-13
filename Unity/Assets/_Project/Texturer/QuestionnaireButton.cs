using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestionnaireButton : MonoBehaviour {
    [SerializeField]
    private string shortFormCode;

    public void OpenQuestionnaire() {
        Application.OpenURL("https://forms.gle/"+shortFormCode);
    }
}
