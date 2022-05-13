using System.Text.RegularExpressions;
using Straisimulator.Models;

namespace Straisimulator.Services;

public class DataProcessService
{
    public DataProcessService()
    {
        
    }

    //denne er kun laget fordi jeg enda ikke har fått til å nå de andre attributtene til ProductionType (Description)
    public void AddMachine(List<Event> events)
    {
        foreach (Event ev in events)
        {
            if (ev.ProductionType == 0)
            {
                ev.Maskin = "Undefined";
            }
            else if (ev.ProductionType == 110)
            {
                ev.Maskin = "Other";
            }
            else if (ev.ProductionType == 320)
            {
                ev.Maskin = "Drilling";
            }
            else if (ev.ProductionType == 330)
            {
                ev.Maskin = "Fitting 1";
            }
            else if (ev.ProductionType == 350)
            {
                ev.Maskin = "Fitting 2";
            }
            else if (ev.ProductionType == 360)
            {
                ev.Maskin = "Assembly";
            }
        }
    }

    //RETTET: det som er feil nå: i extrainfo på drilling står det "Tid" og "cykeltid" - cykeltida kommer ut riktig,
    //men Tid er ikke op-tid
    public List<TimeSpan> GetOpAndCykTime(string inputText)
    {
        string pattern = @"tid: ((?<operasjonstid>(\d{2}:\d{2})|\d+)s?).+tid: (?<cykeltid>\d{2}:\d{2}|\d+?.?\d+?)s?";
        Regex r = new Regex(pattern, RegexOptions.IgnoreCase);
        Match match = r.Match(inputText);
        
        //vil alltid bare ha 2 entries
        List<TimeSpan> opAndCykList = new List<TimeSpan>();

        if (match.Success)
        {
            string op = match.Groups["operasjonstid"].Value;
            string cyk = match.Groups["cykeltid"].Value;

            TimeSpan opTime;
            TimeSpan cykTime;
            
            //sjekker om operation- eller cykeltid inneholder ':'
            if(op.Contains(':') && !cyk.Contains(':'))
            {
                string opMin = op.Substring(0, 2);
                string opSec = op.Substring(3, 2);
                opTime = new TimeSpan(0, 0, Convert.ToInt32(opMin), Convert.ToInt32(opSec));
                cykTime = new TimeSpan(0, 0, 0, Convert.ToInt32(Math.Round(Convert.ToDouble(cyk))));
            } 
            else if(cyk.Contains(':') && !op.Contains(':'))
            {
                string cykMin = cyk.Substring(0, 2);
                string cykSec = cyk.Substring(3, 2);
                opTime = new TimeSpan(0, 0, 0, Convert.ToInt32(Math.Round(Convert.ToDouble(op))));
                cykTime = new TimeSpan(0, 0, Convert.ToInt32(cykMin), Convert.ToInt32(cykSec));
            } 
            else if(op.Contains(':') && cyk.Contains(':'))
            {
                string opMin = op.Substring(0, 2);
                string opSec = op.Substring(3, 2);
                opTime = new TimeSpan(0, 0, Convert.ToInt32(opMin), Convert.ToInt32(opSec));
                
                string cykMin = cyk.Substring(0, 2);
                string cykSec = cyk.Substring(3, 2);
                cykTime = new TimeSpan(0, 0, Convert.ToInt32(cykMin), Convert.ToInt32(cykSec));
            } 
            else
            {
                opTime = new TimeSpan(0, 0, 0, Convert.ToInt32(Math.Round(Convert.ToDouble(op))));
                cykTime = new TimeSpan(0, 0, 0, Convert.ToInt32(Math.Round(Convert.ToDouble(cyk))));
            }
            
            opAndCykList.Add(opTime);
            opAndCykList.Add(cykTime);
        }
        return opAndCykList;
    }
}
