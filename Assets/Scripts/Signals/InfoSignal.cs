public class InfoSignal
{
    readonly public string Header;
    readonly public string Info;
    readonly public GameButton Button;

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