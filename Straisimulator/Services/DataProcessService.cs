using System.Text.RegularExpressions;

namespace Straisimulator.Services;

public class DataProcessService
{
    public DataProcessService()
    {
        
    }
    
    //hvis minutter er tom, må den ta hensyn til det
    //denne regex funker hvis det kun er i sekunder
    //problemet er at sjekken nedfor forventer at første delen av verdien fra regex er tom/mellomrom(?) og dermed leter
    //etter substrings som ikke finnes
    public List<TimeSpan> getOpAndCykTime(string inputText)
    {
        string pattern = @"tid: ((?<operasjonstid>(\d{2}:\d{2})|\d+)s).+tid: (?<cykeltid>\d{2}:\d{2}|\d+?.?\d+?)s";
        Regex r = new Regex(pattern, RegexOptions.IgnoreCase);
        Match match = r.Match(inputText);
        
        //vil alltid bare ha 2 entries:
        List<TimeSpan> opAndCykList = new List<TimeSpan>();

        if (match.Success)
        {
            string op = match.Groups["operasjonstid"].Value;
            string cyk = match.Groups["cykeltid"].Value;

            TimeSpan opTime;
            TimeSpan cykTime;
            if (op.Substring(0, 2) == String.Empty)
            {
                opTime = new TimeSpan(0, 0, 0, Convert.ToInt32(op.Substring(3, 2)));
                cykTime = new TimeSpan(0, 0, Convert.ToInt32(cyk.Substring(0, 2)),
                    Convert.ToInt32(cyk.Substring(3, 2)));
            }
            else if (cyk.Substring(0, 2) == String.Empty)
            {
                opTime = new TimeSpan(0, 0, Convert.ToInt32(op.Substring(0, 2)),
                    Convert.ToInt32(op.Substring(3, 2)));
                cykTime = new TimeSpan(0, 0, 0, Convert.ToInt32(cyk.Substring(3, 2)));
            } 
            else if (op.Substring(0, 2) == String.Empty && cyk.Substring(0, 2) == String.Empty)
            {
                opTime = new TimeSpan(0, 0, 0, Convert.ToInt32(op.Substring(3, 2)));
                cykTime = new TimeSpan(0, 0, 0, Convert.ToInt32(cyk.Substring(3, 2)));
            }
            else
            {
                opTime = new TimeSpan(0, 0, Convert.ToInt32(op.Substring(0, 2)), Convert.ToInt32(op.Substring(3, 2)));
                cykTime = new TimeSpan(0, 0, Convert.ToInt32(cyk.Substring(0, 2)),
                    Convert.ToInt32(cyk.Substring(3, 2)));
            }
            opAndCykList.Add(opTime);
            opAndCykList.Add(cykTime);
        }
        return opAndCykList;
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
