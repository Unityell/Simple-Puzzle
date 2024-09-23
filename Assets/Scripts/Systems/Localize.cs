using UnityEngine;
using TMPro;
using System.Collections.Generic;
using YG;

public class Localize : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI Text;
    [SerializeField] List<Localization> LangSettings;

    private void OnEnable() => YandexGame.GetDataEvent += Localization;
    private void OnDisable() => YandexGame.GetDataEvent -= Localization;

    void OnValidate()
    {
        if(!Text) Text = GetComponent<TextMeshProUGUI>();
    }

    void Start()
    {
        if(YandexGame.SDKEnabled)
        {
            Localization();
        }
    }

    void Localization()
    {
        Text.text = LangSettings.Find(x => x.Name == YandexGame.EnvironmentData.language).Phrase;
    }
}