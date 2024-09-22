using UnityEngine;
using UnityEngine.UI;
using Zenject;
using YG;

public class Socket : MonoBehaviour
{
    [Inject] InMouse InMouse;
    [Inject] EventBus EventBus;
    [Inject] AudioManager AudioManager;

    Image Icon;
    bool IsEnter;
    float Timer = 0.1f;

    void Awake()
    {
        Icon = GetComponent<Image>();
    }

    public void CheckMousePos(bool Switch)
    {
        IsEnter = Switch;
    }

    public Sprite GetSprite()
    {
        return Icon.sprite;
    }

    public void Initialization(Sprite Icon)
    {
        this.Icon.color = Color.white;
        this.Icon.sprite = Icon;
    }

    public void SetItemToSocket(Sprite Icon)
    {
        if(this.Icon.sprite == null)
        {
            this.Icon.color = Color.white;
            this.Icon.sprite = Icon;
            EventBus.Invoke("Check");
            AudioManager.PlaySound("SoftClick", null);
            InMouse.Clear(); 
        }
        else
        {
            InMouse.ReturnItemToSocket();              
        }
    }

    void SeItemToMouse()
    {
        AudioManager.PlaySound("SoftClick", null);
        InMouse.SetItemToMouse(Icon.sprite, this);
        Icon.sprite = null;
        Icon.color = new Color(1, 1, 1, 0);
    }

    public void Clear()
    {
        Icon.sprite = null;
        Icon.color = new Color(1, 1, 1, 0);      
    }

    void Update()
    {

        if(!YandexGame.EnvironmentData.isDesktop)
        {
            if(Input.touchCount <= 0)
            {
                Timer -= Time.deltaTime;
                if(Timer <= 0)
                {
                    IsEnter = false;
                }
            }
            else
            {
                Timer = 0.1f;
            }       
        }

        if(IsEnter)
        {
            if(Input.GetMouseButtonUp(0))
            {
                if(InMouse.Icon.sprite != null)
                {
                    if(Icon.sprite == null)
                    {
                        SetItemToSocket(InMouse.Icon.sprite);
                        InMouse.Clear();                        
                    }
                    else
                    {
                        var Sprite = Icon.sprite;
                        Icon.sprite = InMouse.Icon.sprite;
                        EventBus.Invoke("Check");
                        AudioManager.PlaySound("SoftClick", null);
                        InMouse.Icon.sprite = Sprite;
                        InMouse.ReturnItemToSocket();
                    }
                }
            } 

            if(Input.GetMouseButtonDown(0))
            {
                if(Icon.sprite != null && InMouse.Icon.sprite == null)
                {
                    SeItemToMouse();
                }         
            }                    
        }
    }
}
