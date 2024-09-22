using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public List <AudioSettings> AudioBasa;
    public List <GameObject> AllMusic;
    bool SoundIsPlay = true;

    void OnApplicationFocus(bool hasFocus)
    {
        if(!hasFocus)
        {
            SoundIsPlay = Mathf.RoundToInt(AudioListener.volume) == 1 ? true : false;
            AudioListener.volume = 0;
        }
        else
        {
            if(SoundIsPlay) AudioListener.volume = 1; else AudioListener.volume = 0;
        }
    }

    public void OnOffSound(int Number)
    {
        AudioListener.volume = Number;
        PlaySound("SoftClick", null);
    }

    public void PlaySound(string SoundName, Transform Parent)
    {
        if(SoundIsPlay)
        {
            GameObject SoundMaster = new GameObject();
            SoundMaster.name = SoundName;

            SoundMaster.transform.SetParent(Parent == null ? transform : Parent);

            var NewSound = SoundMaster.AddComponent<AudioSource>();
            var SoundSimple = SoundMaster.AddComponent<AudioSimple>();

            SoundSimple.Initialization(this);

            for (int i = 0; i < AudioBasa.Count; i++)
            {
                if(AudioBasa[i].Name == SoundName)
                {
                    NewSound.volume = AudioBasa[i].Volume;
                    NewSound.clip = AudioBasa[i].AudioClips[Random.Range(0, AudioBasa[i].AudioClips.Length)];
                    NewSound.Play();
                    AllMusic.Add(SoundMaster.gameObject);
                    if(!AudioBasa[i].Loop)
                    {
                        Destroy(SoundMaster, NewSound.clip.length);
                    }
                    else
                    {
                        NewSound.loop = true;
                    }
                    break;
                }
            }
        }
    }

    public void DestroySound(string SoundName)
    {
        if(AllMusic.Count > 0)
        {
            if(AllMusic.Count > 0)
            for (int i = 0; i < AllMusic.Count; i++)
            {
                if(AllMusic[i].name == SoundName)
                {
                    Destroy(AllMusic[i]);
                    AllMusic.Remove(AllMusic[i]);
                    break;
                }
            }
        }
    }
}

[System.Serializable]
public class AudioSettings
{
    public string Name;
    [Range(0,1) ] public float Volume;
    public bool Loop;
    public AudioClip[] AudioClips;
}