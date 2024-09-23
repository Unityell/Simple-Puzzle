using UnityEngine;
using Zenject;

public class Widgets : BaseSignal
{
    [Inject] protected EventBus EventBus;

    [Header("Widget socket")]
    [SerializeField] protected GameObject Widget;

    public virtual void Enable(bool Switch)
    {
        Widget.SetActive(Switch);
    }

    protected virtual void Subscribe()
    {
        EventBus.Subscribe(SignalBox);
    }

    protected virtual void UnSubscribe()
    {
        EventBus.Unsubscribe(SignalBox);
    }

    protected virtual void SignalBox(object Obj){}

    void OnDestroy()
    {
        UnSubscribe();
    }
}