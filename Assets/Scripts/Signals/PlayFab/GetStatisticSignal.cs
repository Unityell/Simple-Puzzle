using System.Collections.Generic;

public class GetStatisticSignal
{
    public readonly Dictionary<string, int> Result = new Dictionary<string, int>();
    
    public GetStatisticSignal(Dictionary<string, int> Result)
    {
        this.Result = new Dictionary<string, int>(Result);
    }
}