using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using YG;

public class Rotation : MonoBehaviour
{
    [SerializeField] GameObject Button;
    [SerializeField] TextMeshProUGUI Text;
    [SerializeField] Image BackGround;

    void OnEnable()
    {
        Button.SetActive(false);
        Text.text = "";
    }

    public void ONClick()
    {
        StartCoroutine(Rotate());
    }

    IEnumerator Rotate()
    {
        float RotateSpeed = Random.Range(3, 10);
        float Timer = 3;

        while (Timer > 0)
        {
            Timer -= Time.fixedDeltaTime;
            transform.eulerAngles += Vector3.forward * RotateSpeed;
            yield return null;
        }
        
        Button.SetActive(true);
        int Rand = Random.Range(0, 2);
        string TextRUSucces = "Удача с вами!";
        string TextRUFail = "В следующий раз повезёт!"; 
        string TextENSucces = "Good luck with you!";
        string TextENFail = "Better luck next time!"; 
        Text.text = YandexGame.EnvironmentData.language == "ru" ? Rand == 0 ? TextRUSucces : TextRUFail : Rand == 0 ? TextENSucces : TextENFail;
    }
}