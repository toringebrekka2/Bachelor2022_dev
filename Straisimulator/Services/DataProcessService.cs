using System.Text.RegularExpressions;
using Straisimulator.Models;

namespace Straisimulator.Services;

public class DataProcessService
{
    public DataProcessService()
    {
        
    }

    public void RemoveEmptyExtraInfo(List<Event> prodEvents)
    {
        for (int i = 0; i < prodEvents.Count; i++)
        {
            if (prodEvents[i].ExtraInfo == String.Empty)
            {
                prodEvents.RemoveAt(i);
            }
        }
    }

    public void AddOpAndCykAsTimeSpan(List<Event> prodEvents)
    {
        foreach (Event ev in prodEvents)
        {
            List<TimeSpan> list = GetOpAndCykTime(ev.ExtraInfo);
            
            //fikser feilen med at borring kun gir cykeltid:
            if (list.Count != 0)
            {
                if (ev.ProductionType == 320)
                {
                    list[0] = new TimeSpan(0, 0, 0, 0);
                }
            }
            ev.OpAndCykAsTimeSpan = list;
        }
    }

    //denne er kun laget fordi jeg enda ikke har fått til å nå de andre attributtene til ProductionType (Description):
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

    //RETTET det som er feil nå: i extrainfo på drilling står det "Tid" og "cykeltid" - cykeltida kommer ut riktig,
    //men Tid er ikke op-tid:
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

    public void AddQue(List<Event> prodEvents)
    {
        foreach (Event e in prodEvents)
        {
            if (e.OpAndCykAsTimeSpan.Count != 0)
            {
                //hvis op-tid er lik 0, sett kø til 0, istedenfor det samme som cykeltid (cykeltid-0)
                if (TimeSpan.Compare(e.OpAndCykAsTimeSpan[0], new TimeSpan(0, 0, 0, 0)) == 0)
                {
                    e.Que = new TimeSpan(0, 0, 0, 0);
                }
                else
                {
                    //hvis op-tid er større enn cyk-tid: 0s kø
                    if (TimeSpan.Compare(e.OpAndCykAsTimeSpan[0], e.OpAndCykAsTimeSpan[1]) == 1)
                    {
                        e.Que = new TimeSpan(0,0,0,0);
                    }
                    else
                    {
                        TimeSpan ts = e.OpAndCykAsTimeSpan[1].Subtract(e.OpAndCykAsTimeSpan[0]);
                        e.Que = ts;
                    } 
                }
            }
        }
    }

    public void DistributeEvents(ProductionEventList productionEventList)
    {
        if (productionEventList.ProductionEvents.Count > 0)                                
        {
            foreach (Event ev in productionEventList.ProductionEvents)
            {
                if (ev.ProductionType == 0 || ev.ProductionType == 110)
                {
                    productionEventList.OtherOrUndefinedEvents.Add(ev);
                } 
                else if (ev.ProductionType == 320)
                {
                    productionEventList.DrillingEvents.Add(ev);
                }
                else if (ev.ProductionType == 330)
                {
                    productionEventList.Fitting1Events.Add(ev);
                }
                else if (ev.ProductionType == 350)
                {
                    productionEventList.Fitting2Events.Add(ev);
                }
                else if (ev.ProductionType == 360)
                {
                    productionEventList.AssemblyEvents.Add(ev);
                }
            }
        }
    }

    public TimeSpan GetTotalQueOnX(List<Event> events)
    {
        TimeSpan totalQue = new TimeSpan();
        if (events.Count != 0)
        {
            totalQue = events[0].Que;
            for (int i = 1; i < events.Count; i++)
            {
                TimeSpan ts = new TimeSpan();
                ts = totalQue.Add(events[i].Que);
                totalQue = ts;
            }
        }
        return totalQue;
    }

    public void AddTotalCykelTime(ProductionEventList productionEventList)
    {
        if (productionEventList.ProductionEvents.Count != 0)
        {
            TimeSpan ts1 = new TimeSpan();
            TimeSpan ts2 = new TimeSpan();
            TimeSpan ts3 = new TimeSpan();
            TimeSpan ts4 = new TimeSpan();
            
            foreach (Event ev in productionEventList.DrillingEvents)
            {
                if (ev.OpAndCykAsTimeSpan.Count != 0)
                {
                    ts1 = ts1.Add(ev.OpAndCykAsTimeSpan[1]);
                }

                productionEventList.TotalDrillingCykelTime = ts1;
            }
            
            foreach (Event ev in productionEventList.Fitting1Events)
            {
                if (ev.OpAndCykAsTimeSpan.Count != 0)
                {
                    ts2 = ts2.Add(ev.OpAndCykAsTimeSpan[1]);
                }

                productionEventList.TotalFitting1CykelTime = ts2;
            }
            
            foreach (Event ev in productionEventList.Fitting2Events)
            {
                if (ev.OpAndCykAsTimeSpan.Count != 0)
                {
                    ts3 = ts3.Add(ev.OpAndCykAsTimeSpan[1]);
                }

                productionEventList.TotalFitting2CykelTime = ts3;
            }
            
            foreach (Event ev in productionEventList.AssemblyEvents)
            {
                if (ev.OpAndCykAsTimeSpan.Count != 0)
                {
                    ts4 = ts4.Add(ev.OpAndCykAsTimeSpan[1]);
                }

                productionEventList.TotalAssemblyCykelTime = ts4;
            }
            
            TimeSpan temp = new TimeSpan();
            temp = temp.Add(productionEventList.TotalDrillingCykelTime);
            temp = temp.Add(productionEventList.TotalFitting1CykelTime);
            temp = temp.Add(productionEventList.TotalFitting2CykelTime);
            temp = temp.Add(productionEventList.TotalAssemblyCykelTime);
            productionEventList.TotalOrderCykelTime = temp;
        }
    }
}
