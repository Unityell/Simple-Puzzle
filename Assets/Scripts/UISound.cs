using UnityEngine;
using Zenject;

public class UISound : MonoBehaviour
{
    [Inject] AudioManager AudioManager;

    public void OnClick(string SoundName)
    {
        AudioManager.PlaySound(SoundName, null);
    }
}