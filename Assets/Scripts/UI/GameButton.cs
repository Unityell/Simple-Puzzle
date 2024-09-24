using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zenject;
using YG;

public class GameButton : MonoBehaviour
{
    [Inject] EventBus EventBus;
    
    Image Sprite;
    [SerializeField] List<Sprite> Sprites;
    [SerializeField] List<Localization> Header;
    [SerializeField] List<Localization> Info;
    GameObject Lock;

    void Awake()
    {
        Sprite = GetComponent<Image>();
        Lock = transform.GetChild(0).gameObject;
    }

    public void Refresh()
    {
        if(transform.GetSiblingIndex() == 0) 
        {
            PlayerPrefs.SetInt("Button" + transform.GetSiblingIndex(), 1);
            PlayerPrefs.Save();
            Lock.SetActive(false);
            return;
        }

        if(PlayerPrefs.GetInt("Button" + transform.GetSiblingIndex()) == 0)
        {
            Sprite.color = new Color(0.1f, 0.1f, 0.1f, 1);
            Lock.SetActive(true);
        }
        else
        {
            Lock.SetActive(false);
            Sprite.color = Color.white;
        }
    }

    public void StartGame()
    {
        if(PlayerPrefs.GetInt("Button" + transform.GetSiblingIndex()) == 1)
        {
            var HeaderText = Header.Find(x => x.Name == YandexGame.EnvironmentData.language).Phrase;
            var InfoText = Info.Find(x => x.Name == YandexGame.EnvironmentData.language).Phrase;
            EventBus.Invoke(new InfoSignal(HeaderText, InfoText, this));            
        }
        else
        {
            var HeaderText = YandexGame.EnvironmentData.language == "ru" ? "Закрыто!" : "Locked!";
            var InfoText = YandexGame.EnvironmentData.language == "ru" ? "Чтобы разблокировать, соберите другие открытки!" : "To unlock, collect other postcards!";
            EventBus.Invoke(new InfoSignal(HeaderText, InfoText)); 
        }
    }

    public void Cast()
    {
        StartGameSignal Signal = new StartGameSignal(Sprites, transform.GetSiblingIndex(), Sprite.sprite);
        EventBus.Invoke(Signal);             
    }
}