using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using YG;
using Zenject;

public class LuckieWidget : Widgets
{
    [Inject] GameData GameData;
    [SerializeField] GameObject Button;
    [SerializeField] TextMeshProUGUI Text;
    [SerializeField] Image BackGround;
    [SerializeField] Transform RotatePart;
    bool Start;

    public void LuckieStart()
    {
        if (GameData.Coins <= 0) return;
        Button.SetActive(false);
        BackGround.color = Color.white;
        Text.text = "";
        Enable(true);
    }

    public void OnClick()
    {
        if(Start) return;
        StartCoroutine(Rotate());
        EventBus.Invoke(new CoinSignal(1, EnumCoinAction.Remove));
    }

    IEnumerator Rotate()
    {
        Start = true;
        float RotateSpeed = Random.Range(10, 20);
        float Timer = 3;

        while (Timer > 0)
        {
            Timer -= Time.fixedDeltaTime;
            RotatePart.transform.eulerAngles += Vector3.forward * RotateSpeed;
            yield return null;
        }
        
        Button.SetActive(true);
        int Rand = Random.Range(0, 2);

        string TextRUSucces = "Удача с вами!";
        string TextRUFail = "В следующий раз повезёт!"; 
        string TextENSucces = "Good luck with you!";
        string TextENFail = "Better luck next time!"; 
        BackGround.color = Rand == 0 ? Color.green : Color.red;
        Text.text = YandexGame.EnvironmentData.language == "ru" ? Rand == 0 ? TextRUSucces : TextRUFail : Rand == 0 ? TextENSucces : TextENFail;
    }

    public void Close()
    {
        Start = false;
        Enable(false);
    }
}