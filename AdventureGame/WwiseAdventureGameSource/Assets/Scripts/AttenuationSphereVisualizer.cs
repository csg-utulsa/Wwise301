using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class AttenuationSphere : MonoBehaviour
{
    [ColorUsageAttribute(true, true, 0f, 8f, 0.125f, 3f)]
    public Color color = Color.magenta;
    public float radius = 10;
    public bool AutoRadius = false;
    private void OnDrawGizmosSelected()
    {
        if (AutoRadius)
        {
            AkAmbient m_AkAmbient = GetComponent<AkAmbient>();
            if (m_AkAmbient != null)
            {
                radius = AkWwiseProjectInfo.GetData().GetEventMaxAttenuation(m_AkAmbient.data.Id);
            }
        }
        Gizmos.color= color;
        Gizmos.DrawWireSphere(transform.position, radius);
    }

}
