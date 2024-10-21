using UnityEngine;
using UnityEngine.UI;

public class RandomNumber : MonoBehaviour
{
    [SerializeField] Sprite[] Sprites;
    [SerializeField] Image Image;

    void OnEnable()
    {
        Image.sprite = Sprites[Random.Range(0, Sprites.Length)];
    }
}