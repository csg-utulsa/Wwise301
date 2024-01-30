using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneManagement;

public class CertificateMenu : EditorWindow{

    public static string SceneFolder = "Assets/Scenes/Game Scenes/";
    public static string Cert251_SceneFolder = "Assets/Scenes/Game Scenes/Certificate Scenes/Cert-251/";
    public static string Cert301_SceneFolder = "Assets/Scenes/Game Scenes/Certificate Scenes/Cert-301/";
    public static string AudioEvn_SceneFolder = "Assets/Scenes/Game Scenes/Audio Environment Scenes/"; 
    public static string UnityType = ".unity";
    public const string NameOfMenu = "Audiokinetic";
    public const int priority_gameScenes = 2;
    public const int priority_website = 1337; 
    public const int priority_certification = 3;

    public static void AdjustCamera()
    {
        GameObject obj = GameObject.Find("OpeningSceneView");
        if (obj != null)
        {
            SceneView.lastActiveSceneView.AlignViewToObject(obj.transform);
        }
    }

[MenuItem(NameOfMenu+"/Website",false, priority_website)]
    public static void OpenScene_AK()
    {
        Application.OpenURL("https://www.audiokinetic.com/");
    }

    [MenuItem(NameOfMenu + "/Certification/Certification Page", false, priority_certification)]
    public static void OpenScene_CertificatePage()
    {
        Application.OpenURL("https://www.audiokinetic.com/learn/certifications/");
    }

    [MenuItem(NameOfMenu + "/Certification/Audiokinetic Questions and Answers", false, priority_certification)]
    public static void OpenScene_QAPage()
    {
        Application.OpenURL("https://www.audiokinetic.com/qa/");
    }

    [MenuItem(NameOfMenu + "/Game Scenes/Main Only", false, priority_gameScenes)]
    public static void OpenScene_MainScene()
    {
        EditorSceneManager.OpenScene(SceneFolder + "Main Scene.unity");
        AdjustCamera();
    }

    [MenuItem(NameOfMenu + "/Game Scenes/Title Screen Only", false, priority_gameScenes)]
    public static void OpenScene_TitleScreen()
    {
        EditorSceneManager.OpenScene(SceneFolder + "Title Screen.unity");
        AdjustCamera();
    }
    [MenuItem(NameOfMenu + "/Game Scenes/Credits Only", false, priority_gameScenes)]
    public static void OpenScene_Credits()
    {
        EditorSceneManager.OpenScene(SceneFolder + "Credits.unity");
        AdjustCamera();
    }
    [MenuItem(NameOfMenu + "/Game Scenes/Add Environment Scenes", false, priority_gameScenes)]
    public static void OpenScene_MainSceneEnv()
    {
        EditorSceneManager.OpenScene(SceneFolder + "Audio Environment Scenes/Cave Audio Environment.unity", OpenSceneMode.Additive);
        EditorSceneManager.OpenScene(SceneFolder + "Audio Environment Scenes/Desert Audio Environment.unity", OpenSceneMode.Additive);
        EditorSceneManager.OpenScene(SceneFolder + "Audio Environment Scenes/Dungeon Audio Environment.unity", OpenSceneMode.Additive);
        EditorSceneManager.OpenScene(SceneFolder + "Audio Environment Scenes/Pine Forest Audio Environment.unity", OpenSceneMode.Additive);
        EditorSceneManager.OpenScene(SceneFolder + "Audio Environment Scenes/Village Audio Environment.unity", OpenSceneMode.Additive);
        EditorSceneManager.OpenScene(SceneFolder + "Audio Environment Scenes/Woodlands Audio Environment.unity", OpenSceneMode.Additive);
    }
    [MenuItem(NameOfMenu + "/Game Scenes/Add Dungeon Scene", false, priority_gameScenes)]
    public static void OpenScene_Dungeon()
    {
        EditorSceneManager.OpenScene(SceneFolder + "Dungeon Scene.unity", OpenSceneMode.Additive);
    }
    [MenuItem(NameOfMenu + "/Game Scenes/Add Dungeon Scene (w. Spatial Audio)", false, priority_gameScenes)]
    public static void OpenScene_DungeonSpatial()
    {
        EditorSceneManager.OpenScene(SceneFolder + "Dungeon Scene - Spatial Audio.unity", OpenSceneMode.Additive);
    }

    [MenuItem(NameOfMenu + "/Game Scenes/Add Secret Room", false, priority_gameScenes)]
    public static void OpenScene_SecretRoom()
    {
        EditorSceneManager.OpenScene(SceneFolder + "Secret Room.unity", OpenSceneMode.Additive);
    }

    [MenuItem(NameOfMenu + "/Certification/301/Lesson 1/Loading a SoundBank", false, priority_certification)]
    public static void OpenScene_L1_1()
    {
        EditorSceneManager.OpenScene(Cert301_SceneFolder + "L1_1 - Loading a SoundBank.unity");
        AdjustCamera();
    }
    [MenuItem(NameOfMenu + "/Certification/301/Lesson 1/Playing your first Wwise Event", false, priority_certification)]
    public static void OpenScene_L1_2()
    {
        EditorSceneManager.OpenScene(Cert301_SceneFolder + "L1_2 - Playing your first Wwise Event.unity");
        AdjustCamera();
    }
    [MenuItem(NameOfMenu + "/Certification/301/Lesson 1/Trigger Conditions", false, priority_certification)]
    public static void OpenScene_L1_3()
    {
        EditorSceneManager.OpenScene(Cert301_SceneFolder + "L1_3 - Trigger Conditions.unity");
        AdjustCamera();
    }
    [MenuItem(NameOfMenu + "/Certification/301/Lesson 2/Posting an Ambience", false, priority_certification)]
    public static void OpenScene_L2_1()
    {
        EditorSceneManager.OpenScene(Cert301_SceneFolder + "L2_1 - Posting an Ambience.unity");
        EditorSceneManager.OpenScene(Cert301_SceneFolder + "L2_1B - Village Environment Scene.unity", OpenSceneMode.Additive);
        AdjustCamera();
    }
    [MenuItem(NameOfMenu + "/Certification/301/Lesson 2/Attenuation Spheres", false, priority_certification)]
    public static void OpenScene_L2_2()
    {
        EditorSceneManager.OpenScene(Cert301_SceneFolder + "L2_2 - Attenuation Spheres.unity");
        EditorSceneManager.OpenScene(Cert301_SceneFolder + "L2_2B - Village Environment Scene.unity", OpenSceneMode.Additive);
        AdjustCamera();
    }
    [MenuItem(NameOfMenu + "/Certification/301/Lesson 2/AkAmbient Position Types", false, priority_certification)]
    public static void OpenScene_L2_3()
    {
        EditorSceneManager.OpenScene(Cert301_SceneFolder + "L2_3 - AkAmbient Position Types.unity");
        EditorSceneManager.OpenScene(SceneFolder + "Dungeon Scene.unity", OpenSceneMode.Additive);
        EditorSceneManager.OpenScene(Cert301_SceneFolder + "L2_3B - Dungeon Environment Scene.unity", OpenSceneMode.Additive);
        AdjustCamera();
    }
    [MenuItem(NameOfMenu + "/Certification/301/Lesson 2/EventPositionConfiner", false, priority_certification)]
    public static void OpenScene_L2_4()
    {
        EditorSceneManager.OpenScene(Cert301_SceneFolder + "L2_4 - EventPositionConfiner.unity");
        EditorSceneManager.OpenScene(Cert301_SceneFolder + "L2_4B - Village Environment Scene.unity", OpenSceneMode.Additive);
        AdjustCamera();
    }

    [MenuItem(NameOfMenu + "/Certification/301/Lesson 3/Loading SoundBanks using Triggers", false, priority_certification)]
    public static void OpenScene_L3_1()
    {
        EditorSceneManager.OpenScene(Cert301_SceneFolder + "L3_1 - Loading SoundBanks using Triggers.unity");
        AdjustCamera();
    }
    [MenuItem(NameOfMenu + "/Certification/301/Lesson 3/Decompressing SoundBanks", false, priority_certification)]
    public static void OpenScene_L3_2()
    {
        EditorSceneManager.OpenScene(Cert301_SceneFolder + "L3_2 - Decompressing SoundBanks.unity");
        EditorSceneManager.OpenScene(Cert301_SceneFolder + "L3_2 - Dungeon Audio Environment.unity", OpenSceneMode.Additive);
        AdjustCamera();
    }
    [MenuItem(NameOfMenu + "/Certification/301/Lesson 3/Saving a Decoded SoundBank", false, priority_certification)]
    public static void OpenScene_L3_3()
    {
        EditorSceneManager.OpenScene(Cert301_SceneFolder + "L3_3 - Saving a Decoded SoundBank.unity");
        EditorSceneManager.OpenScene(SceneFolder + "Dungeon Scene.unity", OpenSceneMode.Additive);
        EditorSceneManager.OpenScene(Cert301_SceneFolder + "L3_3 - Dungeon Audio Environment.unity", OpenSceneMode.Additive);
        AdjustCamera();
    }

    [MenuItem(NameOfMenu + "/Certification/301/Lesson 4/Posting Events using Wwise-Types", false, priority_certification)]
    public static void OpenScene_L4_1()
    {
        EditorSceneManager.OpenScene(Cert301_SceneFolder + "L4_1 - Posting Events using Wwise-Types.unity");
        //EditorSceneManager.OpenScene(SceneFolder + "Audio Environment Scenes/Dungeon Audio Environment.unity", OpenSceneMode.Additive);
        AdjustCamera();
    }
    [MenuItem(NameOfMenu + "/Certification/301/Lesson 4/Posting Events from Animations", false, priority_certification)]
    public static void OpenScene_L4_2()
    {
        EditorSceneManager.OpenScene(Cert301_SceneFolder + "L4_2 - Posting Events from Animations.unity");
        AdjustCamera();
    }
    [MenuItem(NameOfMenu + "/Certification/301/Lesson 4/Posting Audio Input", false, priority_certification)]
    public static void OpenScene_L4_3()
    {
        EditorSceneManager.OpenScene(Cert301_SceneFolder + "L4_3 - Posting Audio Input.unity");
        AdjustCamera();
    }

    [MenuItem(NameOfMenu + "/Certification/301/Lesson 5/Setting States using the AkState Components", false, priority_certification)]
    public static void OpenScene_L5_1()
    {
        EditorSceneManager.OpenScene(Cert301_SceneFolder + "L5_1 - Setting States using the AkState Component.unity");
        AdjustCamera();
    }
    [MenuItem(NameOfMenu + "/Certification/301/Lesson 5/Setting Game Parameters using Wwise-Types", false, priority_certification)]
    public static void OpenScene_L5_2()
    {
        EditorSceneManager.OpenScene(Cert301_SceneFolder + "L5_2 - Setting Game Parameters using Wwise-Types.unity");
        AdjustCamera();
    }
    [MenuItem(NameOfMenu + "/Certification/301/Lesson 5/Setting a Switch using a Wwise-Type", false, priority_certification)]
    public static void OpenScene_L5_3()
    {
        EditorSceneManager.OpenScene(Cert301_SceneFolder + "L5_3 - Setting a Switch using a Wwise-Type.unity");
        AdjustCamera();
    }
    [MenuItem(NameOfMenu + "/Certification/301/Lesson 5/Understanding Global and Game Object Scope", false, priority_certification)]
    public static void OpenScene_L5_4()
    {
        EditorSceneManager.OpenScene(Cert301_SceneFolder + "L5_4 - Understanding Global and Game Object Scope.unity");
        AdjustCamera();
    }





    [MenuItem(NameOfMenu + "/Certification/301/Lesson 6/Adding the AkEnvironment to a Trigger", false, priority_certification)]
    public static void OpenScene_L6_1()
    {
        EditorSceneManager.OpenScene(Cert301_SceneFolder + "L6_01 - Adding the AkEnvironment to a Trigger.unity");
        AdjustCamera();
    }

    [MenuItem(NameOfMenu + "/Certification/301/Lesson 6/Creating an Aux Environment", false, priority_certification)]
    public static void OpenScene_L6_2()
    {
        EditorSceneManager.OpenScene(Cert301_SceneFolder + "L6_02 - Creating an Aux Environment.unity");
        AdjustCamera();
    }

    [MenuItem(NameOfMenu + "/Certification/301/Lesson 6/Environments inside Environments", false, priority_certification)]
    public static void OpenScene_L6_3()
    {
        EditorSceneManager.OpenScene(Cert301_SceneFolder + "L6_03 - Environments inside Environments.unity");
        AdjustCamera();
    }



    [MenuItem(NameOfMenu + "/Certification/301/Lesson 7/Callbacks using Components", false, priority_certification)]
    public static void OpenScene_L7_1()
    {
        EditorSceneManager.OpenScene(Cert301_SceneFolder + "L7_01 - Callbacks using Components.unity");
        AdjustCamera();
    }
    [MenuItem(NameOfMenu + "/Certification/301/Lesson 7/Callbacks on a Wwise-Type Event", false, priority_certification)]
    public static void OpenScene_L7_2()
    {
        EditorSceneManager.OpenScene(Cert301_SceneFolder + "L7_02 - Callbacks on a Wwise-Type Event.unity");
        AdjustCamera();
    }






    [MenuItem(NameOfMenu + "/Certification/301/Lesson 8/Setting a Music Region State", false, priority_certification)]
    public static void OpenScene_L8_2()
    {
        EditorSceneManager.OpenScene(Cert301_SceneFolder + "L8_02 - Setting a Music Region State.unity");
        AdjustCamera();
    }
    [MenuItem(NameOfMenu + "/Certification/301/Lesson 8/Creating a System for Intersecting State Areas", false, priority_certification)]
    public static void OpenScene_L8_3()
    {
        EditorSceneManager.OpenScene(Cert301_SceneFolder + "L8_03 - Creating a System for Intersecting State Areas.unity");
        AdjustCamera();

    }
    [MenuItem(NameOfMenu + "/Certification/301/Lesson 8/Creating Diversity Using Game Parameters", false, priority_certification)]
    public static void OpenScene_L8_4()
    {
        EditorSceneManager.OpenScene(Cert301_SceneFolder + "L8_04 - Creating Diversity Using Game Parameters.unity");
        AdjustCamera();
    }

    ///////////////////////// LESSON SCENES
    [MenuItem(NameOfMenu + "/Certification/251/Lesson 1/Profiler Scene", false, priority_certification)]
    public static void OpenScene_251_L1_13()
    {
        EditorSceneManager.OpenScene(Cert251_SceneFolder + "Lesson 1.1 - The Profiler" + UnityType);
        AdjustCamera();
    }
    [MenuItem(NameOfMenu + "/Certification/251/Lesson 1/Voice Profiler Scene", false, priority_certification)]
    public static void OpenScene_251_L1_24()
    {
        EditorSceneManager.OpenScene(Cert251_SceneFolder + "Lesson 1.2 - Voice Profiler" + UnityType);
        AdjustCamera();
    }

    [MenuItem(NameOfMenu + "/Certification/251/Lesson 2/Main Scene", false, priority_certification)]
    public static void OpenScene_251_L2_X()
    {
        EditorSceneManager.OpenScene(SceneFolder + "Main Scene" + UnityType);
        AdjustCamera();
    }
    [MenuItem(NameOfMenu + "/Certification/251/Lesson 3/Understanding Virtual Voices", false, priority_certification)]
    public static void OpenScene_251_L3_X()
    {
        EditorSceneManager.OpenScene(Cert251_SceneFolder + "Lesson 3.1 - Understanding Virtual Voices" + UnityType);
        AdjustCamera();
    }
    [MenuItem(NameOfMenu + "/Certification/251/Lesson 3/Verify your Virtual Voices", false, priority_certification)]
    public static void OpenScene_251_L3_2()
    {
        EditorSceneManager.OpenScene(Cert251_SceneFolder + "Lesson 3.2 - Verify your Virtual Voices" + UnityType);
        AdjustCamera();
    }
    [MenuItem(NameOfMenu + "/Certification/251/Lesson 3/Mass Evil Head", false, priority_certification)]
    public static void OpenScene_251_L3_3()
    {
        EditorSceneManager.OpenScene(Cert251_SceneFolder + "Lesson 3.3 - Mass Evil Head" + UnityType);
        AdjustCamera();
    }

    [MenuItem(NameOfMenu + "/Certification/251/Lesson 4/Slow Motion", false, priority_certification)]
    public static void OpenScene_251_L4_2()
    {
        EditorSceneManager.OpenScene(Cert251_SceneFolder + "Lesson 4.2 - Slow Motion" + UnityType);
        AdjustCamera();
    }

    [MenuItem(NameOfMenu + "/Certification/251/Lesson 5/Main Scene", false, priority_certification)]
    public static void OpenScene_251_L5_X()
    {
        EditorSceneManager.OpenScene(SceneFolder + "Main Scene" + UnityType);
        AdjustCamera();
    }
    [MenuItem(NameOfMenu + "/Certification/251/Lesson 6/All In One", false, priority_certification)]
    public static void OpenScene_251_L6_allInOne()
    {
        EditorSceneManager.OpenScene(Cert251_SceneFolder + "Lesson 6.1 - All In One" + UnityType);
        AdjustCamera();
    }
    [MenuItem(NameOfMenu + "/Certification/251/Lesson 6/Loading Region SoundBanks", false, priority_certification)]
    public static void OpenScene_251_L6_LRS()
    {
        EditorSceneManager.OpenScene(Cert251_SceneFolder + "Lesson 6.2 - Loading Region SoundBanks" + UnityType);
        AdjustCamera();
    }
    [MenuItem(NameOfMenu + "/Certification/251/Lesson 6/Desert SoundBank Scenes", false, priority_certification)]
    public static void OpenScene_251_L6_DSS()
    {
        EditorSceneManager.OpenScene(Cert251_SceneFolder + "Lesson 6.3 - Desert Audio Environment" + UnityType);
        AdjustCamera();
    }
    [MenuItem(NameOfMenu + "/Certification/251/Lesson 6/Quest SoundBanks", false, priority_certification)]
    public static void OpenScene_251_L6_QS()
    {
        EditorSceneManager.OpenScene(Cert251_SceneFolder + "Lesson 6.4 - Quest SoundBanks" + UnityType);
        AdjustCamera();
    }

    [MenuItem(NameOfMenu + "/Certification/251/Lesson 7/Memory Pools Scene", false, priority_certification)]
    public static void OpenScene_251_L7_X()
    {
        EditorSceneManager.OpenScene(Cert251_SceneFolder + "Lesson 7.1 - Memory Pool Scene" + UnityType);
        AdjustCamera();
    }




}
