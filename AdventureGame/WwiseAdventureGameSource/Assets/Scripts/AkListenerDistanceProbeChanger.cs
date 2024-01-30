using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AkListenerDistanceProbeChanger : MonoBehaviour
{
    public AkListenerDistanceProbe AkLDP;
    private AkGameObj probe;

    private void Awake()
    {
        if (AkLDP == null) { // If not assigned already
            AkLDP = GetComponent<AkListenerDistanceProbe>(); // Will automatically try to find the AkListenerDistanceProbeScript.

            if (AkLDP != null)
            {
                probe = AkLDP.distanceProbe;
            }
            else
            {
                Debug.LogWarning(this.name + " cannot find AkListenerDistanceProbe script.");
            }
        }
    }

    public void SetDistanceProbe(AkGameObj newProbe)
    {
        if (AkLDP)
        {
            if (newProbe != AkLDP.distanceProbe) {
                AkLDP.distanceProbe = newProbe;
                if (AkLDP.distanceProbe)
                {
                    var listenerGameObjectID = AkSoundEngine.GetAkGameObjectID(AkLDP.gameObject);
                    var distanceProbeGameObjectID = AkSoundEngine.GetAkGameObjectID(newProbe.gameObject);
                    AkSoundEngine.SetDistanceProbe(listenerGameObjectID, distanceProbeGameObjectID);
                }
            }
        }
        else {
            Debug.LogWarning(this.name + " cannot find AkListenerDistanceProbe script.");
        }
    }
}
