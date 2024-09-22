public class EventBus
{
    public delegate void Action(object obj);
    public event Action Signal;
    public void Invoke(object obj)
    {
        Signal?.Invoke(obj);
    }
}