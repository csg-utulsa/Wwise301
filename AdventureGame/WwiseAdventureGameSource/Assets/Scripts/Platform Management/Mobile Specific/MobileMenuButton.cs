////////////////////////////////////////////////////////////////////////
//
// Copyright (c) 2018 Audiokinetic Inc. / All Rights Reserved
//
////////////////////////////////////////////////////////////////////////

﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobileMenuButton : MonoBehaviour {

    public Menu MainMenu;

    public void ToggleMenu(){
        MainMenu.ToggleMenu();
    }
}
