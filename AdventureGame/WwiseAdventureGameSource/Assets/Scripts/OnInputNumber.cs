using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class OnInputNumber : MonoBehaviour
{
    public KeyCode If;
    public UnityEvent Then;

    private void Update()
    {
        if (Input.GetKeyDown(If)) {
            Then.Invoke();
        }
    }
}
