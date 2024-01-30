////////////////////////////////////////////////////////////////////////
//
// Copyright (c) 2018 Audiokinetic Inc. / All Rights Reserved
//
////////////////////////////////////////////////////////////////////////

﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Inventory : MonoBehaviour
{
    public Canvas ThisCanvas;
    public CanvasGroup canvasGroup;

    [Header("Visualization")]
    public Vector3 rotationSpeed;
    public bool RepeatObject2Fill = false;
    public bool ShowRow1 = false;
    public bool ShowRow3 = false;
    public GameObject UIhelp;

    [Header("Layers")]
    public GameObject GRow1;
    public GameObject GRow2;
    public GameObject GRow3;

    [Header("Layer Container")]
    public GameObject Panel;

    [Header("B Icon")]
    public GameObject StatusObject;

    [System.Serializable]
    public class PositionContainers
    {
        public List<GameObject> Row1;
        public List<GameObject> Row2;
        public List<GameObject> Row3;
    }
    public PositionContainers OriginalPositions;

    public bool TransparentNonPickuped = false;
    public bool NonSelectedGrayout = false;
    public Material NotSelectedMaterial;
    public Material NotPickedUpMaterial;
    public GameObject SelectedGameobject;

    public bool debugMode = false;
    [ShowIf("debugMode", true)]
    public PlayerManager.InventoryObjects InventoryReference;

    public static bool InventoryIsOut = false;

    [Header("Wwise")]
    public AK.Wwise.Event InventoryOpenedSound;
    public AK.Wwise.Event InventoryClosedSound;
    public AK.Wwise.Event InventorySelectSound;

    #region private variables
    private bool hasShown = false;
    private int SelectIncrementor_Row1 = 0;
    private int SelectIncrementor_Row2 = 0;
    private int SelectIncrementor_Row3 = 0;
    private List<float> Scales;
    private List<GameObject> WeaponIcon;
    private List<GameObject> ItemIcon;
    private List<float> ScaleSizes;
    private float speed = 5f;
    private int SelectedRow = 1;
    private int RowAmount = 2;
    private int RowShift = 0;
    private Image MarkerImage_Row1;
    private Image MarkerImage_Row2;
    private Image MarkerImage_Row3;
    #endregion

    private void OnDestroy()
    {
        InputManager.OnInventoryDown -= OpenInventory;
        InputManager.OnInventoryUp -= CloseInventory;
    }

    void OnEnable()
    {
        canvasGroup.interactable = false;

        ScaleSizes = new List<float>();
        float scaleMultiplier = 0.1f;
        ScaleSizes.Add(0.2f * scaleMultiplier); // L2
        ScaleSizes.Add(0.7f * scaleMultiplier); // L
        ScaleSizes.Add(1.0f * scaleMultiplier); // C
        ScaleSizes.Add(0.7f * scaleMultiplier); // R
        ScaleSizes.Add(0.2f * scaleMultiplier); // R2

        WeaponIcon = new List<GameObject>();
        ItemIcon = new List<GameObject>();

        Scales = new List<float>();
        float scaleMultiplierMesh = 10f;
        Scales.Add(100f * scaleMultiplierMesh);
        Scales.Add(70f * scaleMultiplierMesh);

        InputManager.OnInventoryDown += OpenInventory;
        InputManager.OnInventoryUp += CloseInventory;
        Panel.SetActive(false);

        OriginalPositions.Row1 = Utility.GetAllChildren(GRow1.transform);
        OriginalPositions.Row2 = Utility.GetAllChildren(GRow2.transform);
        OriginalPositions.Row3 = Utility.GetAllChildren(GRow3.transform);

        MarkerImage_Row1 = OriginalPositions.Row1[2].transform.Find("Marker").GetComponent<Image>();
        MarkerImage_Row2 = OriginalPositions.Row2[2].transform.Find("Marker").GetComponent<Image>();
        MarkerImage_Row3 = OriginalPositions.Row3[2].transform.Find("Marker").GetComponent<Image>();

        for (int i = 0; i < 3; i++) { CanPressLefts.Add(false); }
        for (int i = 0; i < 3; i++) { CanPressRights.Add(false); }
    }

    void MoveInventoryObjects(List<GameObject> Positions, List<GameObject> invRef, int incre, List<GameObject> StoreSpawns)
    {
        if (invRef.Count > 4)
        {
            OrderCheck(5, StoreSpawns, Positions, false, incre);
        }
        else if (invRef.Count > 2)
        {
            OrderCheck(3, StoreSpawns, Positions, false, incre);
        }
        else if (invRef.Count == 2)
        {
            OrderCheck(2, StoreSpawns, Positions, true, incre);
        }
        else if (invRef.Count == 1)
        {
            OrderCheck(1, StoreSpawns, Positions, false, incre);
        }
    }

    void OrderCheck(int size, List<GameObject> ItemIcons, List<GameObject> OriginalPos, bool shift, int incre)
    {
        Vector2 WheelDimensions = CheckModuleSize(size, OriginalPos);
        int checkFrom;
        int checkTo;

        if (shift)
        {
            checkFrom = (int)WheelDimensions.x - (incre % 2);
            checkTo = (int)WheelDimensions.y - (incre % 2);

        }
        else
        {
            checkFrom = (int)WheelDimensions.x;
            checkTo = (int)WheelDimensions.y;
        }

        for (int a = checkFrom; ItemIcons != null && a < checkTo; a++)
        {
            ObjectLerpTransform(ItemIcons, OriginalPos, a, a - checkFrom, speed, ScaleSizes[a]);
        }
    }

    void ObjectLerpTransform(List<GameObject> ListIcons, List<GameObject> OriginalPos, int posNum, int itemNum, float speed, float scale)
    {
        // POSITION
        ListIcons[itemNum].transform.position = Vector3.Lerp(ListIcons[itemNum].transform.position, OriginalPos[posNum].transform.GetChild(0).position, Time.unscaledDeltaTime * speed);
        // ROTATE
        ListIcons[itemNum].transform.Rotate(rotationSpeed * 100 * Time.unscaledDeltaTime);
        // SCALE
        if (ListIcons[itemNum].GetComponent<ShowInInventory>() != null)
        {
            float S = ListIcons[itemNum].GetComponent<ShowInInventory>().SizeMultiplier;
            ListIcons[itemNum].transform.localScale = Vector3.Lerp(ListIcons[itemNum].transform.localScale, new Vector3(scale * S, scale * S, scale * S), Time.unscaledDeltaTime * speed);
        }
        else
        {
            ListIcons[itemNum].transform.localScale = Vector3.Lerp(ListIcons[itemNum].transform.localScale, new Vector3(scale, scale, scale), Time.unscaledDeltaTime * speed);
        }

        // SCALE CHILDREN
        Transform[] childs = ListIcons[itemNum].GetComponentsInChildren<Transform>();
        for (int p = 0; p < childs.Length; p++)
        {
            if (childs[p].GetComponent<ParticleSystem>() != null)
            {
                childs[p].localScale = new Vector3(scale * 100f, scale * 100f, scale * 100f);
            }
        }
    }

    void GenerateInventory(List<GameObject> Positions, List<GameObject> invRef, int incre, float scaleMethod, List<GameObject> StoreSpawns, int row)
    {
        ButtonVisibility(Positions, invRef, incre, row);
        if (invRef.Count > 4)
        {
            WheelModule(Positions, invRef, incre, scaleMethod, StoreSpawns, 5);
        }
        else if (invRef.Count > 2)
        {
            WheelModule(Positions, invRef, incre, scaleMethod, StoreSpawns, 3);
        }
        else if (invRef.Count == 2)
        {
            WheelModule(Positions, invRef, incre, scaleMethod, StoreSpawns, 2);
        }
        else if (invRef.Count == 1)
        {
            WheelModule(Positions, invRef, incre, scaleMethod, StoreSpawns, 1);
        }
    }

    void WheelModule(List<GameObject> Positions, List<GameObject> invRef, int incre, float scaleMethod, List<GameObject> StoreSpawns, int wheelSize)
    {
        Vector2 WheelDimensions = CheckModuleSize(wheelSize, Positions);

        for (int i = (int)WheelDimensions.x; i < (int)WheelDimensions.y && invRef.Count > 0; i++)
        {
            int c = 0;
            if (i > 1)
            {
                // i - 1 + increase 
                int modInc = incre % invRef.Count;
                c = (i - 2 + modInc) % invRef.Count;
            }
            else
            {
                // count - 2 + i + increase, modulated with count
                int modInc = incre % invRef.Count;
                c = (invRef.Count - 2 + i + modInc) % invRef.Count;
            }
            GenerateObject(Positions, invRef, c, i, StoreSpawns, scaleMethod, false);
        }
        EnableCenterOutline(WheelDimensions, invRef, StoreSpawns);
    }

    void EnableCenterOutline(Vector2 WheelDimensions, List<GameObject> invRef, List<GameObject> StoreSpawns)
    {

        for (int i = (int)WheelDimensions.x; i < (int)WheelDimensions.y && invRef.Count > 0; i++)
        {
            if ((i - (int)WheelDimensions.x) < StoreSpawns.Count)
            {
                if (i == 2)
                {
                    Transform outline = StoreSpawns[i - (int)WheelDimensions.x].transform.Find("Outline");
                    if (outline != null)
                    {
                        outline.gameObject.SetActive(true);
                    }
                }
                else
                {
                    Transform outline = StoreSpawns[i - (int)WheelDimensions.x].transform.Find("Outline");
                    if (outline != null)
                    {
                        outline.gameObject.SetActive(false);
                    }
                }
            }
        }
    }

    void GenerateObject(List<GameObject> Positions, List<GameObject> invRef, int count, int pos, List<GameObject> StoreSpawns, float scaleMethod, bool insert)
    {
        if (invRef[count].GetComponent<ShowInInventory>().showFullGameobject)
        {
            GameObject GI = (Instantiate(invRef[count])) as GameObject;
            if (insert)
            {
                StoreSpawns.Insert(0, GI);
            }
            else
            {
                StoreSpawns.Add(GI);
            }

            GI.name = "Icon: " + invRef[count].name;
            if (pos > 0)
            {
                GI.transform.position = Positions[(pos - 1) % Positions.Count].transform.GetChild(0).position;
            }
            else
            {
                GI.transform.position = Positions[(pos) % Positions.Count].transform.GetChild(0).position;
            }
            GI.transform.localScale = Vector3.zero;
            GI.transform.parent = Positions[(pos) % Positions.Count].transform.GetChild(0);
            GI.layer = LayerMask.NameToLayer("UI");
            for (int k = 0; k < GI.transform.childCount; k++)
            {
                GI.transform.GetChild(k).gameObject.layer = LayerMask.NameToLayer("UI");
            }

            CheckForAmountItem(Positions, invRef, count, pos);

            GI.gameObject.SetActive(true);
        }
        else
        {
            GameObject GI = new GameObject("Icon: " + invRef[count].name);
            GameObject outline = new GameObject();
            outline.transform.parent = GI.transform;
            outline.name = "Outline";

            if (insert)
            {
                StoreSpawns.Insert(0, GI);
            }
            else
            {
                StoreSpawns.Add(GI);
            }

            if (pos > 0)
            {
                GI.transform.position = Positions[(pos - 1) % Positions.Count].transform.GetChild(0).position;
            }
            else
            {
                GI.transform.position = Positions[(pos) % Positions.Count].transform.GetChild(0).position;
            }
            GI.transform.localScale = Vector3.zero;
            GI.transform.parent = Positions[(pos) % Positions.Count].transform.GetChild(0);

            if (CopyMeshes(GI, invRef, count, outline))
            {
                Utility.RecalculateMesh(outline.GetComponent<MeshFilter>().mesh, scaleMethod);
                outline.gameObject.SetActive(false);
            }
            Utility.RecalculateMesh(GI.GetComponent<MeshFilter>().mesh, scaleMethod);

            CheckForAmountItem(Positions, invRef, count, pos);

            Utility.CopyComponent(invRef[count].GetComponent<ShowInInventory>(), GI);
        }
    }

    void ApplyInventoryInfo2Single_Right(List<GameObject> Positions, List<GameObject> invRef, int incre, float scaleMethod, List<GameObject> StoreSpawns) //TODO: This seems like a lot of copy pasted code. Rework into a single method?
    {
        if (invRef.Count > 2)
        {
            Utility.DestroyFirstListElement(StoreSpawns);
        }

        ButtonVisibility(Positions, invRef, incre, SelectedRow);

        int modInc = incre % invRef.Count;
        int AtWhere = 4;
        int c = 0;
        if (invRef.Count > 2)
        {
            if (invRef.Count > 4)
            {
                AtWhere = 4;
                c = (AtWhere - 2 + modInc) % invRef.Count;
            }
            else if (invRef.Count > 2)
            {
                AtWhere = 3;
                c = (AtWhere - 2 + modInc) % invRef.Count;
            }

            GenerateObject(Positions, invRef, c, AtWhere, StoreSpawns, scaleMethod, false);
        }

        // Update Amount items
        int wheelSize = CheckForAmountItemAll(Positions, invRef, c, modInc);

        Vector2 WheelDimensions = CheckModuleSize(wheelSize, Positions);
        EnableCenterOutline(WheelDimensions, invRef, StoreSpawns);
    }

    void ApplyInventoryInfo2Single_Left(List<GameObject> Positions, List<GameObject> invRef, int incre, float scaleMethod, List<GameObject> StoreSpawns)
    {
        if (invRef.Count > 2)
        {
            Utility.DestroyLastlistElement(StoreSpawns);
        }

        ButtonVisibility(Positions, invRef, incre, SelectedRow);

        int modInc = incre % invRef.Count;
        int AtWhere = 0;
        int c = 0;
        if (invRef.Count > 2)
        {
            if (invRef.Count > 4)
            {
                AtWhere = 0;
                c = (invRef.Count - 2 + modInc) % invRef.Count;
            }
            else if (invRef.Count > 2)
            {
                AtWhere = 1;
                c = (invRef.Count - 1 + modInc) % invRef.Count;
            }

            GenerateObject(Positions, invRef, c, AtWhere, StoreSpawns, scaleMethod, true);
        }

        // Update Amount items
        int wheelSize = CheckForAmountItemAll(Positions, invRef, c, modInc);

        Vector2 WheelDimensions = CheckModuleSize(wheelSize, Positions);
        EnableCenterOutline(WheelDimensions, invRef, StoreSpawns);
    }

    int CheckForAmountItemAll(List<GameObject> Positions, List<GameObject> invRef, int c, int modInc)
    {
        // 3 MODULE
        if (invRef.Count < 5 && invRef.Count > 2)
        {
            for (int i = 1; i < Positions.Count - 1; i++)
            {
                int inventoryNum = (i + 1 + modInc) % invRef.Count;
                if (invRef.Count == 4)
                {
                    inventoryNum = (i + 2 + modInc) % invRef.Count;
                }
                CheckForAmountItem(Positions, invRef, inventoryNum, i);
            }
            return 3;
        }
        // 5 MODULE
        else if (invRef.Count > 4)
        {
            for (int i = 0; i < Positions.Count; i++)
            {
                int adding = invRef.Count - 4;
                int inventoryNum = (i + 2 + adding + modInc) % invRef.Count;
                CheckForAmountItem(Positions, invRef, inventoryNum, i);
            }
            return 5;
        }
        // 2 MODULE
        else
        {
            if (modInc % 2 == 1)
            {
                CheckForAmountItem(Positions, invRef, 0, 1);
                CheckForAmountItem(Positions, invRef, 1, 2);
                Positions[3].GetComponentInChildren<Text>().text = "";
                return 3;
            }
            else
            {
                Positions[1].GetComponentInChildren<Text>().text = "";
                CheckForAmountItem(Positions, invRef, 0, 2);
                CheckForAmountItem(Positions, invRef, 1, 3);
                return 2;
            }

        }
    }

    void CheckForAmountItem(List<GameObject> Positions, List<GameObject> invRef, int c, int pos)
    {
        if (invRef[c].GetComponent<ShowInInventory>() != null && invRef[c].GetComponent<ShowInInventory>().isAmountItem)
        {
            Positions[(pos) % Positions.Count].GetComponentInChildren<Text>().text = invRef[c].GetComponent<ShowInInventory>().currentAmount.ToString();
        }
        else
        {
            Positions[(pos) % Positions.Count].GetComponentInChildren<Text>().text = "";
        }
    }

    /// <summary>
    /// returns false if theres no outline childs.
    /// </summary>
    bool CopyMeshes(GameObject AddTo, List<GameObject> TakeFrom, int c, GameObject outline)
    {
        AddTo.AddComponent<MeshFilter>();
        if (TakeFrom[c].GetComponent<MeshFilter>() != null)
        {
            AddTo.GetComponent<MeshFilter>().mesh = TakeFrom[c].GetComponent<MeshFilter>().mesh;
        }
        else
        {
            AddTo.GetComponent<MeshFilter>().mesh = TakeFrom[c].transform.Find("Graphics").GetComponent<MeshFilter>().mesh;
        }


        if (c != 0 && NonSelectedGrayout)
        {
            if (!TakeFrom[c].GetComponent<ShowInInventory>().PickedUp && TransparentNonPickuped)
            {
                // NOT EQUIPED 
                AddTo.AddComponent<MeshRenderer>();
                Material[] newMat = new Material[TakeFrom[c].GetComponent<MeshRenderer>().materials.Length];
                for (int m = 0; m < newMat.Length; m++)
                {
                    newMat[m] = NotPickedUpMaterial;
                }
                AddTo.GetComponent<MeshRenderer>().materials = newMat;
            }
            else
            {
                // NOT PICKED UP
                AddTo.AddComponent<MeshRenderer>();
                Material[] newMat = new Material[TakeFrom[c].GetComponent<MeshRenderer>().materials.Length];
                for (int m = 0; m < newMat.Length; m++)
                {
                    newMat[m] = NotSelectedMaterial;
                    //newMat[m] = NotPickedUpMaterial;
                }
                AddTo.GetComponent<MeshRenderer>().materials = newMat;
            }
        }
        else
        {
            // EQUIPED
            AddTo.AddComponent<MeshRenderer>();
            if (TakeFrom[c].GetComponent<MeshRenderer>() != null)
            {
                AddTo.GetComponent<MeshRenderer>().materials = TakeFrom[c].GetComponent<MeshRenderer>().materials;
            }
            else
            {
                AddTo.GetComponent<MeshRenderer>().materials = TakeFrom[c].transform.Find("Graphics").GetComponent<MeshRenderer>().materials;
            }
        }
        AddTo.layer = LayerMask.NameToLayer("UI");
        AddTo.GetComponent<MeshRenderer>().receiveShadows = false;
        AddTo.GetComponent<MeshRenderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;

        Transform outlineChild = TakeFrom[c].transform.Find("Outline");
        MeshRenderer MR = outlineChild.GetComponent<MeshRenderer>();
        MeshFilter MF = outlineChild.GetComponent<MeshFilter>();

        if (outlineChild != null && MR != null && MF != null)
        {
            outline.AddComponent<MeshFilter>();
            outline.GetComponent<MeshFilter>().mesh = outlineChild.GetComponent<MeshFilter>().mesh;
            outline.AddComponent<MeshRenderer>();
            outline.GetComponent<MeshRenderer>().materials = outlineChild.GetComponent<MeshRenderer>().materials;
            outline.layer = LayerMask.NameToLayer("UI");
            outline.transform.localScale = outlineChild.localScale;
            return true;
        }
        else
        {
            Debug.Log("Missing Outline or Mesh on: " + AddTo.name);
            return false;
        }
    }

    PlayerManager.InventoryObjects GetAllShowInventoryGameobjects(PlayerManager.InventoryObjects IO)
    {
        for (int i = IO.Items.Count - 1; i > -1; i--)
        {
            if(IO.Items[i] == null || (IO.Items[i] != null && !IO.Items[i].GetComponent<ShowInInventory>().VisibleInInventory)){
                IO.Items.RemoveAt(i);
            }
        }
        return IO;
    }

    void EnableInventory()
    {
        if (!hasShown)
        {
            PlayerManager.Instance.cameraScript.FreezeAndShowCursor(true, gameObject);

            hasShown = true;
            StatusObject.GetComponent<SpriteRenderer>().material.color = Color.yellow;

            // Get weapon list
            InventoryReference = GetAllShowInventoryGameobjects(PlayerManager.Instance.EquippedInventory);

            SelectIncrementor_Row1 = InventoryReference.Items.Count * 1000;
            SelectIncrementor_Row2 = InventoryReference.Weapons.Count * 1000;

            if (ShowRow1)
            {
                GenerateInventory(OriginalPositions.Row1, InventoryReference.Items, SelectIncrementor_Row1, Scales[0], ItemIcon, 0);
            }
            else
            {
                GRow1.SetActive(false);
            }

            GenerateInventory(OriginalPositions.Row2, InventoryReference.Weapons, SelectIncrementor_Row2, Scales[1], WeaponIcon, 1);

            if (!ShowRow3)
            {
                GRow3.SetActive(false);
            }

            if (ShowRow1 && ShowRow3) { RowAmount = 3; }
            else if (ShowRow1 || ShowRow3) { RowAmount = 2; }
            else { RowAmount = 1; }

            if (!ShowRow1) { RowShift = 1; }
            else { RowShift = 0; }

            MarkerVisibility(99);
        }

        if (ShowRow1)
        {
            MoveInventoryObjects(OriginalPositions.Row1, InventoryReference.Items, SelectIncrementor_Row1, ItemIcon);
        }
        MoveInventoryObjects(OriginalPositions.Row2, InventoryReference.Weapons, SelectIncrementor_Row2, WeaponIcon);

    }

    void OnArrowUp()
    {
        SelectedRow = RowShift + ((SelectedRow -= 1) % RowAmount);

        if (SelectedRow < 0)
        {
            SelectedRow = RowShift + (RowAmount - 1);
        }

        MarkerVisibility(SelectedRow);
    }
    void OnArrowDown()
    {
        SelectedRow = RowShift + ((SelectedRow += 1) % RowAmount);
        MarkerVisibility(SelectedRow);
    }


    void MarkerVisibility(int row)
    {
        if (row == 0)
        {
            MarkerImage_Row1.gameObject.SetActive(true);
            MarkerImage_Row2.gameObject.SetActive(false);
            MarkerImage_Row3.gameObject.SetActive(false);
        }
        else if (row == 1)
        {
            MarkerImage_Row2.gameObject.SetActive(true);
            MarkerImage_Row1.gameObject.SetActive(false);
            MarkerImage_Row3.gameObject.SetActive(false);
        }
        else if (row == 2)
        {
            MarkerImage_Row3.gameObject.SetActive(true);
            MarkerImage_Row1.gameObject.SetActive(false);
            MarkerImage_Row2.gameObject.SetActive(false);
        }
        else
        {
            MarkerImage_Row1.gameObject.SetActive(false);
            MarkerImage_Row2.gameObject.SetActive(false);
            MarkerImage_Row3.gameObject.SetActive(false);
        }

    }

    void DisableInventory()
    {
        if (hasShown)
        {
            PlayerManager.Instance.cameraScript.FreezeAndShowCursor(false, gameObject);

            hasShown = false;
            StatusObject.GetComponent<SpriteRenderer>().material.color = Color.white;

            if (InventoryReference.Weapons.Count > 0)
            {
                int newMid = SelectIncrementor_Row2 % InventoryReference.Weapons.Count;
                if (newMid != 0 && InventoryReference.Weapons[newMid].GetComponent<ShowInInventory>().PickedUp)
                {
                    GameObject newWeap = InventoryReference.Weapons[newMid];
                    InventoryReference.Weapons.Remove(InventoryReference.Weapons[newMid]);
                    newWeap.GetComponent<Weapon>().EquipWeapon();
                }
            }

            if (ShowRow3)
            {
                if (InventoryReference.Items.Count > 0)
                {
                    int newBot = SelectIncrementor_Row1 % InventoryReference.Items.Count;
                    if (newBot != 0)
                    {
                        GameObject newItem = InventoryReference.Items[newBot];
                        InventoryReference.Items.Remove(InventoryReference.Items[newBot]);
                        PlayerManager.Instance.Inventory_EquipItem(newItem, false, false);
                    }
                }
            }

            SelectIncrementor_Row1 = InventoryReference.Items.Count * 1000;
            SelectIncrementor_Row2 = InventoryReference.Weapons.Count * 1000;

            Utility.DestroyAllGameObjectsInList(WeaponIcon);
            if (ShowRow3)
            {
                Utility.DestroyAllGameObjectsInList(ItemIcon);
            }

        }
    }

    void OpenInventory()
    {
        if (!Menu.isOpen && DialogueManager.Instance.Dialogue.Count < 1 && !InventoryIsOut)
        {
            canvasGroup.interactable = true;
            InventoryOpenedSound.Post(gameObject);
            InventoryIsOut = true;
            if (EventSystem.current != null)
            {
                EventSystem.current.SetSelectedGameObject(SelectedGameobject);
                SelectedGameobject.GetComponent<Button>().Select();
            }

            GameManager.Instance.gameSpeedHandler.PauseGameSpeed(gameObject.GetInstanceID());
            Panel.SetActive(true);
            InputManager.OnRightArrowDown += ArrowPressRight;
            InputManager.OnLeftArrowDown += ArrowPressLeft;

            GameManager.Instance.BlurCam();
        }
    }

    void CloseInventory()
    {
        if (InventoryIsOut)
        {
            canvasGroup.interactable = false;
            InventoryClosedSound.Post(gameObject);
            InventoryIsOut = false;
            GameManager.Instance.gameSpeedHandler.UnPauseGameSpeed(gameObject.GetInstanceID());

            Panel.SetActive(false);

            InputManager.OnRightArrowDown -= ArrowPressRight;
            InputManager.OnLeftArrowDown -= ArrowPressLeft;

            if (!Menu.isOpen)
            {
                GameManager.Instance.UnBlurCam();
            }
        }
    }

    public List<bool> CanPressLefts = new List<bool>(3);
    public List<bool> CanPressRights = new List<bool>(3);

    void ArrowPressRight()
    {
        if (CanPressRights[SelectedRow])
        {
            ButtonIncrement(SelectedRow);
        }
        else
        {
            print("Inventory: No Inventory Items to the right");
        }
    }
    void ArrowPressLeft()
    {
        if (CanPressLefts[SelectedRow])
        {
            InversedIncrement(SelectedRow);
        }
        else
        {
            print("Inventory: No Inventory Items to the left");
        }
    }

    void Update()
    {
        if (Panel.activeInHierarchy)
        {
            EnableInventory();
        }
        else
        {
            DisableInventory();
        }
    }

    public void ButtonIncrement(int layer)
    {
        InventorySelectSound.Post(gameObject);

        if (Panel.activeInHierarchy && hasShown)
        {
            if (layer == 0)
            {
                SelectIncrementor_Row1++;
                ApplyInventoryInfo2Single_Right(OriginalPositions.Row1, InventoryReference.Items, SelectIncrementor_Row1, Scales[0], ItemIcon);
            }
            else if (layer == 1)
            {
                SelectIncrementor_Row2++;
                ApplyInventoryInfo2Single_Right(OriginalPositions.Row2, InventoryReference.Weapons, SelectIncrementor_Row2, Scales[1], WeaponIcon);
            }
            else if (layer == 2)
            {
                SelectIncrementor_Row3++;
                ApplyInventoryInfo2Single_Right(OriginalPositions.Row3, InventoryReference.Items, SelectIncrementor_Row3, Scales[2], ItemIcon);
            }
        }
    }

    public void InversedIncrement(int layer)
    {
        InventorySelectSound.Post(gameObject);
        if (Panel.activeInHierarchy && hasShown)
        {
            if (layer == 0)
            {
                SelectIncrementor_Row1--;
                ApplyInventoryInfo2Single_Left(OriginalPositions.Row1, InventoryReference.Items, SelectIncrementor_Row1, Scales[0], ItemIcon);
            }
            else if (layer == 1)
            {
                SelectIncrementor_Row2--;
                ApplyInventoryInfo2Single_Left(OriginalPositions.Row2, InventoryReference.Weapons, SelectIncrementor_Row2, Scales[1], WeaponIcon);
            }
            else if (layer == 2)
            {
                SelectIncrementor_Row3--;
                ApplyInventoryInfo2Single_Left(OriginalPositions.Row3, InventoryReference.Items, SelectIncrementor_Row3, Scales[2], ItemIcon);
            }
        }
    }

    Vector2 CheckModuleSize(int wheelSize, List<GameObject> list)
    {
        int x = 0;
        int y = list.Count;

        if (wheelSize == 5)
        {
            x = 0;
            y = list.Count;
        }
        else if (wheelSize == 3)
        {
            x = 1;
            y = list.Count - 1;
        }
        else if (wheelSize == 2)
        {
            x = 2;
            y = list.Count - 1;
        }
        else
        {
            x = 2;
            y = list.Count - 2;
        }
        return new Vector2(x, y);
    }


    void ButtonVisibility(List<GameObject> Positions, List<GameObject> invRef, int increment, int row)
    {

        if (invRef.Count == 2)
        {
            Button[] buttons = Positions[(2) % Positions.Count].GetComponentsInChildren<Button>();
            if (UIhelp != null)
            {
                UIhelp.SetActive(true);
            }
            
            for (int b = 0; b < buttons.Length; b++)
            {
                if (increment % 2 == 0)
                {
                    if (buttons[b].gameObject.name != "Center" && buttons[b].gameObject.name.Substring(0, 8) == "Button_L")
                    {
                        buttons[b].interactable = false;
                    }
                    else
                    {
                        buttons[b].interactable = true;
                    }
                    CanPressLefts[row] = false;
                    CanPressRights[row] = true;
                    //print("Buttons: " + buttons.Length + ", inc: " + increment + ", button B: "+b+", ");
                }
                else
                {
                    if (buttons[b].gameObject.name != "Center" && buttons[b].gameObject.name.Substring(0, 8) == "Button_R")
                    {
                        buttons[b].interactable = false;
                    }
                    else
                    {
                        buttons[b].interactable = true;
                    }
                    CanPressLefts[row] = true;
                    CanPressRights[row] = false;
                }
            }
        }
        else if (invRef.Count < 2)
        {
            Button[] buttons = Positions[(2) % Positions.Count].GetComponentsInChildren<Button>();
            for (int b = 0; b < buttons.Length; b++)
            {
                buttons[b].interactable = false;
                if (UIhelp != null)
                {
                    UIhelp.SetActive(false);
                }

            }
            CanPressLefts[row] = false;
            CanPressRights[row] = false;
            if (invRef.Count < 1)
            {
                Positions[(2) % Positions.Count].GetComponentInChildren<Text>().text = "";
                return;
            }
        }
        else
        {
            Button[] buttons = Positions[(2) % Positions.Count].GetComponentsInChildren<Button>();
            for (int b = 0; b < buttons.Length; b++)
            {
                buttons[b].interactable = true;
                if (UIhelp != null)
                {
                    UIhelp.SetActive(true);
                }
            }
            CanPressLefts[row] = true;
            CanPressRights[row] = true;
        }
    }

}
