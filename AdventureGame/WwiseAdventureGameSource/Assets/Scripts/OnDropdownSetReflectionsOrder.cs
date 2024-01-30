using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OnDropdownSetReflectionsOrder : MonoBehaviour
{
    public bool AllowNonDropdownCalls = false;

    [Header("UI Objects")]
    public Dropdown dropdown;

    private void Awake()
    {
        if (dropdown == null)
        {
            dropdown = GetComponent<Dropdown>();
        }
    }

    public void SetReflectionsOrder(int order) {
        AkSoundEngine.SetReflectionsOrder((uint)order, true);
        print("Wwise: Early Reflections Order set to " + (uint)order);
    }

    public void SetDropdownValue(int element)
    {
        if (AllowNonDropdownCalls)
        {
            dropdown.value = element;
            dropdown.RefreshShownValue();
            SetReflectionsOrder(element);
        }
    }

    public void AllowNonDropdownCall(bool condition)
    {
        AllowNonDropdownCalls = condition;
    }
}
