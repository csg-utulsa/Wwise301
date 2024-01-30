////////////////////////////////////////////////////////////////////////
//
// Copyright (c) 2018 Audiokinetic Inc. / All Rights Reserved
//
////////////////////////////////////////////////////////////////////////

using UnityEngine;
using System.Collections;

public class EvilSpitPlantAI : Creature
{
    [Header("Custom Creature Options:")]
    public GameObject bulletPrefab;
    public GameObject chargeParticles;
    public GameObject shootParticles;
    public GameObject spitBulletSpawnPoint;

    #region private variables
    private bool hasSpawned = false;
    private bool lockRotation = false;
    private readonly int spawnHash = Animator.StringToHash("Spawn");
    private readonly int despawnHash = Animator.StringToHash("Despawn");
    private readonly int isAliveHash = Animator.StringToHash("IsAlive");
    #endregion

    [Header("WWISE")]
    public AK.Wwise.Event AttackSound = new AK.Wwise.Event();
    public AK.Wwise.Event ChargeSound = new AK.Wwise.Event();
    public AK.Wwise.Event Death_Headfall = new AK.Wwise.Event();
    public AK.Wwise.Event asdasdasfasda;

    public override void OnSpotting()
    {
        base.OnSpotting();

        if (!hasSpawned)
        {
            anim.SetTrigger(spawnHash);
            hasSpawned = true;
        }
    }

    public override void OffSpotting()
    {
        base.OffSpotting();

        if (hasSpawned)
        {
            anim.SetTrigger(despawnHash);
            hasSpawned = false;
        }
    }

    /// <summary>
    /// Called from Animation Event. This shoots the projectile!
    /// </summary>
    public void Shoot()
    {
        if (targetOfNPC != null && !GameManager.Instance.AIPaused)
        {
            AttackSound.Post(this.gameObject);

            GameObject bullet = Instantiate(bulletPrefab, spitBulletSpawnPoint.transform.position, Quaternion.LookRotation(transform.forward)) as GameObject; //TODO: Pool spitbullets
            bullet.GetComponent<EvilSpitPlantProjectile>().parent = gameObject;
            bullet.GetComponent<EvilSpitPlantProjectile>().damage = this.AttackDamage;

            GameObject bulletSpawnFX = Instantiate(shootParticles, spitBulletSpawnPoint.transform.position, Quaternion.identity, spitBulletSpawnPoint.transform) as GameObject; //TODO: Pool spitbullet spawn particles
            Destroy(bulletSpawnFX, 5f);
        }
    }

    public void PlayChargeSound()
    {
        ChargeSound.Post(gameObject);
    }

    /// <summary>
    /// Called from Animation Event. This happens when the Evil Spit Plant telegraphs its attack!
    /// </summary>
    public void ChargeUp()
    {
        if (chargeParticles != null)
        {
            GameObject chargeFX = Instantiate(chargeParticles, spitBulletSpawnPoint.transform.position, Quaternion.identity, spitBulletSpawnPoint.transform) as GameObject; //TODO: Pool charge particles.
            Destroy(chargeFX, 5f);
        }
    }

    public override void Move(Vector3 yourPosition, Vector3 targetPosition)
    {
        if (!lockRotation)
        {
            Quaternion newRotation = Quaternion.LookRotation(targetOfNPC.transform.position - transform.position);
            RotationObject.rotation = Quaternion.Lerp(transform.rotation, newRotation, Time.deltaTime * NoNavRotationSpeed);
        }
    }

    public override void OnDamageReset()
    {
        base.OnDamageReset();
        lockRotation = false;
    }

    /// <summary>
    /// Called from Animation Event.
    /// </summary>
    public void LockRotation()
    {
        lockRotation = true;
    }

    /// <summary>
    /// Called from Animation Event. This happens when the Evil Head telegraphs its attack!
    /// </summary>
    public void UnlockRotation()
    {
        lockRotation = false;
    }

    public override void OnDeathAnimation()
    {
        base.OnDeathAnimation();

        anim.SetBool(isAliveHash, false);

        float angle = Vector3.Angle(RotationObject.transform.forward, LastAttack.attackDir);
        if (Mathf.Abs(angle) > 90)
        {
            anim.SetTrigger(DeathAnimations.FrontTrigger);
        }
        else
        {
            anim.SetTrigger(DeathAnimations.BehindTrigger);
        }
        LockRotation();
    }

    public void OnDeathHeadFall()
    {
        Death_Headfall.Post(this.gameObject);
    }
}
