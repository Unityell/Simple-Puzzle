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
        
        // Настройка скорости и продолжительности
        float flashDuration = duration * 0.1f; // Быстрое моргание
        float springDuration = duration * 0.4f; // Быстрая и короткая пружинящая анимация

        // Мгновенное моргание цветом
        Icon.color = flashColor;
        yield return new WaitForSeconds(flashDuration);
        
        // Возвращаем цвет обратно
        Icon.color = originalColor;

        // Пружинистая анимация масштаба
        while (elapsedTime < springDuration)
        {
            // Используем экспоненциальное затухание амплитуды для пружинистого эффекта
            float dampingFactor = Mathf.Exp(-elapsedTime * 6); // Быстрое затухание
            float oscillation = Mathf.Sin(elapsedTime * Mathf.PI * 6) * dampingFactor;
            float scaleProgress = 1 + oscillation * (targetScale - 1); // Амплитуда уменьшается со временем
            
            Icon.transform.localScale = originalScale * scaleProgress;
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Возвращаем масштаб обратно
        Icon.transform.localScale = originalScale;
        isEffectActive = false;
    }
}