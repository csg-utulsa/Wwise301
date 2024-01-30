using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AkOnDropdownSetState : MonoBehaviour
{
    public bool AllowNonDropdownCalls = false;

    [Header("UI Objects")]
    public Dropdown dropdown;

    [Header("Wwise")]
    public List<AK.Wwise.State> ListOfStates;

    private void Awake()
    {
        if (dropdown == null) {
            dropdown = GetComponent<Dropdown>();
        }
        if (ListOfStates == null || dropdown.options.Count != ListOfStates.Count) {
            Debug.LogError("Wwise: Options in "+ dropdown.gameObject.name+" does not equal options in "+this.name);
        }
    }

    public void SetStateBasedOnDropdown() {
        dropdown.Hide();
        SetState();
    }

    public void SetStateValue(int element) {
        if (AllowNonDropdownCalls) {
            dropdown.value = element;
            dropdown.RefreshShownValue();
            //SetState();
        }
    }

    private void SetState() {
        if (ListOfStates != null)
        {
            ListOfStates[dropdown.value].SetValue();
            print("Wwise: State set: " + ListOfStates[dropdown.value].Name);
        }
    }

    public void AllowNonDropdownCall(bool condition) {
        AllowNonDropdownCalls = condition;
    }
}
