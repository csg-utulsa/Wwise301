using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RhythmActions : MonoBehaviour {
   
    //////////////////////////////////////////////////////////////////////////////////////// TRANSFORM
    [System.Serializable]
    public class ModificationClass {
        [Range(0f, 1f)]
        public float x = 0.079f;
        [Range(0f, 1f)]
        public float y = 0.044f;
        [Range(0f, 1f)]
        public float z = 0.041f;
        [Range(0f, 1f)]
        public float Randomness = 0f;

        [HideInInspector]
        public Vector3 ScaleAmount = Vector3.zero;
        [HideInInspector]
        public Vector3 ScaleOriginal;
    }
    public ModificationClass Modifications;
    public IEnumerator ActionProgress;
    [Range(0.1f, 10f)]
    public float Speed = 2.56f;

    //////////////////////////////////////////////////////////////////////////////////////// FUNCTIONS
    public void Start()
    {
        GameManager.OnMusicAction += PushActions;
        Modifications.ScaleOriginal = transform.localScale;
    }

    public void PushActions() {
        if (ActionProgress != null)
        {
            StopCoroutine(ActionProgress);
            ResetAll();
            ActionProgress = null;
        }
        ActionProgress = TransformPerformer();
        StartCoroutine(ActionProgress);
    }

    public void ResetAll()
    {
        this.transform.localScale = Modifications.ScaleOriginal;
    }

    public IEnumerator TransformPerformer()
    {
        
        float rand = Modifications.Randomness;
        Vector3 ScaleModifications = new Vector3(Modifications.x + Random.Range(0, rand), Modifications.y + Random.Range(0, rand), Modifications.z + Random.Range(0, rand));
        Modifications.ScaleAmount = ScaleModifications + Modifications.ScaleOriginal;

        float speedwRandom = Speed + (rand * 10);

        bool State_IsModified = false;
        while (!State_IsModified)
        {
            transform.localScale = Vector3.Lerp(transform.localScale, Modifications.ScaleAmount, Time.deltaTime * speedwRandom);
            float dist = Vector3.Distance(transform.localScale, Modifications.ScaleAmount);
            if (dist < 0.01f) {
                State_IsModified = true;
            }
            yield return new WaitForEndOfFrame();
        }
        bool State_IsNormal = false;
        while (!State_IsNormal)
        {
            transform.localScale = Vector3.Lerp(transform.localScale, Modifications.ScaleOriginal, Time.deltaTime * speedwRandom);
            float dist = Vector3.Distance(transform.localScale, Modifications.ScaleOriginal);
            if (dist < 0.01f)
            {
                State_IsNormal = true;
            }
            yield return new WaitForEndOfFrame();
        }

    }
}
