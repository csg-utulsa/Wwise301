////////////////////////////////////////////////////////////////////////
//
// Copyright (c) 2018 Audiokinetic Inc. / All Rights Reserved
//
////////////////////////////////////////////////////////////////////////

﻿using UnityEngine;
using System.Linq;
using System.Collections.Generic;

public delegate void LanguageEvent();

public class LanguageManager : MonoBehaviour
{
    [Tooltip("This should be a .csv file.")]
    public TextAsset Languages;
    public static event LanguageEvent OnLanguageChange;

    #region private variables
    //private static string WwiseLanguageKey = "wwise_key"; //TODO: Reintroduce in 301
    private static string currentLanguage = "English";
    private static Dictionary<string, Dictionary<string, string>> Language = new Dictionary<string, Dictionary<string, string>>();
    #endregion

    void Awake()
    {
        LoadLanguages();
    }

    private void LoadLanguages()
    {
        Language = new Dictionary<string, Dictionary<string, string>>();
        string entireFile = Languages.text;
        string[] lines = entireFile.Split('\n'); // separates lines
        string[] languages = lines[0].Split(','); //first row is languages

        for (int a = 1; a < languages.Length; a++)
        {
            Language.Add(languages[a], new Dictionary<string, string>());
        }

        for (int l = 1; l < lines.Length; l++)
        { //run through each line except the first, which is the language
            string[] keysAndStrings = lines[l].Split(',');

            string key = keysAndStrings[0];
            for (int i = 1; i < keysAndStrings.Length; i++)
            {
                string str = keysAndStrings[i];

                //check for and fix the weird comma-replacements
                while (str.Contains("ˌ"))
                {
                    int weirdCommaIndex = str.IndexOf("ˌ");

                    char[] newLine = str.ToCharArray();
                    for (int x = 0; x < str.Length; x++)
                    {
                        if (x == weirdCommaIndex)
                        {
                            newLine[x] = ',';
                        }
                        else
                        {
                            newLine[x] = str[x];
                        }
                    }
                    str = new string(newLine);
                }
                Language[languages[i]].Add(key, str);
            }
        }

        if (Language.ContainsKey(currentLanguage))
        {
            ChangeLanguage(currentLanguage);
        }
        else if (languages.Length > 0)
        {
            ChangeLanguage(languages[1]); //if default value ("English") is not available, just pick the first available
        }
        else
        {
            Debug.LogError("Problem parsing Language CSV TextAsset.");
        }
    }

    public static string GetText(string key)
    {
        if (key != "" && Language[currentLanguage] != null && Language[currentLanguage].ContainsKey(key) && Language[currentLanguage][key] != null)
        {
            return Language[currentLanguage][key];
        }
        else
        {
            // Maybe throw exception here...
            return "LOCALISATION ERROR (key: '" + key + "', language: " + currentLanguage;
        }
    }

    public static string GetCurrentLanguage()
    {
        return currentLanguage;
    }

    public static List<string> GetAvailableLanguages()
    {
        return Language.Keys.ToList();
    }

    public static void ChangeLanguage(string newLanguage)
    {
        if (Language.ContainsKey(newLanguage))
        {
            currentLanguage = newLanguage;

            if (OnLanguageChange != null)
            {
                OnLanguageChange();
            }

            //AkSoundEngine.SetCurrentLanguage(GetText(WwiseLanguageKey)); //TODO: Reintroduce in 301
        }
    }
}