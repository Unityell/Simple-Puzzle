using System;
using UnityEngine;

public abstract class BaseSignal : MonoBehaviour
{
    public delegate void Signal(object Obj);
    public event Signal MySignal;

    public void Subscribe(Action<object> Method)
    {
        MySignal += new Signal(Method);
    }

    public void Unsubscribe(Action<object> Method)
    {
        MySignal -= new Signal(Method);
    }

    public void EmitSignal(object Obj)
    {
        #if UNITY_EDITOR
        if (Obj is Message)
        {
            Message Message = (Message)Obj;

            string senderText = Message.Sender != null ? Message.Sender.ToString() : "Sender not specified";
            string signalText = Message.Signal.ToString();

            string dataTypeText;

            switch (Message.Data)
            {
                case string str:
                    dataTypeText = $" {str}";
                    break;
                case int intValue:
                    dataTypeText = $" {intValue}";
                    break;
                case float floatValue:
                    dataTypeText = $" {floatValue}";
                    break;
                case null:
                    dataTypeText = " Data type not specified";
                    break;
                default:
                    dataTypeText = Message.Data.GetType().ToString();
                    break;
            }

            Debug.Log($"<color=yellow><b>Sender:</b></color> {senderText} <color=#ADD8E6><b>Signal:</b></color> {signalText} <color=green><b>DataType:</b></color> {dataTypeText}");
        }
        #endif

        MySignal?.Invoke(Obj);
    }
}