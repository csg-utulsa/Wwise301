using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetTimeOfDayRTPC : MonoBehaviour
{
    // Start is called before the first frame update
    public AK.Wwise.RTPC TimeOfDayRTPC;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        TimeOfDayRTPC.SetGlobalValue(GameManager.TimeOfDay);
    }
}
