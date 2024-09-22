using UnityEngine;
using TMPro;
using System.Collections.Generic;
using YG;

public class Localize : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI Text;
    [SerializeField] List<Localization> LangSettings;

    void OnValidate()
    {
        Text = GetComponent<TextMeshProUGUI>();
    }

    void Start()
    {
        Text.text = LangSettings.Find(x => x.Name == YandexGame.EnvironmentData.language).Phrase;
    }
}