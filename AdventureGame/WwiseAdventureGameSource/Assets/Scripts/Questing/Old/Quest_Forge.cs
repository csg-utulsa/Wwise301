////////////////////////////////////////////////////////////////////////
//
// Copyright (c) 2018 Audiokinetic Inc. / All Rights Reserved
//
////////////////////////////////////////////////////////////////////////

﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace QuestSystem
{
    public class Quest_Forge : QuestObjectivePreparer
    {
        [Header("During Waves")]
        public List<WaveLines> Waves;

        [Header("Wwizard Info")]
        public WwizardAI Wwizard;
        public Transform WwizardSpot;
        public float WwizardPreperationDuration = 4f;

        [Header("Objective Info")]
        public Transform TheCore;
        public DefaultSpellcraft SpellScript;
        public RuneTrigger Rune;
        public Color CrystalReadyColor;
        public MeshRenderer RuneMat;
        ParticleSystem CrystalParticles;
        public GameObject EnemySpawnVFX;

        #region private variables
        private int currentWave = -1;
        private IEnumerator sequenceRoutine;
        private List<Creature> spawnedEnemies = new List<Creature>();
        #endregion

        private void Awake()
        {
            if (SpellScript != null) {
                SpellScript.SetTarget(TheCore.position);
            }

            GameManager.Instance.Quest_ForgeSystem = this;
            CrystalParticles = RuneMat.gameObject.GetComponent<ParticleSystem>();
        }

        public override void PrepareObjective()
        {
            sequenceRoutine = DefeatTheCoreSequence();
            StartCoroutine(sequenceRoutine);

            PreparationDone();
        }

        public override void ReversePreparations()
        {
            currentWave = -1;

            if (sequenceRoutine != null)
            {
                StopCoroutine(sequenceRoutine);
                Wwizard_Reset();
                Player_Reset();
                Rune_Reset();
                Spawner_Stop();
            }

            for (int i = spawnedEnemies.Count - 1; i >= 0; i--)
            {
                Creature currentEnemy = spawnedEnemies[i];

                if (currentEnemy != null) {
                    Destroy(currentEnemy.gameObject);
                }
                spawnedEnemies.RemoveAt(i);
            }

            PreparationReversed();
        }

        IEnumerator DefeatTheCoreSequence()
        {
            //// The core was attacked, and things start to spawn.
            Wwizard_ChangeRuneMode();
            Rune_Prepare();

            // WAVE 1
            yield return Wave(0);

            // WAVE 2
            yield return Wave(1);

            // WAVE 3
            yield return Wave(2);
            Wwizard_Reset();
        }


        IEnumerator Wave(int waveNo)
        {
            Spawner_Start(waveNo);

            Wwizard.transform.rotation = WwizardSpot.rotation;

            currentWave = waveNo;

            // Send dialogue
            if (Waves[waveNo].PreChargeDialogue.Count > 0)
            {
                DialogueManager.Instance.TransferDialogue(Waves[waveNo].PreChargeDialogue, Wwizard.gameObject);
            }

            // Spawn enemies
            yield return new WaitForSeconds(Waves[waveNo].WaitBeforeSpawnDuration);

            for (int i = 0; i < Waves[waveNo].EnemySpawns.Count; i++)
            {
                spawnedEnemies.Add(SpawnEnemy(waveNo, i));
                yield return new WaitForSeconds(0.5f);
            }

            // Wait for Wwizard charging
            yield return StartCoroutine(ChargeSpell(true));
            Rune_Ready();

            // Send post-charge dialogue
            if (Waves[waveNo].ChargeDoneDialogue.Count > 0)
            {
                DialogueManager.Instance.TransferDialogue(Waves[waveNo].ChargeDoneDialogue, Wwizard.gameObject);
            }

            // Wait until the shield has been destroyed
            yield return new WaitUntil(() => Waves[waveNo].hasBeenDestroyed);
            Player_Reset();
            Rune_Reset();
            Spawner_Stop();
        }

        void Player_Reset() {
            PlayerManager.Instance.Player_Reset();
        }

        IEnumerator RespawnerRoutine;
        IEnumerator Respawner(int waveNo)
        {
            while (true)
            {

                AllowedToSpawn = false;
                while (AllowedToSpawn == false)
                {
                    yield return null;
                }

                //SPAWN
                spawnedEnemies.Add(SpawnRandomEnemy(waveNo));
            }
        }


        IEnumerator ChargeSpell(bool chargeIN)
        {
            if (chargeIN)
            {
                Wwizard.TriggerAnimation_Charge();
                for (float t = 0; t < 1; t += Time.deltaTime / WwizardPreperationDuration)
                {
                    RuneMat.material.SetColor("_EmissionColor", CrystalReadyColor * t);
                    yield return null;
                }
                RuneMat.material.SetColor("_EmissionColor", CrystalReadyColor);
                Wwizard.TriggerAnimation_ChargeEnd();
            }
            else
            {
                for (float t = 1; t > 0; t -= Time.deltaTime / 0.5f)
                {
                    Color newC = Color.black;
                    newC.r = t;
                    newC.b = t;
                    newC.g = t;
                    RuneMat.material.SetColor("_EmissionColor", newC);
                    yield return null;
                }
                RuneMat.material.SetColor("_EmissionColor", Color.black);
            }
        }

        public void NextWave(int c)
        {
            Waves[c].hasBeenDestroyed = true;
            Rune.SetHelptextVisibility(false);
        }

        private Creature SpawnRandomEnemy(int waveNumber)
        {
            int RandomEnemy = Random.Range(0, Waves[waveNumber].EnemySpawns.Count-1);
            Vector3 position = Waves[waveNumber].EnemySpawns[RandomEnemy].SpawnPosition.transform.position;
            GameObject enemy = Instantiate(Waves[waveNumber].EnemySpawns[RandomEnemy].EnemySpawn, position, Quaternion.identity) as GameObject;
            enemy.transform.parent = TheCore.transform;

            // Instantly set the target to the player
            Creature creature = enemy.GetComponent<Creature>();
            creature.SetTarget(PlayerManager.Instance.player, true);

            // Spawn particles
            if (EnemySpawnVFX != null)
            {
                GameObject VFX = Instantiate(EnemySpawnVFX, position, Quaternion.identity) as GameObject;
                Destroy(VFX, 5f);
            }

            return creature;
        }

        private Creature SpawnEnemy(int waveNumber, int enemyNumber)
        {
            Vector3 position = Waves[waveNumber].EnemySpawns[enemyNumber].SpawnPosition.transform.position;
            GameObject enemy = Instantiate(Waves[waveNumber].EnemySpawns[enemyNumber].EnemySpawn, position, Quaternion.identity) as GameObject;
            enemy.transform.parent = TheCore.transform;

            // Instantly set the target to the player
            Creature creature = enemy.GetComponent<Creature>();
            creature.SetTarget(PlayerManager.Instance.player, true);

            // Spawn particles
            if (EnemySpawnVFX != null)
            {
                GameObject VFX = Instantiate(EnemySpawnVFX, position, Quaternion.identity) as GameObject;
                Destroy(VFX, 5f);
            }

            return creature;
        }

        public int GetCurrentWave()
        {
            return currentWave;
        }

        void Rune_Prepare()
        {
            Rune.SetHelptextVisibility(false);
            Rune.RuneStatus(false);
        }

        void Rune_Ready()
        {
            CrystalParticles.Play();
            Rune.RuneStatus(true);
            Rune.SetHelptextVisibility(true);
        }

        void Rune_Reset()
        {
            StartCoroutine(ChargeSpell(false));
            CrystalParticles.Stop();
            Rune.RuneStatus(false);
            Rune.SetHelptextVisibility(false);
        }

        void Wwizard_ChangeRuneMode()
        {
            Wwizard.DisableNavMeshAgent();
        }

        void Wwizard_Reset()
        {
            Wwizard.EnableNavMeshAgent();
            SpellScript.DisableMagic();
        }

        bool AllowedToSpawn = false;

        public void AllowEnemiesToSpawn()
        {
            if (GameManager.Instance.Quest_ForgeSystem != null)
            {
                AllowedToSpawn = true;
            }
        }

        void Spawner_Start(int waveNo)
        {
            if (RespawnerRoutine == null)
            {
                RespawnerRoutine = Respawner(waveNo);
                StartCoroutine(RespawnerRoutine);
            }
        }

        void Spawner_Stop()
        {
            if (RespawnerRoutine != null)
            {
                StopCoroutine(RespawnerRoutine);
                RespawnerRoutine = null;
            }
        }

        [System.Serializable]
        public class Spawn
        {
            public GameObject EnemySpawn;
            public GameObject SpawnPosition;
        }

        [System.Serializable]
        public class WaveLines
        {
            public List<DialogueLine> PreChargeDialogue;
            public List<DialogueLine> ChargeDoneDialogue;
            public float WaitBeforeSpawnDuration = 1f;
            public List<Spawn> EnemySpawns;
            public bool hasBeenDestroyed;
        }

    }
}