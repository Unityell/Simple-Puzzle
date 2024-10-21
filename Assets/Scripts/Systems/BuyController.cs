using System.Collections;
using UnityEngine;
using YG;
using Zenject;

public class BuyController : MonoBehaviour
{
    [Inject] EventBus EventBus;

    void Start() => StartCoroutine(Waiting());

    IEnumerator Waiting()
    {
        yield return new WaitForSeconds(1);
        YandexGame.ConsumePurchases();
    }

    private void OnEnable()
    {
        if(!YandexGame.auth)
        {
            gameObject.SetActive(false);
        }
        YandexGame.PurchaseSuccessEvent += SuccessPurchased;
        YandexGame.PurchaseFailedEvent += FailedPurchased;
    }

    private void OnDisable()
    {
        YandexGame.PurchaseSuccessEvent -= SuccessPurchased;
        YandexGame.PurchaseFailedEvent -= FailedPurchased;
    }

    void SuccessPurchased(string id)
    {
        EventBus.Invoke(new CoinSignal(25, EnumCoinAction.Add));
    }

    void FailedPurchased(string id)
    {
        
    }
}