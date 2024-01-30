////////////////////////////////////////////////////////////////////////
//
// Copyright (c) 2018 Audiokinetic Inc. / All Rights Reserved
//
////////////////////////////////////////////////////////////////////////

﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Linq;

public class WorldTeleporter : MonoBehaviour
{
    //TODO: Custom inspector with EditorGUI.HelpBox("Tag all teleport destinations with the tag 'TeleportDestination'");
    private TeleportDestination[] destinations;

    [Header("Wwise")]
    public AK.Wwise.Event TeleportSelectSound;

    [Header("UI Objects")]
    public Dropdown dropdown;
    public string destinationKeyPrefix = "dropdown_";

    [Header("VFX")]
    public GameObject teleportParticles;
    public UnityEvent OnTeleport;

    private UnityAction<int> teleport;

    private void Awake()
    {
        LanguageManager.OnLanguageChange += ConfigureTeleportLocations;
        Menu.OnMenuStateChange += OnMenuOpen;
        dropdown.value = 0;
    }

    private void OnDestroy()
    {
        LanguageManager.OnLanguageChange -= ConfigureTeleportLocations;
    }

    void OnEnable()
    {
        ConfigureTeleportLocations();
    }

    private void OnMenuOpen(bool state) {
        if (state) { // If menu is opened - Set caption text. 
            dropdown.captionText.text = LanguageManager.GetText("menu_teleport");
        }
    }

    private void ConfigureTeleportLocations()
    {
        dropdown.options = new List<Dropdown.OptionData>();

        destinations = FindObjectsOfType<TeleportDestination>();

        if (destinations.Length > 0)
        {
            //Sort by Teleport Position override 
            destinations = destinations.OrderBy(x => x.ListPositionOverride).ToArray();

            dropdown.options.Add(new Dropdown.OptionData("Cancel..."));

            for (int i = 0; i < destinations.Length; i++)
            {
                string optionLabel = LanguageManager.GetText(destinations[i].Key);
                Dropdown.OptionData option = new Dropdown.OptionData(optionLabel);
                dropdown.options.Add(option);
            }
            
            dropdown.captionText.text = LanguageManager.GetText("menu_teleport");
        }
        else
        {
            dropdown.options.Add(new Dropdown.OptionData("N/A"));
        }
    }

    public void Teleport()
    {
        dropdown.Hide();

        if (dropdown.value != 0)
        {
            PlayerManager.Instance.player.transform.position = destinations[dropdown.value - 1].transform.position;
            TeleportSelectSound.Post(PlayerManager.Instance.player);
            dropdown.value = 0;
            dropdown.captionText.text = LanguageManager.GetText("menu_teleport");

            OnTeleport.Invoke();

            //Spawn particles at the new position
            if (teleportParticles != null)
            {
                GameObject go = Instantiate(teleportParticles, PlayerManager.Instance.player.transform.position, Quaternion.identity) as GameObject;
                Destroy(go, 5f);
            }
        }
    }
}
