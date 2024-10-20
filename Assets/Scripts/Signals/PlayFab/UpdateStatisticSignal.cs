using System.Collections.Generic;

public class UpdateStatisticSignal
{
    public readonly Dictionary<string, int> Result = new Dictionary<string, int>();
    
    public UpdateStatisticSignal(Dictionary<string, int> Result)
    {
        this.Result = new Dictionary<string, int>(Result);
    }
}