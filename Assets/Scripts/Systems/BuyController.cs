using UnityEngine;
using YG;
using Zenject;

public class BuyController : MonoBehaviour
{
    [Inject] EventBus EventBus;

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