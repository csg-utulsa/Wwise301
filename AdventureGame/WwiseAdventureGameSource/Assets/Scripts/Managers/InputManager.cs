////////////////////////////////////////////////////////////////////////
//
// Copyright (c) 2018 Audiokinetic Inc. / All Rights Reserved
//
////////////////////////////////////////////////////////////////////////

﻿using UnityEngine;
using System.Collections.Generic;
using UnityEngine.EventSystems;

public delegate void InputEvent();
public delegate void PlayerMovementEvent(Vector2 dir);
public delegate void ControlChange();

public class InputManager : MonoBehaviour
{

    [System.Serializable]
    public enum ControllerMode
    {
        pc, ps3, ps4, xbone, xbox, mobile
    };

    public static ControllerMode mode = ControllerMode.pc;

    public bool EnableMicrophoneAttack = false;

    [SerializeField, Header("You can also change mode from the context menu.")]
    private ControllerMode controllerMode;

    void OnValidate()
    {
        if(mode != controllerMode)
        {
            ChangeController(controllerMode);
        }
    }

    public static void ChangeController(ControllerMode controllerMode)
    {
        mode = controllerMode;
        inputThreshold = mode == ControllerMode.mobile ? 0f : 0.25f; //This is to eliminate joysticks that have inaccurate/worn down analogue sticks. TODO: Automatically measure the idle readings and base the threshold off of that.

        modeAsString = mode.ToString();

        horizontalString = "Horizontal_" + modeAsString;
        verticalString = "Vertical_" + modeAsString;
        inventoryString = "Inventory_" + modeAsString;
        inventorySelectString = "InventorySelect_" + modeAsString;
        useString = "Use_" + modeAsString;
        actionString = "Action_" + modeAsString;
        menuString = "Menu_" + modeAsString;
        mouseXString = "MouseX_" + modeAsString;
        mouseYString = "MouseY_" + modeAsString;
        sprintString = "Sprint_" + modeAsString;

        StandaloneInputModule currentModule = GameObject.Find("EventSystem").GetComponent<StandaloneInputModule>();
        currentModule.horizontalAxis = horizontalString;
        currentModule.verticalAxis = verticalString;

        if (OnControlChange != null)
        {
            OnControlChange();
        }
    }

    public static Sprite GetControlImage(string ctrl)
    {
        Platform p = platformControls.Find(ptf => ptf.name == mode);
        Control c = p.controls.Find(ctr => ctr.control == ctrl);

        if (c != null)
        {
            return c.image;
        }
        else
        {
            return null;
        }
    }

    public static string GetControlString(string ctrl)
    {
        Platform p = platformControls.Find(ptf => ptf.name == mode);
        Control c = p.controls.Find(ctr => ctr.control == ctrl);

        if (c != null)
        {
            return c.value;
        }
        else
        {
            return null;
        }

    }

    public static string ReplaceControllerStrings(string sequence)
    {
        string[] splitStringFronts = sequence.Split(new char[] { '[', ']' }, System.StringSplitOptions.RemoveEmptyEntries);
        string EndString = "";

        for (int i = 0; i < splitStringFronts.Length; i++)
        {
            string newString = splitStringFronts[i];
            if (i % 2 == 1)
            {
                newString = GetControlString(splitStringFronts[i]);
            }
            else
            {
                newString = splitStringFronts[i];
            }
            EndString = EndString + newString;
        }

        return EndString;
    }

    //Input events
    public static event InputEvent OnMoveDown;
    public static event PlayerMovementEvent OnMoveHold;
    public static event InputEvent OnMoveUp;
    public static event InputEvent OnActionDown; //Left click, etc.
    public static event InputEvent OnActionHold;
    public static event InputEvent OnPostAction;
    public static event InputEvent OnUseDown;
    public static event InputEvent OnUseHold;
    public static event InputEvent OnUseUp;
    public static event InputEvent OnSprintDown;
    public static event InputEvent OnSprint;
    public static event InputEvent OnSprintUp;
    public static event InputEvent OnMenuDown;
    public static event InputEvent OnInventoryDown;
    public static event InputEvent OnInventoryUp;

    public static event InputEvent OnRightArrowDown;
    public static event InputEvent OnLeftArrowDown;

    //Other events
    public static event ControlChange OnControlChange;

    public static Vector2 inputVector;
    public static Vector2 mouseVector = new Vector2(0f, 35f);

    #region private variables
    private bool sentMove;
    private bool sendPostMovement;
    private bool sentAction;
    private bool sendPostAction;
    private bool sentUse;
    private bool sendPostUse;
    private bool sentTab;
    private bool sentSprint;
    private bool sendPostSprint;
    private bool sentJump;
    private bool sendPostJump;
    private bool sentMenu;
    private bool sentInventory;
    private bool sendPostInventory;
    private bool sentInventorySelect;

    private static float inputThreshold;
    private static string modeAsString;
    #endregion

    public bool controllerConnected = false;

    [System.Serializable]
    public class Control
    {
        public string control;
        public string value;
        public Sprite image;
    }

    [System.Serializable]
    public class Platform
    {
        public ControllerMode name;
        public List<Control> controls;
    }

    [Header("Control Definitions")]
    public List<Platform> platforms;
    public static List<Platform> platformControls;

    #region private variables
    private MobileControls mobileControls;
    private bool isMobile = false;

    private static string horizontalString;
    private static string verticalString;
    private static string inventoryString;
    private static string inventorySelectString;
    private static string useString;
    private static string actionString;
    private static string menuString;
    private static string sprintString;
    private static string mouseXString;
    private static string mouseYString;
    #endregion

    void Awake()
    {
        platformControls = platforms;

        mobileControls = FindObjectOfType<MobileControls>();
        if (IsMobile())
        {
            isMobile = true;
            MobileEvents.OnMobileUse += OnMobileUse;
            MobileEvents.OnMobileUseUp += OnMobileUseUp;
            MobileEvents.OnMobileSprintDown += OnMobileSprint;
            MobileEvents.OnMobileSprintUp += OnMobileSprintUp;
            MobileEvents.OnMobileMenu += OnMobileMenu;
        }
        else
        {
            CheckForControllers();
            ChangeController(mode);
        }
    }

    void Start()
    {
        //Fire OnControlChange in order to update all temporary scene images
        if (OnControlChange != null)
        {
            OnControlChange();
        }
    }

    void Update()
    {
        CheckForInputs();
    }

    void CheckForInputs()
    {
        // MOVEMENT
        if (isMobile)
        {
            inputVector = mobileControls.virtualJoystick.GetInputVector();
        }
        else
        {
            inputVector = new Vector2(Input.GetAxis(horizontalString), Input.GetAxis(verticalString));
        }
        if (inputVector.magnitude > 1f)
        {
            inputVector.Normalize();
        }

        if (inputVector.magnitude > inputThreshold)
        {
            sendPostMovement = true;
            if (!sentMove)
            {
                if (OnMoveDown != null)
                {
                    OnMoveDown();
                }
                sentMove = true;
            }
            else
            {
                if (OnMoveHold != null)
                {
                    OnMoveHold(inputVector);
                }
                PlayerManager.Instance.isMoving = true;
            }
        }
        else if (sendPostMovement)
        {
            if (OnMoveHold != null)
            {
                OnMoveHold(inputVector);
            }
            if (OnMoveUp != null)
            {
                OnMoveUp();
            }
            sendPostMovement = false;
            PlayerManager.Instance.isMoving = false;
        }
        else
        {
            sentMove = false;
        }

        // ARROW KEYS
        float arrows;
        if (isMobile)
        {
            arrows = 0f;
        }
        else
        {
            arrows = Input.GetAxis(inventorySelectString);
        }

        if (arrows != 0)
        {
            if (!sentInventorySelect)
            {
                if (arrows > 0)
                {//right
                    if (OnRightArrowDown != null)
                    {
                        OnRightArrowDown();
                    }
                    sentInventorySelect = true;
                }
                else
                { //left
                    if (OnLeftArrowDown != null)
                    {
                        OnLeftArrowDown();
                    }
                    sentInventorySelect = true;
                }
            }
        }
        else
        {
            sentInventorySelect = false;
        }


        // ACTION (attack)
        float action;
        if (isMobile)
        {
            action = mobileControls.actionButton.GetButtonInput();
        }
        else
        {
            action = Input.GetAxis(actionString);
        }

        if (action > 0 || ShouldTriggerActionWithMicrophone())
        {
            sendPostAction = true;
            if (!sentAction)
            {
                if (OnActionDown != null)
                {
                    OnActionDown();
                }
                sentAction = true;
            }
            else
            {
                if (OnActionHold != null)
                {
                    OnActionHold();
                }
            }
        }
        else if (sendPostAction)
        {
            if (OnPostAction != null)
            {
                OnPostAction();
            }
            sendPostAction = false;
        }
        else
        {
            sentAction = false;
        }

        // USE (interact)
        float use;
        if (isMobile)
        {
            use = 0f;
        }
        else
        {
            use = Input.GetAxis(useString);
        }

        if (use > 0)
        {
            sendPostUse = true;
            if (!sentUse)
            {
                if (OnUseDown != null)
                {
                    OnUseDown();
                }
                sentUse = true;
            }
            else
            {
                if (OnUseHold != null)
                {
                    OnUseHold();
                }
            }
        }
        else if (sendPostUse)
        {
            if (OnUseUp != null)
            {
                OnUseUp();
            }
            sendPostUse = false;
        }
        else
        {
            sentUse = false;
        }

        // SPRINT
        float sprint;
        if (isMobile)
        {
            sprint = 0f;
        }
        else
        {
            sprint = Input.GetAxis(sprintString);
        }

        if (sprint > 0)
        {
            sendPostSprint = true;
            if (!sentSprint)
            {
                if (OnSprintDown != null)
                {
                    OnSprintDown();
                }
                sentSprint = true;
            }
            else
            {
                if (OnSprint != null)
                {
                    OnSprint();
                }
            }
        }
        else if (sendPostSprint)
        {
            if (OnSprintUp != null)
            {
                OnSprintUp();
            }
            sendPostSprint = false;
        }
        else
        {
            sentSprint = false;
        }

        // MENU
        float menu;
        if (isMobile)
        {
            // TODO: Get Menu from mobile
            menu = 0f;
        }
        else
        {
            menu = Input.GetAxis(menuString);
        }

        if (menu > 0)
        {
            if (!sentMenu)
            {
                if (OnMenuDown != null)
                {
                    OnMenuDown();
                }
                sentMenu = true;
            }
        }
        else
        {
            sentMenu = false;
        }

        // INVENTORY
        float inventory;
        if (isMobile)
        {
            inventory = 0;
        }
        else
        {
            inventory = Input.GetAxis(inventoryString);
        }

        if (inventory > 0)
        {
            sendPostInventory = true;
            if (!sentInventory)
            {
                if (OnInventoryDown != null)
                {
                    OnInventoryDown();
                }
                sentInventory = true;
            }
        }
        else if (sendPostInventory)
        {
            if (OnInventoryUp != null)
            {
                OnInventoryUp();
            }
            sendPostInventory = false;
        }
        else
        {
            sentInventory = false;
        }

        // MOUSE MOVEMENT
        float mouseX, mouseY;
        if (isMobile)
        {
            Vector2 mouse = mobileControls.cameraMovement.GetMouseMovement();
            mouseX = mouse.x;
            mouseY = mouse.y;
        }
        else
        {
            mouseX = Input.GetAxis(mouseXString);
            mouseY = Input.GetAxis(mouseYString);
        }

        mouseVector = new Vector2(mouseX, mouseY);
    }

    bool ShouldTriggerActionWithMicrophone()
    {
#if UNITY_WEBGL
        return false;
#else
        return EnableMicrophoneAttack && AkMicrophone.Instance != null && AkMicrophone.Instance.IsAboveThreshold;
#endif
    }

    bool IsMobile()
    {
#if UNITY_IOS || UNITY_ANDROID
        mode = ControllerMode.mobile;
        return true;
#else
        return false;
#endif
    }

    void OnMobileUse()
    {
        if (OnUseDown != null)
        {
            OnUseDown();
        }
    }

    void OnMobileUseUp()
    {
        if (OnUseUp != null)
        {
            OnUseUp();
        }
    }

    void OnMobileSprint()
    {
        if (OnSprint != null)
        {
            OnSprint();
        }
    }

    void OnMobileSprintUp()
    {
        if (OnSprintUp != null)
        {
            OnSprintUp();
        }
    }

    void OnMobileMenu(){
        if(OnMenuDown != null){
            OnMenuDown();
        }
    }

    void CheckForControllers()
    {
        controllerConnected = false;
        string[] Controllers = Input.GetJoystickNames();

        if (Controllers != null && Controllers.Length > 0)
        {
            for (int c = 0; c < Controllers.Length; c++)
            {
                if (Controllers[c] != "")
                {
                    print("InputManager: " + Controllers[c] + " registered.");
                    controllerConnected = true;
                }
            }
        }
    }

#region ConextMenu (inspector) functions
    [ContextMenu("Use Mouse and Keyboard")]
    void pcControls()
    {
        controllerMode = mode = ControllerMode.pc;
    }

    [ContextMenu("Use PS3 Controller")]
    void Ps3Controller()
    {
        controllerMode = mode = ControllerMode.ps3;
    }

    [ContextMenu("Use PS4 Controller")]
    void Ps4Controller()
    {
        controllerMode = mode = ControllerMode.ps4;
    }

    [ContextMenu("Use XBOX 360 Controller")]
    void Xbox360Controller()
    {
        controllerMode = mode = ControllerMode.xbox;
    }

    [ContextMenu("Use XBOX One Controller")]
    void XboneController()
    {
        controllerMode = mode = ControllerMode.xbone;
    }
#endregion
}
