using UnityEngine;
using Zenject;

public class InMouseInstaller : MonoInstaller
{
    [SerializeField] InMouse InMouse;
    public override void InstallBindings()
    {
        Container.BindInstance<InMouse>(InMouse).AsSingle();
    }
}
