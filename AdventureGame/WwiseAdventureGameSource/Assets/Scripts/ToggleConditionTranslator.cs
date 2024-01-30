using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ToggleConditionTranslator : MonoBehaviour
{
    public Toggle toggle;

    public UnityEvent OnTrue;
    public UnityEvent OnFalse;

    public void ToggleCall(Toggle T){
        toggle = T;
        if (toggle.isOn) {
            OnTrue.Invoke();
        } else {
            OnFalse.Invoke();
        }
    }
}
