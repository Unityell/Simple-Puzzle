using UnityEngine;
using Zenject;

public class AudioInstaller : MonoInstaller
{
    [SerializeField] AudioManager AudioManager;

    public override void InstallBindings()
    {
        Container.BindInstance<AudioManager>(AudioManager).AsSingle();
    }
}
