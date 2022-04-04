namespace Straisimulator.Services;

public class DataProcessService
{
    public DataProcessService()
    {
        
    }
    
    /*public List<int> getOpAndCykTime(string inputText)
    {
        string pattern = @"tid: (?<operasjonstid>\d{2}:\d{2}) .+tid: (?<cykeltid>\d{2}:\d{2})";
    }*/

    public bool checkForOperation(string input)
    {
        if(input.Contains("Operation"))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public int calculateQueTime(DateTime skap1Started, DateTime skap2Started)
    {
        TimeSpan que = skap2Started - skap1Started;
        int secondsOfQue = que.Seconds;
        return secondsOfQue;
    }
}