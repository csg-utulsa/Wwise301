////////////////////////////////////////////////////////////////////////
//
// Copyright (c) 2018 Audiokinetic Inc. / All Rights Reserved
//
////////////////////////////////////////////////////////////////////////

﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public delegate void DeathMessage();
public delegate void NewWeaponEvent();
public class PlayerManager : Singleton<PlayerManager>
{

    public static event DeathMessage OnDeathMessage;
    public static NewWeaponEvent OnNewWeaponPickedUp;
    public static NewWeaponEvent OnWeaponInventoryChanged;

    protected PlayerManager() { }

    [Header("-- Wwise --")]
    public AK.Wwise.Event Health = new AK.Wwise.Event();
    public AK.Wwise.Trigger Death = new AK.Wwise.Trigger();
    public AK.Wwise.RTPC HealthLevel = new AK.Wwise.RTPC();
    public AK.Wwise.RTPC RegenerationLevel = new AK.Wwise.RTPC();
    public AK.Wwise.Event HurtSound = new AK.Wwise.Event();

    [Header("Player Information")]
    public bool isAlive;
    public bool isDashing;
    public bool inAir;
    public bool isMoving;
    public bool isSprinting;
    public bool Immortal;
    public GameObject PlayerCam;
    public PlayerCamera cameraScript;
    public GameObject targetEnemy;

    [Header("Weapon Information")]
    public GameObject startingWeapon;
    public List<WeaponTypes> pickedUpWeapons = new List<WeaponTypes>();
    public List<GameObject> pickedUpWeaponObjects = new List<GameObject>();
    public GameObject weaponSlot;
    public GameObject equippedWeapon;
    public Weapon equippedWeaponInfo;

    [Header("Player Objects")]
    public GameObject player;
    public Transform playerTransform;
    public Rigidbody playerRb;
    public Collider playerCollider;
    public Animator playerAnimator;
    public GameObject playerHead;
    public Camera TotallyNotZeldaPickupCam;
    public GameObject GraveStone;

    [HideInInspector]
    public PlayerMovement motor;
    [HideInInspector]
    public PlayerAttack attackSystem;

    [Range(0f, 100f), Header("Health")]
    public float HealthOfPlayer = 100f;
    public static float timeBeforeRegen = 3f;
    [Range(0f, 3f)]
    public float regenerationCooldown = 0f;
    [Range(0f, 1f)]
    public float regenSpeed = 0f;
    public float maxRegenerationSpeed = 3f;

    public AnimationCurve RegenerationCurve;
    public float RespawnTime = 8f;
    public DialogueLine deathMessage;
    public bool SpawnDeathWards = true;

    [Header("Inventory")]
    public GameObject Items;

    [System.Serializable]
    public class InventoryObjects
    {
        public List<GameObject> Weapons;
        public List<GameObject> Items;
    }
    public InventoryObjects EquippedInventory;

    public InventoryObjects ShowInInventoryBeforePickup;

    #region private variables
    private bool isRegenerating = false;
    private bool haveClickedToRespawn = true;
    private GameObject CheckPoint;
    private Vector3 playerStartPosition;

    //Cached Animator hashes
    private readonly int isAliveHash = Animator.StringToHash("isAlive");
    private readonly int hurtHash = Animator.StringToHash("Hurt");
    private readonly int pickUpItemHash = Animator.StringToHash("PickupItem");
    private readonly int deathNormalHash = Animator.StringToHash("Death_Normal");
    private readonly int deathHardBehindHash = Animator.StringToHash("Death_Hard_FromBehind");
    private readonly int deathHardFrontHash = Animator.StringToHash("Death_Hard_FromFront");
    private readonly int magicInterruptHash = Animator.StringToHash("Magic_Interrupted");
    private readonly int chargeMagicHash = Animator.StringToHash("ChargeMagic");
    private readonly int canShootMagicHash = Animator.StringToHash("CanShootMagic");
    private readonly int chargingMagicHash = Animator.StringToHash("ChargingMagic");
    private readonly int shootMagicHash = Animator.StringToHash("ShootMagic");
    #endregion

    public static MaterialChecker foot_L;
    public static MaterialChecker foot_R;

    void Awake()
    {
        EquippedInventory = new InventoryObjects();
        isAlive = true;

        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player");

        }
        if (player != null)
        {
            playerStartPosition = player.transform.position;
            SetupPlayerConnections();
        }

        StartCoroutine(Regen());

        // Get weapon / armor / item gameobject
        if (weaponSlot != null && ShowInInventoryBeforePickup != null)
        {
            Items = GameObject.FindGameObjectWithTag("PlayerItem");

            // Start by filling up inventory
            EquippedInventory.Weapons = GetAllChildrenWithInventoryTrue(weaponSlot.transform);
            EquippedInventory.Items = GetAllChildrenWithInventoryTrue(Items.transform);

            //for (int i = 0; i < ShowInInventoryBeforePickup.Weapons.Count; i++) { EquipedInventory.Weapons.Add(ShowInInventoryBeforePickup.Weapons[i]); }
            for (int i = 0; i < ShowInInventoryBeforePickup.Items.Count; i++) { EquippedInventory.Items.Add(ShowInInventoryBeforePickup.Items[i]); }
        }
    }

    private void Start()
    {
        //Equip starting weapon
        if (startingWeapon != null)
        {
            GameObject go = Instantiate(startingWeapon, Vector3.zero, Quaternion.identity) as GameObject;
            GameManager.InteractWithObject(go);
        }
    }

    public List<GameObject> GetAllChildrenWithInventoryTrue(Transform reference)
    {
        List<GameObject> listOfGameobjects = new List<GameObject>();
        //print(reference.childCount);
        for (int c = 0; c < reference.childCount; c++)
        {
            //print (reference.GetChild (c).gameObject.name);
            var sii = reference.GetChild(c).GetComponent<ShowInInventory>();
            if (sii != null && sii.VisibleInInventory)
            {
                listOfGameobjects.Add(reference.GetChild(c).gameObject);
            }

        }
        return listOfGameobjects;
    }

    void SetupPlayerConnections()
    {
        playerRb = player.GetComponent<Rigidbody>();
        playerTransform = player.transform;
        playerCollider = player.GetComponent<Collider>();
        motor = player.GetComponent<PlayerMovement>();
        attackSystem = player.GetComponent<PlayerAttack>();
        playerHead = GameObject.FindGameObjectWithTag("PlayerHead");
        playerAnimator = player.GetComponent<Animator>();

        if (weaponSlot == null)
        {
            weaponSlot = GameObject.Find("Weapon_Slot").transform.GetChild(0).gameObject;
        }

        Transform pickupCamTransform = player.transform.Find("PickupCamera");
        if (pickupCamTransform != null)
        {
            TotallyNotZeldaPickupCam = player.transform.Find("PickupCamera").GetComponent<Camera>();
        }
        
        if (Camera.main != null)
        {
            PlayerCam = Camera.main.gameObject;
            cameraScript = PlayerCam.GetComponent<PlayerCamera>();
        }


    }

    private void Update()
    {
        if (player != null)
        {
            if (playerTransform == null)
            {
                playerTransform = player.transform;
            }

            if (weaponSlot == null)
            {
                weaponSlot = GameObject.FindGameObjectWithTag("PlayerWeapon").gameObject;
            }
            if (equippedWeapon == null && EquippedInventory.Weapons != null)
            {
                equippedWeapon = weaponSlot.transform.GetChild(0).gameObject;
                equippedWeapon = EquippedInventory.Weapons[0].gameObject;
                equippedWeapon.SetActive(true);
                equippedWeaponInfo = equippedWeapon.GetComponent<Weapon>();
            }
            else if (equippedWeaponInfo == null && EquippedInventory.Weapons != null)
            {
                equippedWeaponInfo = equippedWeapon.GetComponent<Weapon>();
            }
        }

    }

    public bool Magic_isCharging() {
        return playerAnimator.GetBool(chargingMagicHash);
    }

    public bool Magic_CanShoot() {
        return playerAnimator.GetBool(canShootMagicHash);
    }

    public void Magic_AllowShooting() {
        if (playerAnimator.GetBool(chargingMagicHash)) {
            playerAnimator.SetBool(canShootMagicHash, true);
        }
    }

    public void Magic_StartCharging() {
        playerAnimator.ResetTrigger(magicInterruptHash);
        playerAnimator.SetTrigger(chargeMagicHash);
        PauseMovement(player);
        PauseAttacking(player);
        playerAnimator.SetBool(chargingMagicHash, true);
    }

    public void Magic_Shoot() {
        Magic_StopCasting();
        playerAnimator.SetTrigger(shootMagicHash);
        playerAnimator.ResetTrigger(chargingMagicHash);
        playerAnimator.SetBool(canShootMagicHash, false);
    }

    public void Magic_StopCasting()
    {
        Player_Reset();
        playerAnimator.SetBool(chargingMagicHash, false);
        playerAnimator.SetTrigger(magicInterruptHash);
    }

    public void Player_Reset()
    {
        motor.movementPausers.Clear();
        attackSystem.AttackPausers.Clear();
    }

    #region HEALTH
    IEnumerator AkHealthStatus()
    {
        while (true)
        {
            HealthLevel.SetGlobalValue(Mathf.Min(HealthOfPlayer, 100));
            yield return new WaitForSeconds(0.1f);
        }
    }


    IEnumerator Regen()
    {
        Health.Post(this.gameObject);
        StartCoroutine(AkHealthStatus());
        while (true)
        {
            if (isAlive)
            {
                if (isRegenerating)
                {

                    if (regenSpeed < 1f)
                    {
                        regenSpeed += Time.deltaTime / maxRegenerationSpeed;
                    }

                    if (HealthOfPlayer < 100f)
                    {
                        RegenerationLevel.SetGlobalValue(RegenerationCurve.Evaluate(regenSpeed) * 100f);
                        HealthOfPlayer += RegenerationCurve.Evaluate(regenSpeed);
                    }
                    else
                    {
                        RegenerationLevel.SetGlobalValue(0f);
                    }
                }
                else
                {
                    regenerationCooldown += Time.deltaTime / timeBeforeRegen;
                    if (regenerationCooldown > 1f)
                    {
                        isRegenerating = true;
                    }
                }
            }
            else
            {
                HealthOfPlayer = 100f;
            }

            yield return null;
        }
    }

    void StopRegen()
    {
        regenerationCooldown = 0f;
        isRegenerating = false;

    }

    public void TakeDamage(Attack a) //TODO: This should maybe be moved into the Health class instead? 
    {
        if (!Immortal)
        {
            HealthOfPlayer -= a.damage;
            HurtSound.Post(player);
            StopRegen();
            if (HealthOfPlayer < 0f)
            {
                Respawn();
                playerAnimator.SetBool(isAliveHash, false);
                if (a.damage > 20f)
                { //if the damage that killed the player is more than 20
                    float angle = Vector3.Angle(PlayerManager.Instance.playerTransform.forward, a.attackDir);
                    PlayerManager.Instance.playerAnimator.ResetTrigger(hurtHash);
                    if (Mathf.Abs(angle) > 90)
                    {
                        playerAnimator.SetTrigger(deathHardFrontHash);
                    }
                    else
                    {
                        playerAnimator.SetTrigger(deathHardBehindHash);
                    }
                    Death.Post(player.gameObject);

                }
                else
                {
                    playerAnimator.SetTrigger(deathNormalHash);
                }
            }
            else
            {
                playerAnimator.SetTrigger(hurtHash);
            }
        }
    }

    public void Respawn()
    {
        StartCoroutine(RespawnSequence());
    }

    public void SetRespawn(GameObject spawnPos)
    {
        if (CheckPoint != null && spawnPos != CheckPoint)
        {
            CheckPoint.GetComponent<CheckPoint>().DisableCheckPoint();
        }

        CheckPoint = spawnPos;

    }

    IEnumerator RespawnSequence()
    {
        isAlive = false;
        Death.Post(GetComponent<GameManager>().MusicGameObject);

        UI_Overlayers.Instance.FadeLayer(true, 6.0f);

        // DISABLE COLLIDER
        //playerCollider.enabled = false;
        //playerRb.isKinematic = true;

        // DISABLE MOVEMENT
        PauseMovement(this.gameObject);
        PauseAttacking(this.gameObject);

        // SEND DEATH DIALOGUE
        List<DialogueLine> newDeathMessages = new List<DialogueLine>();
        newDeathMessages.Add(deathMessage);
        DialogueManager.Instance.TransferDialogue(newDeathMessages, this.gameObject);

        // RESPAWN WAIT

#if UNITY_STANDALONE || UNITY_WEBGL
        InputManager.OnUseDown += ClickToRespawn;
#endif

#if UNITY_ANDROID || UNITY_IOS
        MobileEvents.OnMobileUse += ClickToRespawn;
#endif
        haveClickedToRespawn = false;

        Vector3 DeathPos = player.transform.position;

        // WAIT FOR CLICK
        yield return new WaitUntil(() => haveClickedToRespawn);

        playerAnimator.SetBool(chargeMagicHash, false);
        playerAnimator.SetBool(isAliveHash, true);

        // PUT PLAYER AT SPAWN
        if (CheckPoint != null)
        {
            player.transform.position = CheckPoint.transform.position + Vector3.up * 3f;
        }
        else
        {
            player.transform.position = playerStartPosition + Vector3.up * 3f;
        }
        InputManager.OnUseDown -= ClickToRespawn;

        UI_Overlayers.Instance.FadeLayer(false, 3.0f);

        Instantiate(GraveStone, DeathPos, Quaternion.identity);

        DeathMessage();

        // ENABLE MOVEMENT
        ResumeMovement(this.gameObject);
        ResumeAttacking(this.gameObject);
        isAlive = true;

        // SET CAM BACK TO PLAYER
        cameraScript.cameraMode = PlayerCamera.CameraMode.normal;
    }

    void DeathMessage()
    {
        if (OnDeathMessage != null)
        {
            OnDeathMessage();
        }
    }

    void ClickToRespawn()
    {
        haveClickedToRespawn = true;
    }

    public void ResetFocus()
    {
        if (targetEnemy != null)
        {
            targetEnemy = null;
        }
    }

    #endregion

    // ADD WEAPONS TO INVENTORY
    public void Inventory_EquipWeapon(GameObject weaponObject)
    {
        var weaponType = weaponObject.GetComponent<Weapon>().weaponType;

        if (EquippedInventory.Weapons != null)
        {
            if (EquippedInventory.Weapons.Contains(weaponObject))
            {
                EquippedInventory.Weapons.Remove(weaponObject);
            }
            else
            {
                //This is a completely new Weapon.
                if (OnNewWeaponPickedUp != null)
                {
                    OnNewWeaponPickedUp();

                    if (!pickedUpWeapons.Contains(weaponType))
                    {
                        pickedUpWeapons.Add(weaponType);
                    }
                }
            }
        }

        if (!pickedUpWeaponObjects.Contains(weaponObject))
        {
            pickedUpWeaponObjects.Add(weaponObject);
        }

        EquippedInventory.Weapons.Insert(0, weaponObject);
        UpdateVisibility(EquippedInventory.Weapons);
        weaponObject.SetActive(true);
        weaponObject.GetComponent<ShowInInventory>().SetItemToPickedUp(true);

        if (OnWeaponInventoryChanged != null)
        {
            OnWeaponInventoryChanged();
        }
    }

    public void Inventory_DestroyWeapon(GameObject weaponObject)
    {
        var weaponType = weaponObject.GetComponent<Weapon>().weaponType;

        if (EquippedInventory.Weapons.Contains(weaponObject))
        {
            EquippedInventory.Weapons.Remove(weaponObject);
            pickedUpWeaponObjects.Remove(weaponObject);
            UpdateVisibility(EquippedInventory.Weapons);
            weaponObject.GetComponent<ShowInInventory>().SetItemToPickedUp(false);
            Destroy(weaponObject);
        }

        if (pickedUpWeapons.Contains(weaponType))
        {
            pickedUpWeapons.Remove(weaponType);
        }

        if (OnWeaponInventoryChanged != null)
        {
            OnWeaponInventoryChanged();
        }
    }

    public void Inventory_UnequipWeapon(GameObject weaponObject)
    {
        if (EquippedInventory.Weapons != null)
        {
            if (EquippedInventory.Weapons.Contains(weaponObject))
            {
                EquippedInventory.Weapons.Remove(weaponObject);
                weaponObject.SetActive(false);
                UpdateVisibility(EquippedInventory.Weapons);
                weaponObject.GetComponent<ShowInInventory>().SetItemToPickedUp(false);

                EquippedInventory.Weapons[0].GetComponent<Weapon>().EquipWeapon();
            }
        }
    }

    public GameObject FindGameObjectWithString(List<GameObject> SearchList, string Prefix, string lookfor)
    {

        for (int i = 0; i < SearchList.Count; i++)
        {
            string lookforName = lookfor.Replace(Prefix, "");
            lookforName = lookforName.Split(" ".ToCharArray(), System.StringSplitOptions.None)[0];
            lookforName = lookforName.Split("(".ToCharArray(), System.StringSplitOptions.None)[0];
            string itemName = SearchList[i].name.Replace(Prefix, "");
            itemName = itemName.Split(" ".ToCharArray(), System.StringSplitOptions.None)[0];

            if (itemName == lookforName)
            {
                return SearchList[i].gameObject;
            }
        }
        return null;
    }

    public void Inventory_EquipItem(GameObject itemObject, bool transform2Player, bool isAmountItem)
    {
        if (isAmountItem)
        {
            List<GameObject> allInventoryItems = Utility.GetAllChildren(Items.transform);
            GameObject ListItem = FindGameObjectWithString(allInventoryItems, "Collectable_", itemObject.name);

            if (ListItem != null)
            {
                int instanceID = itemObject.transform.parent != null ? itemObject.transform.parent.GetInstanceID() : itemObject.transform.GetInstanceID();

                if (Items.transform.GetInstanceID() != instanceID)
                {
                    ListItem.GetComponent<ShowInInventory>().AddAmount2Object(itemObject.GetComponent<ShowInInventory>().currentAmount);
                    //Destroy(itemObject.gameObject);
                    return;
                }
            }
        }

        if (EquippedInventory.Items.Contains(itemObject))
        {
            EquippedInventory.Items.Remove(itemObject);
            EquippedInventory.Items.Insert(0, itemObject);

        }
        else
        {
            EquippedInventory.Items.Insert(0, itemObject);
        }

        itemObject.GetComponent<ShowInInventory>().SetItemToPickedUp(true);

        if (transform2Player)
        {
            itemObject.transform.parent = Items.transform;
            itemObject.transform.position = Items.transform.position;
            itemObject.transform.rotation = Items.transform.rotation;
            itemObject.SetActive(false);
        }
    }

    public void StartPickupEvent()
    {
        playerAnimator.SetTrigger(pickUpItemHash);

    }

    // REMOVE WEAPONS FROM INVENTORY
    public void Inventory_RemoveWeapon(GameObject weaponObject)
    {
        EquippedInventory.Weapons.Remove(weaponObject);
    }
    public void Inventory_RemoveItem(GameObject itemObject)
    {
        EquippedInventory.Items.Remove(itemObject);
    }

    public void PickUpEvent()
    {
        playerRb.velocity = Vector3.zero;
        cameraScript.ChangeCamera(new PlayerCamera.CameraEvent(TotallyNotZeldaPickupCam, 0.5f, 1.5f, true));
    }

    public void PauseMovement(GameObject pauserObject)
    {
        if (!motor.movementPausers.Contains(pauserObject))
        {
            playerRb.velocity = Vector3.zero;
            motor.movementPausers.Add(pauserObject);
        }
    }

    public void ResumeMovement(GameObject pauserObject)
    {
        if (motor.movementPausers.Contains(pauserObject))
        {
            motor.movementPausers.Remove(pauserObject);
        }
    }

    public void PauseAttacking(GameObject pauserObject)
    {

        attackSystem.AttackPausers.Add(pauserObject);
    }

    public void ResumeAttacking(GameObject pauserObject)
    {
        if (attackSystem.AttackPausers.Contains(pauserObject))
        {
            attackSystem.AttackPausers.Remove(pauserObject);
        }
    }

    void UpdateVisibility(List<GameObject> list)
    {
        list[0].SetActive(true);

        for (int i = 1; i < list.Count; i++)
        {
            if (list[i].GetComponent<ShowInInventory>().PickedUp)
            {
                list[i].SetActive(false);
            }
        }
    }

    public void CamShake(PlayerCamera.CameraShake shake)
    {
        cameraScript.CamShake(shake);

    }

    public void StartShake(float strength)
    {
        cameraScript.StartShake(strength);
    }

    public void StopShake()
    {
        cameraScript.StopShake();
    }

}
