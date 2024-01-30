using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableObjectOnAwake : MonoBehaviour
{
    void Awake() {
        gameObject.SetActive(true);
    }
}
