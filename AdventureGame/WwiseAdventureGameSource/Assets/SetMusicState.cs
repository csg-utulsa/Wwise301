using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetMusicState : MonoBehaviour
{
    public AK.Wwise.State OnTriggerEnterState;
    public static AK.Wwise.State OnTriggerExitState;
    public static List<AK.Wwise.State> ListOfStates = new List<AK.Wwise.State>();

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            ListOfStates.Insert(0, OnTriggerEnterState);
            ListOfStates[0].SetValue();
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            ListOfStates.Remove(OnTriggerEnterState);
            if (ListOfStates.Count > 0) {
                ListOfStates[0].SetValue();
            }
            else
            {
                OnTriggerExitState.SetValue();
            }
            
        }
    }
}
