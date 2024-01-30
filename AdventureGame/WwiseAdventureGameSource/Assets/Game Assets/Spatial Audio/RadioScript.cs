using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RadioScript : MonoBehaviour
{
    public Toggle toggle;
    // Set by toggle
    private bool toggleCondition = false;

    public GameObject Radio;
    private List<GameObject> RadioInstances;

    private Transform playerTransform;

    private void Awake()
    {
        if (toggle == null) {
            toggle = GetComponent<Toggle>();
        }
        toggleCondition = toggle.isOn;
    }

    private void Start()
    {
        playerTransform = PlayerManager.Instance.player.transform;
    }

    public void SetToggleStatus() {
        toggleCondition = toggle.isOn;

        if (toggleCondition)
        {
            RadioInstances = new List<GameObject>();
        }
        else {
            DestroyAllRadios();
        }
    }

    public void MakeRadio()
    {
        if (Radio != null && toggleCondition)
        {
            RadioInstances.Add((Instantiate(Radio, playerTransform.position + playerTransform.forward+ playerTransform.up, Quaternion.identity)) as GameObject);
        }
    }

    void DestroyAllRadios() {
        if (RadioInstances != null)
        {
            for (int ri = 0; ri < RadioInstances.Count; ri++)
            {
                if (RadioInstances[ri] != null)
                {
                    Destroy(RadioInstances[ri]);
                }
            }
            RadioInstances.Clear();
        }
    }
}
