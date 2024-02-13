using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetTimeOfDayRTPC : MonoBehaviour
{
    public AK.Wwise.RTPC TimeOfDayRTPC;
    // Update is called once per frame
    void Update()
    {
        TimeOfDayRTPC.SetGlobalValue(GameManager.TimeOfDay);
    }
}
