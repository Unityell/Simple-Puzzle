using UnityEngine;

public class AudioSimple : MonoBehaviour
{
    AudioManager AudioManager;
    public void Initialization(AudioManager AudioManager)
    {
        this.AudioManager = AudioManager;
    }
    void OnDestroy()
    {   
        AudioManager.AllMusic.Remove(gameObject);
    }
}
