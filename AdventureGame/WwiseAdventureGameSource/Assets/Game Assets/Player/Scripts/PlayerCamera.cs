////////////////////////////////////////////////////////////////////////
//
// Copyright (c) 2018 Audiokinetic Inc. / All Rights Reserved
//
////////////////////////////////////////////////////////////////////////

﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public delegate void CameraEventStart();
public delegate void CameraEventEnd();

public class PlayerCamera : MonoBehaviour
{

    public static event CameraEventStart OnCameraEventStart;
    public static event CameraEventEnd OnCameraEventEnd;

    void CameraEventStarted() { }
    void CameraEventFinished() { }

    public enum CameraMode
    {
        normal, freezeRotation, CamEvent, lockPosition, lockCameraCompletely
    };

    [Header("Camera Objects")]
    [Tooltip("What is the camera to focus on? (Probably the Player)")]
    public GameObject target;
    [Tooltip("The skycam is the secondary camera located at the physical skybox area")]
    public GameObject skycam;

    [Header("Camera Settings")]
    [Tooltip("Change to offset the cameras position relative to the target")]
    public float xOffset = 0;
    public LayerMask collideWith;

    [Tooltip("Higher value is more responsive")]
    [Range(1.0f, 50f)]
    public float lerpSpeed = 8f;
    public float distance = 4;
    public float minY = -70f;
    public float maxY = 70f;
    public AnimationCurve yDisplacement;
    public float yDisplacementMultiplier = 2f;

    [Header("Camera in Combat Settings")]
    [Tooltip("If the player has attacked an enemy and runs away, how long distance before the camera returns to normal?")]
    public float focusDistanceThreshold = 5f;

    [Header("Mouse Control Settings")]
    [Range(0.0f, 10f)]
    public float mouseSensitivity = 2.5f;

    [Header("Camera Information")]
    public CameraMode cameraMode = CameraMode.normal;

    public AK.Wwise.State VoiceAttenuation_Off;
    public AK.Wwise.State VoiceAttenuation_On;

    #region private variables
    private Vector3 desiredPosition;
    private Vector3 shakeVector = Vector3.zero;
    private Quaternion desiredRotation;
    private float cameraDistance = 4;
    private float mouseX;
    private float mouseY;
    private float desiredDistance = 20;
    private float actualDistance = 20;
    private Vector3 targetToCamera;
    private float newDist;
    private bool colliding;
    private float yDisp;
    private bool doingCameraEvent;
    private SkyboxCamera skycamScript;
    private float origFOV;
    private AkListenerDistanceProbeChanger ListenerProbeChanger;
    private AkListenerDistanceProbe ListenerProbe;
    private AkGameObj thisAkGameObject;
    private AkGameObj probedefinedAkGameObject;

    private IEnumerator cameraEventRoutine;
    private IEnumerator cameraShakeRoutine;
    private IEnumerator cameraContinuousShakeRoutine;
    #endregion

    void OnEnable()
    {
        InputManager.OnUseDown += BreakCamera;
        OnCameraEventStart += CameraEventStarted;
        OnCameraEventEnd += CameraEventFinished;
        if (skycam == null)
        {
            skycam = GameObject.FindWithTag("Skycam");
        }
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        mouseY = maxY / 2;
        cameraDistance = distance;
        origFOV = Camera.main.fieldOfView;

        ListenerProbe = GetComponent<AkListenerDistanceProbe>();
        ListenerProbeChanger = GetComponent<AkListenerDistanceProbeChanger>();

        if (ListenerProbe != null && ListenerProbe.distanceProbe == null)
        { // If no distance probe assigned, then set the player. This is important for certification scenes where there might be a different Player prefab. 
            ListenerProbe.distanceProbe = PlayerManager.Instance.player.GetComponent<AkGameObj>();
        }

        if (ListenerProbe != null && ListenerProbeChanger != null && ListenerProbe.distanceProbe != null) // If distance probe and distance probe changer is there ...
        {
            probedefinedAkGameObject = ListenerProbe.distanceProbe;
            thisAkGameObject = GetComponent<AkGameObj>();
        }
        else {
            Debug.LogWarning(this+" could not find AkListenerDistanceProbe script or Distance Probe AkGameObject.");
        }
    }

    private void OnDestroy()
    {
        InputManager.OnUseDown -= BreakCamera;
    }

    void Start()
    {
        if (target == null) 
        {
            target = PlayerManager.Instance.playerHead;
        }
    }
     
    CameraMode prevCamMode;
    List<GameObject> cursorLockers = new List<GameObject>();
    /// <summary>
    /// Sets the visibility and movement freedom of the cursor.
    /// </summary>
    /// <param name="freezeAndShow">Freeze the rotation of the camera and show the cursor?</param>
    public void FreezeAndShowCursor(bool freezeAndShow, GameObject caller)
    {
        if (freezeAndShow)
        {
            if(cursorLockers.Count == 0)
            {
                prevCamMode = cameraMode;
                cameraMode = CameraMode.freezeRotation;
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }

            if (!cursorLockers.Contains(caller))
            {
                cursorLockers.Add(caller);
            }
        }
        else
        {
            if (cursorLockers.Contains(caller)){
                cursorLockers.Remove(caller);
            }

            if(cursorLockers.Count == 0)
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
                cameraMode = prevCamMode;
            }
        }
    }

#if UNITY_EDITOR
    void Update()
    {
        //reveal mouse and lock camera rotation on LSHIFT + P
        //This is mostly debug functionality, but is handy when you need cursor control during playmode in the editor
        if (Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.P))
        {
            if (cameraMode == CameraMode.freezeRotation)
            {
                FreezeAndShowCursor(false, gameObject);
            }
            else
            {
                FreezeAndShowCursor(true, gameObject);
            }
        }
    }
#endif

    RaycastHit hit;
    private float CollisionCheck()
    {
        if (target != null)
        {
            targetToCamera = transform.position - target.transform.position;

            if (Physics.Raycast(target.transform.position, targetToCamera.normalized, out hit, desiredDistance - yDisp, collideWith))
            {
                colliding = true;
                return desiredDistance - hit.distance + 0.5f;
            }
        }

        colliding = false;
        return 0;
    }

    void LateUpdate()
    {
        if (target != null)
        {
            switch (cameraMode)
            {
                /* NORMAL */
                case CameraMode.normal:
                    if (ListenerProbeChanger != null && probedefinedAkGameObject != null) {
                        ListenerProbeChanger.SetDistanceProbe(probedefinedAkGameObject);
                    }
                    
                    //mouse Y along sphere 
                    if (Time.timeScale > 0f) //if menu is not up i guess
                    { 
                        Vector2 input = InputManager.mouseVector;
                        float inputThreshold =
                            (InputManager.mode == InputManager.ControllerMode.mobile ||
                             InputManager.mode == InputManager.ControllerMode.pc) ? 0f : 0.3f; // 0 if mobile/pc, 0.3 if console joystick. TODO: Automatically detect what the idle input is. 

                        if (Mathf.Abs(input.y) > inputThreshold)
                        {
                            mouseY -= input.y * mouseSensitivity;
                            mouseY = Mathf.Clamp(mouseY, minY, maxY);
                        }

                        if (Mathf.Abs(input.x) > inputThreshold)
                        {
                            mouseX += input.x * mouseSensitivity;
                        }
                    }

                    desiredDistance = Mathf.Lerp(desiredDistance, distance, 2f);
                    actualDistance = Mathf.Clamp(desiredDistance, 0, desiredDistance - CollisionCheck());

                    if (!colliding)
                    {
                        yDisp = yDisplacement.Evaluate(1 - ((mouseY + maxY) / (maxY - minY))) * yDisplacementMultiplier;
                        actualDistance -= yDisp;
                    }

                    cameraDistance = Mathf.Lerp(cameraDistance, actualDistance, Time.deltaTime * lerpSpeed * 10);
                    desiredRotation = Quaternion.Euler(mouseY, mouseX, 0);
                    desiredPosition = desiredRotation * (new Vector3(xOffset, 0.0f, -cameraDistance)) + target.transform.position;

                    break;
                /* FREEZE ROTATION */
                case CameraMode.freezeRotation:
                    if (!doingCameraEvent)
                    {
                        desiredPosition = desiredRotation * (new Vector3(xOffset, 0.0f, -cameraDistance)) + target.transform.position;
                    }
                    break;
                /* LOCK POSITION */
                case CameraMode.lockPosition:
                    desiredRotation = Quaternion.LookRotation(target.transform.position - transform.position);
                    break;
                /* COMPLETE LOCK */
                case CameraMode.lockCameraCompletely:
                    //Do nothing
                    break;
                case CameraMode.CamEvent:
                    if (thisAkGameObject != null)
                    {
                        ListenerProbeChanger.SetDistanceProbe(thisAkGameObject);
                    }
                 
                    break;
            }
            Vector3 shake = transform.right * shakeVector.x + transform.up * shakeVector.y + transform.forward * shakeVector.z;
            transform.position = Vector3.Lerp(transform.position, desiredPosition, lerpSpeed * Time.deltaTime) + shake;
            transform.rotation = Quaternion.Lerp(transform.rotation, desiredRotation, lerpSpeed * Time.deltaTime);
        }


        //Send camera information to skycam
        if (skycam != null)
        {
            if (skycamScript != null)
            {
                skycamScript.SetRotation(transform.rotation);
                skycamScript.SetPosition(transform.position);
                skycamScript.SetFOV(Camera.main.fieldOfView);
            }
            else
            {
                skycamScript = skycam.GetComponent<SkyboxCamera>();
            }
        }

    }

    public void ChangeCamera(CameraEvent e)
    {
        if (cameraEventRoutine != null)
        {
            StopCoroutine(cameraEventRoutine);
        }
        cameraEventRoutine = CameraTransition(e);
        StartCoroutine(cameraEventRoutine);
    }

    public bool CameraIsActive()
    {
        if (timeLeft != 0f)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    void BreakCamera()
    {

        if (doingCameraEvent && !unSkippable && DialogueManager.Instance.Dialogue.Count > 0)
        {
            breakCameraEvent = true;
        }
    }

    float timeLeft = 0;
    bool breakCameraEvent = false;
    bool unSkippable = false;
    IEnumerator CameraTransition(CameraEvent e)
    {
        OnCameraEventStart();
        VoiceAttenuation_Off.SetValue();

        unSkippable = e.unSkippable;
        breakCameraEvent = false;
        doingCameraEvent = true;
        CameraMode oldMode = cameraMode;
        float oldFOV = Camera.main.fieldOfView;
        cameraMode = CameraMode.CamEvent;

        Vector3 startPos = transform.position;
        Quaternion startRotation = transform.rotation;

        timeLeft = e.transitionTime + e.holdTime; // +1 for backtransition

        //Camera transition
        for (float t = 0; t < 1 && !breakCameraEvent; t += Time.deltaTime / e.transitionTime)
        {
            float s = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f).Evaluate(t);

            transform.position = Vector3.Lerp(startPos, e.newPosition, s);
            desiredPosition = transform.position;

            transform.rotation = Quaternion.Lerp(startRotation, e.newRotation, s);
            desiredRotation = transform.rotation;

            Camera.main.fieldOfView = Mathf.Lerp(Camera.main.fieldOfView, e.newFOV, s);
            timeLeft -= Time.deltaTime / e.transitionTime;
            yield return null;
        }
        if (e.transitionTime == 0)
        {
            transform.position = e.newPosition;
            desiredPosition = e.newPosition;
            transform.rotation = e.newRotation;
            desiredRotation = e.newRotation;
            Camera.main.fieldOfView = e.newFOV;
        }

        //Camera hold
        if (e.holdTime != 0)
        {
            for (float t = 0; t < 1 && !breakCameraEvent; t += Time.deltaTime / e.holdTime)
            {
                //print("Hold time");
                float shake = Mathf.Sin(Time.realtimeSinceStartup) / 20f;

                transform.position = Vector3.Lerp(transform.position, e.newPosition + (Vector3.up * shake), Time.deltaTime);
                desiredPosition = transform.position;

                timeLeft -= Time.deltaTime / e.holdTime;
                yield return null;
            }
        }

        timeLeft = 0;
        doingCameraEvent = false; 

        OnCameraEventEnd();

        if (e.returnToOldCam)
        {
            cameraMode = oldMode;
            Camera.main.fieldOfView = origFOV;
        }
        doingCameraEvent = false;
        VoiceAttenuation_On.SetValue(); 
    }

    public void ResetCamera()
    {
        if (doingCameraEvent)
        {
            StopCoroutine("CameraTransition");
            VoiceAttenuation_On.SetValue();
            doingCameraEvent = false;
        }
        if (cameraMode != CameraMode.lockCameraCompletely)
        {
            cameraMode = CameraMode.normal;
            Camera.main.fieldOfView = origFOV;
        }
    }

    public class CameraEvent
    {
        public float transitionTime;
        public Vector3 newPosition;
        public Quaternion newRotation;
        public float newFOV;
        public bool returnToOldCam;
        public float holdTime;
        public bool disableMotor;
        public bool unSkippable;

        public CameraEvent(Camera newCam, float transitionTimeInSeconds, float holdTimeInSeconds, bool returnToOld)
        {
            transitionTime = transitionTimeInSeconds;
            newPosition = newCam.gameObject.transform.position;
            newRotation = newCam.gameObject.transform.rotation;
            newFOV = newCam.fieldOfView;
            holdTime = holdTimeInSeconds;
            returnToOldCam = returnToOld;
            disableMotor = true;
        }
    }

    public void SetCameraInstantly(Vector3 position, Quaternion rotation, float fieldOfViewSet)
    {
        if (doingCameraEvent)
        {
            StopCoroutine("CameraTransition");
            doingCameraEvent = false;
        }
        transform.position = position;
        desiredPosition = position;
        transform.rotation = rotation;
        desiredRotation = rotation;
        Camera.main.fieldOfView = fieldOfViewSet;
    }

    public struct CameraShake
    {
        public float strength;
        public float duration;

        public CameraShake(float str, float dur)
        {
            strength = str;
            duration = dur;
        }
    }


    public void CamShake(CameraShake shake)
    {
        if (cameraShakeRoutine != null)
        {
            StopCoroutine(cameraShakeRoutine);
        }
        cameraShakeRoutine = ShakeCamera(shake);
        StartCoroutine(cameraShakeRoutine);
    }

    IEnumerator ShakeCamera(CameraShake c)
    {
        for(float i = 1f; i > 0f; i -= Time.unscaledDeltaTime / c.duration + 0.01f)
        {
            shakeVector = Utility.GetRandomVector(c.strength * i);
            yield return new WaitForSecondsRealtime(0.01f);
        }

        //for (float i = c.duration; i > 0; i -= Time.unscaledDeltaTime)
       // {
       //     Vector3 randomVector = Utility.GetRandomVector(c.strength);
		//	shakeVector = randomVector * (i / c.duration);
       //     yield return new WaitForSecondsRealtime(0.01f);
       // }
        shakeVector = Vector3.zero;
    }

    public void StartShake(float strength)
    {
        if (cameraContinuousShakeRoutine != null)
        {
            StopCoroutine(cameraContinuousShakeRoutine);
        }
        cameraContinuousShakeRoutine = ShakeContinuous(strength);
        StartCoroutine(cameraContinuousShakeRoutine);
    }

    IEnumerator ShakeContinuous(float strength)
    {
        while (true)
        {
            shakeVector = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), Random.Range(-1f, 1f)) * strength;
            yield return null;
        }
    }

    public void StopShake()
    {
        if (cameraContinuousShakeRoutine != null)
        {
            StopCoroutine(cameraContinuousShakeRoutine);
        }
    }

    public void SetMouseSensitivity(UnityEngine.UI.Slider slider)
    {
        float sensitivity = slider.value;
        if (sensitivity > 0 && sensitivity < 10)
        {
            mouseSensitivity = sensitivity;
        }
    }
}

