using System.Text.RegularExpressions;

namespace Straisimulator.Services;

public class DataProcessService
{
    public DataProcessService()
    {
        
    }
    
    public List<int> getOpAndCykTime(string inputText)
    {
        string pattern = @"tid: (?<operasjonstid>\d{2}:\d{2}) .+tid: (?<cykeltid>\d{2}:\d{2})";
        Match match = Regex.Match(inputText, pattern);

        if (match.Success)
        {
            string test = match.Groups["operationstid"].Value;
            string test2 = match.Groups["cykeltid"].Value;

            TimeSpan time = new TimeSpan(0, 0, Convert.ToInt32(test.Substring(0, 2)),
                Convert.ToInt32(test.Substring(3, 2)));
        }
        return new List<int>();
    }

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
