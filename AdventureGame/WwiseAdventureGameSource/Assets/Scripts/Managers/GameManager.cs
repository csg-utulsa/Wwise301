////////////////////////////////////////////////////////////////////////
//
// Copyright (c) 2018 Audiokinetic Inc. / All Rights Reserved
//
////////////////////////////////////////////////////////////////////////

﻿using UnityEngine;
using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityStandardAssets.ImageEffects;

public delegate void dayNightPush(bool condition);
public delegate void OnMusic();

public class GameManager : Singleton<GameManager>
{
    protected GameManager() { }

    [Header("Wwise")]
    public AK.Wwise.RTPC OnMenu = new AK.Wwise.RTPC();
    public AK.Wwise.Event MusicEvent = new AK.Wwise.Event();
    public AK.Wwise.State MusicStart_Region = new AK.Wwise.State();
    public AK.Wwise.Trigger EnemyMusicTrigger = new AK.Wwise.Trigger();
    public AK.Wwise.Event EnemyMusicEvent = new AK.Wwise.Event();

    [Space(10f)]

    public AK.Wwise.RTPC EvilCrawlerRTPC = new AK.Wwise.RTPC();
    public AK.Wwise.RTPC EvilHeadRTPC = new AK.Wwise.RTPC();
    public AK.Wwise.RTPC EvilSpitPlantRTPC = new AK.Wwise.RTPC();
    public AK.Wwise.RTPC CombatLevelRTPC = new AK.Wwise.RTPC();
    public float proximityThreshold = 10f;

    [Space(10f)]

    [Header("Coins")]
    public CoinHandler coinHandler;

    [Header("Game Speed")]
    public GameSpeedHandler gameSpeedHandler;

    [Header("DLC Management")]
    public List<DLC> activeDLCs;

    public AK.Wwise.RTPC TimeOfDayRTPC;
    public static float TimeOfDay = 0;
    public static void SetTimeOfDay(float t) {
        if (Instance.TimeOfDayRTPC.Validate())
        {
            Instance.TimeOfDayRTPC.SetGlobalValue(t);
        }
        TimeOfDay = t;
    }

    public Vector2 DayAndNightChange;
    public bool dayTime = false;
    private bool lastDayTime = false;

    public static event dayNightPush DayNightCall;
    public static event OnMusic OnMusicAction;

    #region private variables
    private bool overrideEnemyRTPCs = false;

    //Camera Effects
    private Camera ActiveCamera;
    private BlurOptimized blur;
    private IEnumerator blurRoutine;
    #endregion

    [HideInInspector]
    public GameObject MusicGameObject;

    [Header("CHEATS")]
    public bool BigHeadMode;
    public bool AIPaused;
    [Header("MODES")]
    public bool DisableWwizardMagicStateOnStart = true;

    public QuestSystem.Quest_Forge Quest_ForgeSystem;

    /// <summary>
    /// Used with dynamic bools from UnityEvents on the UI
    /// </summary>
    /// <param name="on">If set to <c>true</c> on.</param>
    public void SetGodMode(bool on)
    {
        PlayerManager.Instance.Immortal = on;
    }

    /// <summary>
    /// Used with dynamic bools from UnityEvents on the UI
    /// </summary>
    /// <param name="on">If set to <c>true</c> on.</param>
    public void SetBigHeadMode(bool on)
    {
        BigHeadMode = on;
    }

    /// <summary>
    /// Function meant to be called from a Callback function. This will push all RhythmActions scripts subscribed to the OnMusicAction OnMusic type Event. 
    /// </summary>
    public static void PushRhythmAction() {
        // If nothing is subscribed to it, don't call. 
        if (OnMusicAction != null) {
            OnMusicAction();
        }
    }

    void Start()
    {
        if (DisableWwizardMagicStateOnStart) {
            AkSoundEngine.SetState("MagicZone", "Outside");
        }
        
        MusicGameObject = GameObject.Find("Ak_PlayMusic");
        DayNightCall += dayNightPush;
        MusicStart_Region.SetValue();

        AkCallbackType CallbackType = AkCallbackType.AK_MusicSyncUserCue;
        MusicEvent.Post(gameObject, (uint)CallbackType, CallbackFunction);

        StartCoroutine(DistanceToEnemies());
    }
    void CallbackFunction(object in_cookie, AkCallbackType in_type, object in_info)
    {
        if (in_type == AkCallbackType.AK_MusicSyncUserCue) {

            AkMusicSyncCallbackInfo info = (AkMusicSyncCallbackInfo)in_info;
            string callbackText = info.userCueName;
            if (callbackText == "SpawnEnemies" && Quest_ForgeSystem != null) {
                Quest_ForgeSystem.AllowEnemiesToSpawn();
            }
        }
    }

    void Awake()
    {
        
        ActiveCamera = Camera.main;
        if (ActiveCamera != null)
        {
            if (ActiveCamera.GetComponent<BlurOptimized>() != null)
            {
                blur = ActiveCamera.GetComponent<BlurOptimized>();
            }
            else
            {
                BlurOptimized b = ActiveCamera.gameObject.AddComponent<BlurOptimized>();
                blur = b;
            }
            blur.blurIterations = 1;
            blur.enabled = false;
        }
    }

    void Update()
    {
        // ------------------------ DAY / NIGHT CYCLE
        if (TimeOfDay < DayAndNightChange.x || TimeOfDay > DayAndNightChange.y)
        {
            dayTime = false;
        }
        else if (TimeOfDay > DayAndNightChange.x && TimeOfDay < DayAndNightChange.y)
        {
            dayTime = true;
        }

        if (dayTime != lastDayTime)
        {
            DayNightCall(dayTime);
            lastDayTime = dayTime;
        }
    }

    public void BlurCam()
    {
        if (blur != null)
        {
            if (blurRoutine != null)
            {
                StopCoroutine(blurRoutine);
            }
            blurRoutine = Blur(4f, 1f);
            StartCoroutine(blurRoutine);
        }
    }
    public void UnBlurCam()
    {
        if (blur != null)
        {
            if (blurRoutine != null)
            {
                StopCoroutine(blurRoutine);
            }
            blurRoutine = Blur(0f, 0.15f);
            StartCoroutine(blurRoutine);
        }
    }

    IEnumerator Blur(float target, float seconds)
    {
        if (blur.blurSize == target)
        {
            yield break;
        }

        blur.enabled = true;
        float currentBlur = blur.blurSize;

        for (float t = 0f; t < 1f; t += Time.unscaledDeltaTime / seconds)
        {
            blur.blurSize = Mathf.Lerp(currentBlur, target, t);
            yield return null;
        }
        blur.blurSize = target;
        blur.enabled = target == 0f ? false : true;
    }

    void dayNightPush(bool condition) { }

    [Header("Zones")]
    public List<ZoneTrigger> CurrentZones = new List<ZoneTrigger>();
    public List<Creature> Enemies = new List<Creature>();

    public void LeaveZone(ZoneTrigger Z)
    {
        CurrentZones.Remove(Z);
        UpdateMusic();
    }

    public void EnterZone(ZoneTrigger Z)
    {
        CurrentZones.Insert(0, Z);
        UpdateMusic();
    }

    void UpdateMusic()
    {
        if (CurrentZones.Count > 0)
        {
            CurrentZones[0].MusicState.SetValue();
        }
        else
        {
            MusicStart_Region.SetValue();
        }
    }

    public void SetInCombat(Creature G)
    {
        if (!Enemies.Contains(G))
        {
            Enemies.Add(G);
        }
    }

    [SerializeField, Range(0f, 1f)]
    private float EvilHeadValue = 1f;
    [SerializeField, Range(0f, 1f)]
    private float EvilSpitPlantValue = 1f;
    [SerializeField, Range(0f, 1f)]
    private float EvilCrawlerValue = 1f;

    IEnumerator DistanceToEnemies()
    {
        while (true)
        {
            float combatLevel = Enemies.Count;

            if (!overrideEnemyRTPCs)
            {
                EvilHeadValue = EvilCrawlerValue = EvilSpitPlantValue = 1f;

                if (Enemies.Count > 0)
                {
                    Transform playerTransform = PlayerManager.Instance.playerTransform;
                    if (Enemies.Count > 2)
                    {
                        Enemies.Sort((Creature a, Creature b) => Vector3.SqrMagnitude(playerTransform.position - a.transform.position).CompareTo(Vector3.SqrMagnitude(playerTransform.position - b.transform.position)));
                    }

                    Creature closestEvilHead = Enemies.FirstOrDefault(x => x.AttackType == WeaponTypes.EvilHead);
                    Creature closestEvilSpitPlant = Enemies.FirstOrDefault(x => x.AttackType == WeaponTypes.EvilSpitPlant);
                    Creature closestEvilCrawler = Enemies.FirstOrDefault(x => x.AttackType == WeaponTypes.EvilCrawler);

                    EvilHeadValue = Mathf.Clamp01(closestEvilHead != null ? (Vector3.SqrMagnitude(closestEvilHead.transform.position - playerTransform.position) / (proximityThreshold * proximityThreshold)) : 1f);
                    EvilSpitPlantValue = Mathf.Clamp01(closestEvilSpitPlant != null ? (Vector3.SqrMagnitude(closestEvilSpitPlant.transform.position - playerTransform.position) / (proximityThreshold * proximityThreshold)) : 1f);
                    EvilCrawlerValue = Mathf.Clamp01(closestEvilCrawler != null ? (Vector3.SqrMagnitude(closestEvilCrawler.transform.position - playerTransform.position) / (proximityThreshold * proximityThreshold)) : 1f);

                }
            }
			EvilHeadRTPC.SetGlobalValue(EvilHeadValue * 100f);
			EvilCrawlerRTPC.SetGlobalValue(EvilCrawlerValue * 100f);
			EvilSpitPlantRTPC.SetGlobalValue(EvilSpitPlantValue * 100f);

            CombatLevelRTPC.SetGlobalValue(combatLevel);

            yield return new WaitForSeconds(0.1f);
        }
    }

    public void OverrideEnemyRTPCValues(bool enableOverride, float value = 100f)
    {
        overrideEnemyRTPCs = enableOverride;

        if (enableOverride)
        {
            EvilHeadValue = EvilCrawlerValue = EvilSpitPlantValue = value / 100f;
        }
    }

    public void RemoveFromCombat(Creature G)
    {
        if (Enemies.Contains(G))
        {
            bool removed = Enemies.Remove(G);
            if (!removed)
            {
                print("Was not able to remove " + G.name);
            }
        }
    }

    public float GetTimeOfDayParameter() {
        return TimeOfDay;
    }


    public static void DamageObject(GameObject objectToDamage, Attack attack)
    {
        IDamageable[] damageables = objectToDamage.gameObject.GetComponentsInChildren<IDamageable>();
        for (int i = 0; i < damageables.Length; i++)
        {
            damageables[i].OnDamage(attack);
        }
    }

    public static void InteractWithObject(GameObject objectToInteractWith)
    {
        IInteractable[] interactables = objectToInteractWith.gameObject.GetComponentsInChildren<IInteractable>();

        for (int i = 0; i < interactables.Length; i++)
        {
            interactables[i].OnInteract();
        }
    }



}