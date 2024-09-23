using UnityEngine;

public class Message
{
    public readonly EnumSignals Signal;
    public readonly GameObject Sender;
    public readonly object Data;

    public Message(EnumSignals Signal, GameObject Sender, object Data)
    {
        this.Signal = Signal;
        this.Data = Data;
        this.Sender = Sender;
    }

    public Message(EnumSignals Signal, GameObject Sender)
    {
        this.Signal = Signal;
        Data = null;
        this.Sender = Sender;
    }
}