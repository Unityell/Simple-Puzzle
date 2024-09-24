using UnityEngine;
using UnityEngine.UI;
using Zenject;
using System.Collections;
using YG;

public class Socket : MonoBehaviour
{
    [Inject] InMouse InMouse;
    [Inject] EventBus EventBus;
    [Inject] AudioManager AudioManager;

    Image Icon;
    bool IsEnter;
    float Timer = 0.1f;

    bool isEffectActive = false;

    void Awake()
    {
        Icon = GetComponent<Image>();
    }

    public void Enable(bool Switch)
    {
        Icon.raycastTarget = Switch;
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
        Enable(true);
    }

    public void SetItemToSocket(Sprite newIcon)
    {
        if (this.Icon.sprite == null)
        {
            this.Icon.color = Color.white;
            this.Icon.sprite = newIcon;
            EventBus.Invoke(EnumSignals.Check);
            AudioManager.PlaySound("SoftClick", null);
            InMouse.Clear(); 
            StartEffect(Color.cyan, 1.1f, 0.5f);
        }
        else
        {
            StartEffect(Color.cyan, 1.1f, 0.5f);
            if(InMouse.Icon.sprite == null)
            {
                this.Icon.sprite = newIcon;  
                EventBus.Invoke(EnumSignals.Check);
                AudioManager.PlaySound("SoftClick", null);          
            }
            else InMouse.ReturnItemToSocket(); 
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
        Icon.transform.localScale = Vector3.one;
    }

    void Update()
    {
        if (!YandexGame.EnvironmentData.isDesktop)
        {
            if (Input.touchCount <= 0)
            {
                Timer -= Time.deltaTime;
                if (Timer <= 0)
                {
                    IsEnter = false;
                }
            }
            else
            {
                Timer = 0.1f;
            }       
        }

        if (IsEnter)
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (Icon.sprite != null && InMouse.Icon.sprite == null)
                {
                    SeItemToMouse();
                }         
            }  

            if (Input.GetMouseButtonUp(0))
            {
                if (InMouse.Icon.sprite != null)
                {
                    if (Icon.sprite == null)
                    {
                        SetItemToSocket(InMouse.Icon.sprite);
                        InMouse.Clear();                        
                    }
                    else
                    {
                        var oldSprite = Icon.sprite;
                        Icon.sprite = InMouse.Icon.sprite;
                        EventBus.Invoke(EnumSignals.Check);
                        AudioManager.PlaySound("SoftClick", null);
                        InMouse.Icon.sprite = oldSprite;
                        InMouse.ReturnItemToSocket();

                        StartEffect(Color.cyan, 1.1f, 0.5f);
                    }
                }
            }                   
        }
    }

    private void StartEffect(Color flashColor, float targetScale, float duration)
    {
        if (isEffectActive) return;

        isEffectActive = true;
        StartCoroutine(EffectCoroutine(flashColor, targetScale, duration));
    }

    private IEnumerator EffectCoroutine(Color flashColor, float targetScale, float duration)
    {
        Color originalColor = Icon.color;
        Vector3 originalScale = Icon.transform.localScale;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            Icon.color = Color.Lerp(originalColor, flashColor, Mathf.PingPong(elapsedTime * 4, 1));
            Icon.transform.localScale = Vector3.Lerp(originalScale, originalScale * targetScale, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        Icon.color = originalColor;
        Icon.transform.localScale = originalScale;
        isEffectActive = false;
    }
}
