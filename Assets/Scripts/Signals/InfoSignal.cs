public class InfoSignal
{
    public readonly string Header;
    public readonly string Info;
    public readonly GameButton Button;

    public InfoSignal(string Header, string Info, GameButton Button)
    {
        this.Header = Header;
        this.Info = Info;
        this.Button = Button;
    }

    public InfoSignal(string Header, string Info)
    {
        this.Header = Header;
        this.Info = Info;
    }
}