using UnityEngine;

public class Hand : MonoBehaviour
{
    void Start()
    {
        if(PlayerPrefs.GetInt("Hand") == 1)
        {
            gameObject.SetActive(false);
        }
    }
}
