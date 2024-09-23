using UnityEngine;

public class Hand : MonoBehaviour
{
    [SerializeField] RectTransform Target;

    void Start()
    {
        gameObject.SetActive(PlayerPrefs.GetInt("Hand") == 0);
    }

    public void Hide()
    {
        if(PlayerPrefs.GetInt("Hand") == 0)
        {
            gameObject.SetActive(false);
            PlayerPrefs.SetInt("Hand", 1);
            PlayerPrefs.Save();
        }
    }
}