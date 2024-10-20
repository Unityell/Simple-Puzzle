public class LoadingWidget : Widgets
{
    void Awake()
    {
        Enable(true);
        Subscribe();
    }

    protected override void SignalBox(object Obj)
    {
        switch (Obj)
        {
            case EnumSignals.Start :
                Enable(false);
                break;
            default: break;
        }
    }
}