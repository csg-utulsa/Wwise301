using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OnToggleSetState : MonoBehaviour
{
    public Toggle toggle;

    [Header("Wwise")]
    public AK.Wwise.State OnToggleEnabled;
    public AK.Wwise.State OnToggleDisabled;


    public void OnToggleChange() {
        if (toggle.isOn)
        {
            OnToggleEnabled.SetValue();
        }
        else {
            OnToggleDisabled.SetValue();
        }
    }
}
