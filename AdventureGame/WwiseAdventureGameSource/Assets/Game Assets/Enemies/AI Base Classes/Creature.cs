////////////////////////////////////////////////////////////////////////
//
// Copyright (c) 2018 Audiokinetic Inc. / All Rights Reserved
//
////////////////////////////////////////////////////////////////////////

﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;

public abstract class Creature : MonoBehaviour, IDamageable //TODO: This class is due for a big ol' rework. It's too monolithic and includes much functionality that should really be split into derived classes.
{
    public delegate void CreatureEvent();

    public event CreatureEvent OnCreatureDeath;

    [Header("Movement")]
    public bool CanMove = true;
    [ShowIf("CanMove", true)]
    public bool HasNavMesh = false;
    [ShowIf("CanMove", true)]
    public bool canRoam = false;
    [ShowIf("canRoam", true)]
    public int RoamRapidness = 5;
    public bool periodicallyRotate = false;
    [ShowIf("CanMove", false)]
    public Transform RotationObject;
    [ShowIf("HasNavMesh", false)]
    public float NoNavRotationSpeed = 3f;
    public bool lockYRotation = false;

    [Header("Vision")]
    public float AgroRange = 5f;
    public float fullVisionRange = 30f;
    [ShowIf("HasNavMesh", true)]
    public float NavMeleeDistance = 2f;
    [ShowIf("HasNavMesh", true)]
    public float NavRangedDistance = 5f;

    [Header("AI Behaviour")]
    public bool canTakeDamage = true;
    public bool isFriendly = false;
    [ShowIf("CanMove", true)]
    public bool isFollowing = false;
    [ShowIf("isFriendly", false)]
    public Collider WeaponCollider;
    [ShowIf("isFriendly", false)]
    public Weapon weapon;
    protected CreatureState state;
    public CreatureState currentState;
    public CreatureState ReturnState = CreatureState.patrolling;
    public LayerMask AwareOf;

    [Header("Interaction"), ShowIf("isFriendly", false)]
    public bool isMelee = false;
    [ShowIf("isFriendly", false)]
    public bool isRanged = false;
    [ShowIf("isFriendly", false)]
    public bool attackDespiteDay = false; //TODO: Only attacking during daytime is as of now a 100% unused concept in WPAG. We should consider removing stuff like this if we do not plan to include this feature.
    [ShowIf("isFriendly", false)]
    public bool CanAttackAll = false;
    [ShowIf("CanMove", true)]
    public bool CanKnockBack = true;

    [Header("Health")]
    public float Health = 30f;
    public bool CanBeStunned = true;
    [ShowIf("CanBeStunned", true)]
    public float StunDuration = 5f;
    [ShowIf("CanMove", true)]
    public bool IsImmobilized = false;
    public float ImmobilizedDuration = 3f;
    public bool DestroyCreatureOnDeath = true;
    [ShowIf("DestroyCreatureOnDeath", false)]
    public bool shouldDisableCollider = false;
    public bool PlayDeathSoundOnDeath = true;

    [HideInInspector]
    public GameObject OriginalPrefab;
    [HideInInspector]
    public float OriginalHealth;

    [Header("Damage ")]
    public float AttackDamage = 10f;
    public float PastAttackDelay = 1f;
    public float OnSpottingAttackDelay = 0f;
    [ShowIf("isRanged", true)]
    public bool HasShootingAngle = false;
    [ShowIf("HasShootingAngle", true)]
    public float maxShootAngle = 30f;

    [Header("Animations")]
    [SerializeField]
    protected Animator anim;
    public AttackAnimations AttackAnimation;
    public KnockBackAnimations KnockBackAnimation;
    public DeathAnimation DeathAnimations;
    public TalkAnimation TalkAnimations;
    public IdleAnimation IdleAnimations;
    public string WalkingAnimation;

    [Header("Termination")]
    public GameObject StunParticles;

    [Header("Other")]
    public DebugOption Debugging;

    [Header("Wwise")]
    public AK.Wwise.Event HurtSound;
    public AK.Wwise.Event DeathSound;

    IEnumerator StateSwitchInstance;
    IEnumerator ActiveStateInstance;
    IEnumerator ObservantInstance;
    protected Rigidbody thisRigidbody;
    protected NavMeshAgent thisNavMeshAgent;
    protected bool hasAttacked = false;
    protected GameObject targetOfNPC;
    [HideInInspector]
    public bool isAlive = true;
    [HideInInspector]
    public Vector3 startPosition;
    protected Attack LastAttack;
    public bool daytime = false;
    protected bool isMoving = false;
    protected bool ready2Roam = true;

    #region private variables
    private float spotSpeed = 0.2f;
    private float currentVisionRange = 3f;
    private string previousSwing = "";
    private bool pausedAI = false;
    #endregion

    [Header("CreatureType")]
    public WeaponTypes AttackType;

    public virtual void Start()
    {
        PlayerCamera.OnCameraEventStart += PauseAI;
        PlayerCamera.OnCameraEventEnd += ResumeAI;

        OriginalPrefab = this.gameObject;
        OriginalHealth = Health;
        if (RotationObject == null)
        {
            RotationObject = this.transform;
        }
        if (anim == null)
        {
            anim = GetComponent<Animator>();
        }

        thisRigidbody = GetComponent<Rigidbody>();
        thisNavMeshAgent = GetComponent<NavMeshAgent>();
        //print(thisNavMeshAgent);
        if (thisNavMeshAgent != null)
        {
            startPosition = thisNavMeshAgent.nextPosition;
            thisNavMeshAgent.ResetPath();
        }

        currentState = CreatureState.none;

        StateSwitchInstance = StateSwitch();
        StartCoroutine(StateSwitchInstance);

        ObservantInstance = Awareness();
        StartCoroutine(ObservantInstance);

        currentVisionRange = AgroRange;
        if (Debugging.RaycastVisionRange)
        {
            if (thisNavMeshAgent != null)
            {
                Utility.RayVisionRange(30, thisNavMeshAgent.nextPosition, 0.5f, 10f, AgroRange, Color.gray);
                Utility.RayVisionRange(30, thisNavMeshAgent.nextPosition, 0.5f, 10f, fullVisionRange, Color.black);
            }
            else
            {
                Utility.RayVisionRange(30, transform.position, 0.5f, 10f, AgroRange, Color.cyan);
                Utility.RayVisionRange(30, transform.position, 0.5f, 10f, fullVisionRange, Color.black);
            }
        }

        if (GetComponentInChildren<Weapon>() != null)
        {
            WeaponCollider = GetComponentInChildren<Weapon>().gameObject.GetComponent<Collider>();
        }
        GameManager.DayNightCall += SetDayTime;
    }

    void SetDayTime(bool condition)
    {
        daytime = condition;
    }

    //    _____ _______    _______ ______   _____   ____  _    _ _______ _____ _   _ ______  _____ 
    //   / ____|__   __|/\|__   __|  ____| |  __ \ / __ \| |  | |__   __|_   _| \ | |  ____|/ ____|
    //  | (___    | |  /  \  | |  | |__    | |__) | |  | | |  | |  | |    | | |  \| | |__  | (___  
    // 	 \___ \   | | / /\ \ | |  |  __|   |  _  /| |  | | |  | |  | |    | | | . ` |  __|  \___ \ 
    // 	 ____) |  | |/ ____ \| |  | |____  | | \ \| |__| | |__| |  | |   _| |_| |\  | |____ ____) |
    //  |_____/   |_/_/    \_\_|  |______| |_|  \_\\____/ \____/   |_|  |_____|_| \_|______|_____/ 

    /// <summary>
    /// The state-switch that checks what the state is and activates the correct coroutines. 
    /// </summary>
    /// <returns>The switch.</returns>
    IEnumerator StateSwitch()
    {
        while (true)
        {
            if (!isFollowing && state == CreatureState.follow)
            {
                state = CreatureState.patrolling;
                //print(gameObject.name+": Follow state to patrol");
            }

            if (currentState != state)
            {
                // Clear current Coroutine
                if (ActiveStateInstance != null)
                {
                    StopCoroutine(ActiveStateInstance);
                    ActiveStateInstance = null;
                }

                if (state == CreatureState.attacking || state == CreatureState.immobilized || state == CreatureState.stunned)
                {
                    GameManager.Instance.SetInCombat(this);
                }
                else
                {
                    GameManager.Instance.RemoveFromCombat(this);
                }

                // Select new coroutine
                switch (state)
                {
                    case CreatureState.dead:
                        ActiveStateInstance = Death();
                        break;

                    case CreatureState.stunned:
                        ActiveStateInstance = Stunned();
                        break;

                    case CreatureState.attacking:
                        ActiveStateInstance = Attacking();
                        break;

                    case CreatureState.immobilized:
                        ActiveStateInstance = Immobalized();
                        break;

                    case CreatureState.follow:
                        ActiveStateInstance = Following();
                        break;

                    case CreatureState.talking:
                        ActiveStateInstance = Talking();
                        break;

                    case CreatureState.patrolling:
                        ActiveStateInstance = Patrolling();
                        if (IdleAnimationRoutine != null)
                        {
                            StopCoroutine(IdleAnimationRoutine);
                        }

                        break;
                }

                currentState = state;

                // Activate new Coroutine
                if (ActiveStateInstance != null)
                {
                    StartCoroutine(ActiveStateInstance);
                }

                if (anim != null)
                {
                    // TALK ANIMATION STATE
                    if (TalkAnimations.TalkAnimationBools != "")
                    {
                        if (currentState == CreatureState.talking)
                        {
                            anim.SetBool(TalkAnimations.TalkAnimationBools, true);
                        }
                        else
                        {
                            anim.SetBool(TalkAnimations.TalkAnimationBools, false);
                        }
                    }

                    // IDLE ANIMATION STATE
                    if (IdleAnimations.IdleState != "")
                    {
                        if (currentState == CreatureState.patrolling || currentState == CreatureState.follow)
                        {
                            anim.SetBool(IdleAnimations.IdleState, true);
                        }
                        else
                        {
                            anim.SetBool(IdleAnimations.IdleState, false);
                        }
                    }
                    if (WalkingAnimation != "")
                    {
                        if (currentState == CreatureState.follow && thisNavMeshAgent.velocity.magnitude > 0.5f)
                        {
                            anim.SetBool(WalkingAnimation, true);
                        }
                        else
                        {
                            anim.SetBool(WalkingAnimation, false);
                        }
                    }
                }
            }

            yield return new WaitForSeconds(0.1f);
        }
    }

    //   _____ _______    _______ ______   ______ _    _ _   _  _____ _______ _____ ____  _   _  _____ 
    //  / ____|__   __|/\|__   __|  ____| |  ____| |  | | \ | |/ ____|__   __|_   _/ __ \| \ | |/ ____|
    // | (___    | |  /  \  | |  | |__    | |__  | |  | |  \| | |       | |    | || |  | |  \| | (___  
    // 	\___ \   | | / /\ \ | |  |  __|   |  __| | |  | | . ` | |       | |    | || |  | | . ` |\___ \ 
    // 	____) |  | |/ ____ \| |  | |____  | |    | |__| | |\  | |____   | |   _| || |__| | |\  |____) |
    // |_____/   |_/_/    \_\_|  |______| |_|     \____/|_| \_|\_____|  |_|  |_____\____/|_| \_|_____/ 

    IEnumerator Death()
    {
        isAlive = false;

        if (thisRigidbody != null)
        {
            thisRigidbody.useGravity = true;
        }
        yield return null;

        if (DestroyCreatureOnDeath)
        {
            Destroy(gameObject);
        }

        if (DeathAnimations.Enable)
        {
            OnDeathAnimation();
        }
        PlayerManager.Instance.ResetFocus();
        if (shouldDisableCollider)
        {
            GetComponent<Collider>().enabled = false;
        }

    }

    bool permissionToIdleAnim = false;
    IEnumerator IdleAnimationRoutine;
    public void AllowTriggeringOfIdleAnimation() //TODO: This shouldn't really be part of the Creature base class, as it's pretty specific to the Wwizard
    {
        permissionToIdleAnim = true;
    }

    IEnumerator TriggerIdleAnimation()
    {
        permissionToIdleAnim = true;
        while (state == CreatureState.patrolling)
        {
            if (anim != null && permissionToIdleAnim)
            {
                if (IdleAnimations.IdleAnimationName.Count > 0)
                {
                    int pickAnimation = Random.Range(0, IdleAnimations.IdleAnimationName.Count);
                    anim.SetTrigger(IdleAnimations.IdleAnimationName[pickAnimation]);
                    permissionToIdleAnim = false;
                }
            }
            float waitTime = Random.Range(IdleAnimations.RandomIdleTriggerTime.x, IdleAnimations.RandomIdleTriggerTime.y);
            yield return new WaitForSeconds(waitTime);
        }
        yield return null;
    }

    IEnumerator Patrolling()
    {
        if (IdleAnimations.IdleAnimationName.Count > 0)
        {
            if (IdleAnimationRoutine != null)
            {
                StopCoroutine(IdleAnimationRoutine);
            }
            IdleAnimationRoutine = TriggerIdleAnimation();
            StartCoroutine(IdleAnimationRoutine);
        }

        currentVisionRange = AgroRange;
        while (state == CreatureState.patrolling)
        {
            if (targetOfNPC != null)
            {
                if (!isFriendly)
                {
                    if (attackDespiteDay)
                    {
                        state = CreatureState.attacking;
                    }
                    else if (!daytime)
                    {
                        state = CreatureState.attacking;
                    }

                }
                else
                {

                    if (isFollowing)
                    {
                        state = CreatureState.follow;
                    }
                }
            }
            else
            {
                // RANDOM LOOK AROUND
                if (periodicallyRotate && thisNavMeshAgent != null && !thisNavMeshAgent.hasPath)
                {
                    Quaternion newRotation = Quaternion.Euler(0f, Random.Range(0f, 360f), 0f);

                    while (Quaternion.Angle(RotationObject.rotation, newRotation) > 10f)
                    {
                        if (targetOfNPC != null)
                        {
                            break;
                        }
                        RotationObject.rotation = Quaternion.Lerp(transform.rotation, newRotation, Time.deltaTime);
                        yield return null;
                    }
                }

                // RANDOM WALK AROUND
                if (CanMove && canRoam)
                {

                    if (ready2Roam && thisNavMeshAgent != null && !thisNavMeshAgent.hasPath)
                    {
                        isMoving = true;
                        Vector3 randomRadiusPosition = new Vector3(Random.Range(-currentVisionRange, currentVisionRange), 0f, Random.Range(-currentVisionRange, currentVisionRange));
                        thisNavMeshAgent.SetDestination(startPosition + randomRadiusPosition);
                        yield return new WaitUntil(() => thisNavMeshAgent.hasPath);
                        StartCoroutine(CanRoamWait());
                    }
                    else
                    {

                        if (thisNavMeshAgent != null && thisNavMeshAgent.hasPath && thisNavMeshAgent.remainingDistance < thisNavMeshAgent.stoppingDistance)
                        {
                            isMoving = false;
                            thisNavMeshAgent.ResetPath();
                        }
                    }
                }
            }
            yield return new WaitForSeconds(0.1f);
        }
        yield return null;
    }

    IEnumerator CanRoamWait()
    {
        ready2Roam = false;
        yield return new WaitForSeconds(RoamRapidness);
        ready2Roam = true;
    }

    public void SetTarget(GameObject sentTransform, bool setInstant)
    {
        targetOfNPC = sentTransform;
        OnSpotting();
        if (setInstant)
        {
            transform.LookAt(sentTransform.transform);
        }
    }


    public virtual IEnumerator Talking()
    {
        float interactionDistance = GetComponent<Pickup>().trigger.radius;
        while (state == CreatureState.talking)
        {
            if (targetOfNPC != null && Vector3.Distance(targetOfNPC.transform.position, this.transform.position) < interactionDistance)
            {
                Vector3 SameHeightTarget = new Vector3(targetOfNPC.transform.position.x, transform.position.y, targetOfNPC.transform.position.z);
                Quaternion desiredRotation = Quaternion.LookRotation(SameHeightTarget - transform.position);
                float angle = Quaternion.Angle(this.transform.rotation, desiredRotation);
                //print("Rotating towards: "+targetOfNPC.gameObject.name+" degrees left: "+angle);
                if (angle > 1f)
                {
                    transform.rotation = Quaternion.RotateTowards(this.transform.rotation, desiredRotation, Time.deltaTime * 200f);
                }
            }
            else
            {
                if (isFollowing)
                {
                    state = CreatureState.follow;
                }
                else
                {
                    if (targetOfNPC != null)
                    {
                        state = CreatureState.patrolling;
                    }
                }

            }

            yield return new WaitForSeconds(0.02f);
        }
    }

    public void TalkMode(bool condition)
    {
        if (condition)
        {
            state = CreatureState.talking;
        }
        else
        {
            if (state == CreatureState.talking)
            {
                state = CreatureState.patrolling;
            }

        }
    }

    IEnumerator Attacking()
    {
        if (thisNavMeshAgent != null)
        {
            thisNavMeshAgent.SetDestination(this.transform.position);
        }
        currentVisionRange = fullVisionRange;

        if(Health < 0)
        {
            state = CreatureState.dead;
            yield return null;
        }

        while (state == CreatureState.attacking)
        {
            if (targetOfNPC != null && !pausedAI && !GameManager.Instance.AIPaused)
            {
                if (CanMove)
                {
                    float distanceTo = Vector3.Distance(targetOfNPC.transform.position, this.transform.position);

                    if (isMelee && distanceTo < NavMeleeDistance && !hasAttacked)
                    {
                        if (thisNavMeshAgent != null && !thisNavMeshAgent.pathPending && thisNavMeshAgent.hasPath)
                        {
                            thisNavMeshAgent.ResetPath();
                        }
                        Anim_MeleeAttack();

                        isMoving = false;
                        Quaternion newRotation = Quaternion.LookRotation(targetOfNPC.transform.position - transform.position);
                        RotationObject.rotation = Quaternion.Lerp(transform.rotation, newRotation, Time.deltaTime * NoNavRotationSpeed);
                        thisNavMeshAgent.SetDestination(targetOfNPC.transform.position);
                    }
                    else if (isRanged && distanceTo < NavRangedDistance && !hasAttacked)
                    {
                        if (thisNavMeshAgent != null && !thisNavMeshAgent.pathPending && thisNavMeshAgent.hasPath)
                        {
                            thisNavMeshAgent.ResetPath();
                        }
                        Anim_RangedAttack();
                        isMoving = false;
                    }
                    else if (!hasAttacked)
                    {
                        Move(this.transform.position, targetOfNPC.transform.position);
                        isMoving = true;
                    }
                }
                else
                {
                    Vector3 direction = (targetOfNPC.transform.position - transform.position).normalized;
                    float angleOfTarget = Vector3.Angle(direction, RotationObject.transform.forward);

                    if (HasShootingAngle)
                    {
                        if (angleOfTarget < maxShootAngle && !hasAttacked)
                        {
                            Anim_RangedAttack();
                        }
                        else
                        {
                            Move(this.transform.position, targetOfNPC.transform.position);
                        }
                    }
                    else
                    {
                        
                        if (angleOfTarget < 1f && !hasAttacked)
                        {
                           
                            RotationObject.rotation = Quaternion.LookRotation(targetOfNPC.transform.position - transform.position);
                            Anim_RangedAttack();
                        }
                        else
                        {
                            Move(this.transform.position, targetOfNPC.transform.position);
                        }
                    }
                }
            }
            else
            {
                if (targetOfNPC == null)
                    state = CreatureState.patrolling;
            }

            yield return null;
        }
        yield return null;
    }

    public virtual void Anim_MeleeAttack()
    {
        if (AttackAnimation.Enable && AttackAnimation.DefaultMelee != "" && PlayerManager.Instance.cameraScript.cameraMode != PlayerCamera.CameraMode.CamEvent)
        {
            anim.SetTrigger(AttackAnimation.DefaultMelee);
            StartCoroutine(AllowAttackIn(PastAttackDelay));
        }
    }

    public virtual void Anim_RangedAttack()
    {
        if (AttackAnimation.Enable && AttackAnimation.DefaultRanged != "" && PlayerManager.Instance.cameraScript.cameraMode != PlayerCamera.CameraMode.CamEvent && (!GameManager.InstanceIsNull() && !GameManager.Instance.AIPaused))
        {
            anim.SetTrigger(AttackAnimation.DefaultRanged);
            StartCoroutine(AllowAttackIn(PastAttackDelay));
        }
    }

    IEnumerator Following()
    {
        currentVisionRange = fullVisionRange;
        while (state == CreatureState.follow)
        {
            if (targetOfNPC != null)
            {
                Move(this.transform.position, targetOfNPC.transform.position);

                if (anim != null && WalkingAnimation != "")
                {
                    if (currentState == CreatureState.follow && thisNavMeshAgent.velocity.magnitude > 0.5f)
                    {
                        anim.SetBool(WalkingAnimation, true);
                    }
                    else
                    {
                        anim.SetBool(WalkingAnimation, false);
                    }
                }
            }
            else
            {
                state = CreatureState.patrolling;
            }

            yield return new WaitForSeconds(0.1f);
        }

        yield return null;
    }

    IEnumerator Stunned()
    {
        if (StunParticles != null)
        {
            GameObject StunP = (Instantiate(StunParticles, transform.position + (Vector3.up * 1.4f), Quaternion.identity)) as GameObject;
            Destroy(StunP, StunDuration);
        }
        NavMeshAgentMovement(false);
        yield return new WaitForSeconds(StunDuration);
        NavMeshAgentMovement(true);

        state = ReturnState;
    }

    void NavMeshAgentMovement(bool condition)
    {
        if (thisNavMeshAgent != null)
        {
            if (condition)
            {
                thisNavMeshAgent.Warp(this.transform.position);
            }

            thisNavMeshAgent.updatePosition = condition;
            thisNavMeshAgent.updatePosition = condition;

        }
    }

    IEnumerator Immobalized()
    {
        NavMeshAgentMovement(false);
        yield return new WaitForSeconds(ImmobilizedDuration);
        NavMeshAgentMovement(true);

        state = ReturnState;
    }

    IEnumerator SpotAttackDelay()
    {
        hasAttacked = true;

        float counter = 0f;
        while (counter < OnSpottingAttackDelay)
        {
            counter += Time.deltaTime;
            yield return null;
        }
        counter = OnSpottingAttackDelay;

        hasAttacked = false;

    }

    public virtual void Move(Vector3 yourPosition, Vector3 targetPosition)
    {
       
        if (state != CreatureState.stunned)
        {
            if (thisNavMeshAgent != null && !IsImmobilized && CanMove)
            {
                thisNavMeshAgent.SetDestination(targetPosition);
                if (!thisNavMeshAgent.updateRotation)
                {
                    Quaternion newRotation = Quaternion.LookRotation(targetOfNPC.transform.position - transform.position);
                    RotationObject.rotation = Quaternion.Lerp(transform.rotation, newRotation, Time.deltaTime * NoNavRotationSpeed);
                }
            }
            else
            {
                Vector3 targetPos = lockYRotation ? targetOfNPC.transform.position.WithY(transform.position.y) : targetOfNPC.transform.position;
                Quaternion newRotation = Quaternion.LookRotation(targetPos);
                RotationObject.rotation = Quaternion.Lerp(transform.rotation, newRotation, Time.deltaTime * NoNavRotationSpeed);
                /*
                 TODO: Okay so this is why the AI (at least on the spitplant) breaks if you put a Transform that is also animated by an Animator. Rotation stuff has to be done in LateUpdate!
                */
            }
        }
    }

    IEnumerator AllowAttackIn(float duration)
    {
        hasAttacked = true;
        yield return new WaitForSeconds(duration);
        hasAttacked = false;
    }

    public virtual void OnSpotting()
    {
        StartCoroutine(SpotAttackDelay());
    }
    public virtual void OffSpotting() { }
    public virtual void OnDeathAnimation() { }
    public virtual void OnInterrupt() { }

    public enum WeaponColliderStates { Enable, Disable }

    public virtual void SetWeaponColliderActive(WeaponColliderStates condition)
    {
        if (condition == WeaponColliderStates.Enable)
        {
            weapon.EnableHitbox();
        }
        else
        {
            weapon.DisableHitbox();
        }
    }
    // WHEN THIS CREATURE TAKES DAMAGE
    public virtual void OnDamage(Attack a)
    {
        LastAttack = a;
        hasAttacked = false;

        // KNOCKBACK
        if (KnockBackAnimation.Enable)
        {
            if (previousSwing != "")
            {
                anim.ResetTrigger(previousSwing);
            }

            Vector3 directionToTarget = a.attackDir;
            float angel = Vector3.Angle(RotationObject.transform.forward, directionToTarget);

            string AnimationName = "";

            if (Mathf.Abs(angel) > 90)
            {
                if (a.swingType == SwingTypes.Right)
                {
                    //print("infront + right");
                    AnimationName = KnockBackAnimation.Right;
                }
                else if (a.swingType == SwingTypes.Left)
                {
                    //print("infront + left");
                    AnimationName = KnockBackAnimation.Left;
                }
                else if (a.swingType == SwingTypes.Top)
                {
                    //print("infront + top");
                    AnimationName = KnockBackAnimation.TopFront;
                }

            }
            else
            {
                if (a.swingType == SwingTypes.Right)
                {
                    //print("behind + right");
                    AnimationName = KnockBackAnimation.Left;
                }
                else if (a.swingType == SwingTypes.Left)
                {
                    //print("behind + left");
                    AnimationName = KnockBackAnimation.Right;
                }
                else if (a.swingType == SwingTypes.Top)
                {
                    //print("behind + top");
                    AnimationName = KnockBackAnimation.TopBehind;
                }
            }

            if (AnimationName != "")
            {
                anim.SetTrigger(AnimationName);
            }
            else
            {
                //print(AnimationName);
                anim.SetTrigger(KnockBackAnimation.Default);
            }

            previousSwing = AnimationName;
        }

        // STUN
        if (CanBeStunned)
        {
            if (CanKnockBack)
            {
                if (thisRigidbody != null)
                {
                    NavMeshAgentMovement(false);
                    thisRigidbody.AddForce(a.attackDir * a.knockbackStrength / 10, ForceMode.Impulse);
                }
            }
            if (state == CreatureState.stunned)
            {
                ReturnState = CreatureState.patrolling;
            }
            else
            {
                ReturnState = currentState;
            }

            state = CreatureState.stunned;
        }

        // LOOSE HEALTH
        if (!isFriendly && canTakeDamage)
        {
            Health -= a.damage;
            if (Health <= 0f && isAlive)
            {
                isAlive = false;
                state = CreatureState.dead;

                if (PlayDeathSoundOnDeath)
                {
                    PlayCreatureDeathSound();
                }

                if (OnCreatureDeath != null)
                {
                    OnCreatureDeath();
                }
            }
            else
            {
                HurtSound.Post(gameObject);
                OnDamageReset();
            }
        }
    }

    public virtual void OnDamageReset()
    {

    }

    public bool CanBeAttacked() {
        if (currentState != CreatureState.dead)
        {
            return true;
        }
        else {
            return false;
        }
    }

    protected virtual void PlayCreatureDeathSound()
    {
        DeathSound.Post(gameObject);
    }

    /// <summary>
    /// The awareness of the NPC. How the NPC spots and looks for things.  
    /// </summary>
    public virtual IEnumerator Awareness()
    {
        while (true)
        {
            float sqrDistanceToPlayer = (PlayerManager.Instance.playerTransform.position - transform.position).sqrMagnitude;
            if(sqrDistanceToPlayer < currentVisionRange * currentVisionRange && PlayerManager.Instance.isAlive)
            {
                if (targetOfNPC == null)
                {
                    targetOfNPC = PlayerManager.Instance.player;
                    OnSpotting();
                }
            }else
            {
                if (targetOfNPC != null)
                {
                    targetOfNPC = null;
                    OffSpotting();
                }
            }
            yield return new WaitForSeconds(spotSpeed);
        }
    }

    public virtual void PauseAI()
    {
        pausedAI = true;
    }

    public virtual void ResumeAI()
    {
        pausedAI = false;
    }

    public enum DeathCondition
    {
        Drop, SetInactive
    }

    private void OnDisable()
    {
        if (!GameManager.InstanceIsNull())
        {
            GameManager.Instance.RemoveFromCombat(this);
        }
    }

    [System.Serializable]
    public class KnockBackAnimations
    {
        public bool Enable = false;
        [ShowIf("KnockBackAnimation.Enable", true)] public string Left = "";
        [ShowIf("KnockBackAnimation.Enable", true)] public string Right = "";
        [ShowIf("KnockBackAnimation.Enable", true)] public string TopFront = "";
        [ShowIf("KnockBackAnimation.Enable", true)] public string TopBehind = "";
        [ShowIf("KnockBackAnimation.Enable", true)] public string Default = "";
    }

    [System.Serializable]
    public class AttackAnimations
    {
        public bool Enable = false;
        [ShowIf("AttackAnimation.Enable", true)] public string DefaultMelee;
        [ShowIf("AttackAnimation.Enable", true)] public string DefaultRanged;
    }

    [System.Serializable]
    public class DeathAnimation
    {
        public bool Enable = false;
        [ShowIf("DeathAnimations.Enable", true)] public string FrontTrigger = "";
        [ShowIf("DeathAnimations.Enable", true)] public string BehindTrigger = "";
    }

    [System.Serializable]
    public class IdleAnimation
    {
        public string IdleState = "";
        public Vector2 RandomIdleTriggerTime;
        public List<string> IdleAnimationName;
    }

    [System.Serializable]
    public class TalkAnimation
    {
        public string TalkAnimationBools = "";
        public float weight = 1f;
    }

    public enum CreatureState
    {
        patrolling, attacking, dead, stunned, immobilized, follow, talking, none
    };

    [System.Serializable]
    public class DebugOption
    {
        public bool RaycastVisionRange = false;
    }
}
