using UnityEngine;
using System.Collections;
using System.IO;
using System.Text;
using System.Globalization;
using Newtonsoft.Json;
using System.Linq;
using System;

public class LocalizationManager : MonoBehaviour
{
    public static LocalizationManager localizationInstance; 
    public string currentLanguage = ""; 
    private LocalizationData localizationData; 

    void Awake()
    {
        if (localizationInstance == null)
        {
            localizationInstance = this;
        }
    }

    public string SetNewLanguage(string language)
    {
        currentLanguage = language;

        if(currentLanguage == "")
        {
            currentLanguage = CultureInfo.CurrentCulture.TwoLetterISOLanguageName;
        }

        LoadLocalizedText(currentLanguage);
        ApplyNewLocalization();

        return currentLanguage;
    }

    public void LoadLocalizedText(string language)
    {
        string filePath = "Localization/" + language;
    
        TextAsset jsonTextAsset = Resources.Load<TextAsset>(filePath) ?? null;

        if (jsonTextAsset != null)
        {
            string json = jsonTextAsset.text;
            localizationData = JsonConvert.DeserializeObject<LocalizationData>(json);
        }
        else
        { 
            jsonTextAsset = Resources.Load<TextAsset>("Localization/en");
            string json = jsonTextAsset.text;
            localizationData = JsonConvert.DeserializeObject<LocalizationData>(json);
        }
    }

    public string GetLocalizedValue(string key)
    {
        string value = key;

        if (localizationData != null)
        {
            if(localizationData.localizedTexts.ContainsKey(key))
            {
                value = localizationData.localizedTexts[key];
            }
            else
            {
                value = localizationData.localizedTexts["error"];
            }
        }
        
        return value;
    }

    public int GetSizeDictionary(string color)
    {
        if (localizationData != null)
        {
            return localizationData.narrationsTexts[color].Count;
        }
        return 0;
    }

    public string GetLocalizedValueForNarration(string color, string key)
    {
        string value = localizationData.localizedTexts["error"];

        if (localizationData != null)
        {
            if(localizationData.narrationsTexts[color].ContainsKey(key))
            {
                value = localizationData.narrationsTexts[color][key];
            }
        }

        return value;
    }

    public void ApplyNewLocalization()
    {
        GameObject[] texts = GameObject.FindGameObjectsWithTag("TextLocalized");

        foreach (GameObject text in texts)
        {
            SetTextLocalized newText = text.GetComponent<SetTextLocalized>();
            StartCoroutine(newText.NewTextLocalized());
        }
    }
}
