using System.Security.Cryptography;
using Microsoft.EntityFrameworkCore;
using Straisimulator.Data;
using Straisimulator.Data.Entities;
using Straisimulator.Models;

namespace Straisimulator.Services;

public class DataFetchService : IDataFetchService
{
    public readonly ApplicationDbContext ApplicationDbContext;
    
    public DataFetchService(ApplicationDbContext applicationDbContext)
    {
        ApplicationDbContext = applicationDbContext;
    }
    
    public ProductionDay FetchProductionDay(DateTime prodDate)
    {

        //henter en tabell med Production-rader som har samme MESProductionDate som parameter "prodDate"
        var production = ApplicationDbContext.Production
            .Where(p => p.OrderNumber != String.Empty)
            .OrderBy(p => p.OrderNumber);
        
        List<Skap> skaps = production.Select(p => new Skap
            {
                ProductionId = p.ProductionId,
                OrderNumber = p.OrderNumber,
                ItemCom = p.Comment,
            })
            .ToList();

        List<Order> orders = new List<Order>();
        
        //holder styr på hvilken ordre loopen er på:
        string lastOrderNumber = string.Empty;
        Order currentOrder = null;
        
        //denne loopen kjører på hver rad i variabelen "production" (hvert skap) og lager nye Order objekter som skapene blir fordelt i
        foreach (Skap skap in skaps)
        {
            if (skap.OrderNumber.Equals(lastOrderNumber) == false)
            {
                if (currentOrder != null)
                {
                    orders.Add(currentOrder);
                }
                currentOrder = new Order();
                currentOrder.OrderNumber = skap.OrderNumber;
                currentOrder.Skap.Add(skap);
                lastOrderNumber = currentOrder.OrderNumber;
            }
            else
            {
                currentOrder.Skap.Add(skap);
            }
        }
        
        ProductionDay productionDay = new ProductionDay()
        {
            Orders = orders
        };
        return productionDay;
    }

    //SO2209158
    public ProductionEventList FetchProductionEvents(string orderId)
    {
        //henter en join mellom ProductionEventLog, ProductionEventType og Production der Production har samme OrderNumber som "orderId"
        var productionEvents = ApplicationDbContext.ProductionEventLog
            .Include(p => p.ProductionTypes)
            .Include(p => p.Production)
            .Where(p => p.Production.OrderNumber == orderId)
            .OrderBy(p => p.ProductionId);

        List<Event> prodEvents = productionEvents.Select(p => new Event
            {
                Id = p.Id,
                TimeStamp = p.TimeStamp,
                ExtraInfo = p.ExtraInfo,
                ProductionId = p.ProductionId,
                ProductionType = p.ProductionType,
                EventType = p.EventType
            })
            .ToList();

        //fjerner Event objekter i prodEvents der ExtraInfo er tom
        for (int i = 0; i < prodEvents.Count; i++)
        {
            if (prodEvents[i].ExtraInfo == String.Empty)
            {
                prodEvents.RemoveAt(i);
            }
        }
        
        //legger til operation- og cykeltid som timespan i hvert event i prodEvents
        DataProcessService dataProcessService = new DataProcessService();
        foreach (Event ev in prodEvents)
        {
            List<TimeSpan> list = dataProcessService.getOpAndCykTime(ev.ExtraInfo);
            
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
        
        dataProcessService.addMachine(prodEvents);
        
        //legger til kø
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

        ProductionEventList productionEventList = new ProductionEventList();
        productionEventList.ProductionEvents = prodEvents;
        
        //fordeler events etter ProductionType (maskin) i lister:
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
            /*
            for (int i = 0; i <= productionEventList.ProductionEvents.Count; i++)
            {
                if (productionEventList.ProductionEvents[i].ProductionType == 0 ||
                    productionEventList.ProductionEvents[i].ProductionType == 110)
                {
                    productionEventList.OtherOrUndefinedEvents.Add(productionEventList.ProductionEvents[i]);
                } 
                else if (productionEventList.ProductionEvents[i].ProductionType == 320)
                {
                    productionEventList.DrillingEvents.Add(productionEventList.ProductionEvents[i]);
                }
                else if (productionEventList.ProductionEvents[i].ProductionType == 330)
                {
                    productionEventList.Fitting1Events.Add(productionEventList.ProductionEvents[i]);
                }
                else if (productionEventList.ProductionEvents[i].ProductionType == 350)
                {
                    productionEventList.Fitting2Events.Add(productionEventList.ProductionEvents[i]);
                }
                else if (productionEventList.ProductionEvents[i].ProductionType == 360)
                {
                    productionEventList.AssemblyEvents.Add(productionEventList.ProductionEvents[i]);
                }
            }*/
        }

        //legger en verdi i productionEventList.TotalOrderQue (samlet kø for hele ordren):
        if (productionEventList.ProductionEvents.Count != 0)
        {
            productionEventList.TotalOrderQue = productionEventList.ProductionEvents[0].Que;
            for (int i = 1; i < productionEventList.ProductionEvents.Count; i++)
            {
                TimeSpan ts = new TimeSpan();
                ts = productionEventList.TotalOrderQue.Add(productionEventList.ProductionEvents[i].Que);
                productionEventList.TotalOrderQue = ts;
            }
        }
        //legger en verdi i productionEventList.TotalDrillingQue:
        if (productionEventList.DrillingEvents.Count != 0)
        {
            productionEventList.TotalDrillingQue = productionEventList.DrillingEvents[0].Que;
            for (int i = 1; i < productionEventList.DrillingEvents.Count; i++)
            {
                TimeSpan ts = new TimeSpan();
                ts = productionEventList.TotalDrillingQue.Add(productionEventList.DrillingEvents[i].Que);
                productionEventList.TotalDrillingQue = ts;
            }
        }
        //legger en verdi i productionEventList.TotalFitting1Que:
        if (productionEventList.Fitting1Events.Count != 0)
        {
            productionEventList.TotalFitting1Que = productionEventList.Fitting1Events[0].Que;
            for (int i = 1; i < productionEventList.Fitting1Events.Count; i++)
            {
                TimeSpan ts = new TimeSpan();
                ts = productionEventList.TotalFitting1Que.Add(productionEventList.Fitting1Events[i].Que);
                productionEventList.TotalFitting1Que = ts;
            }
        }
        //legger en verdi i productionEventList.TotalFitting2Que:
        if (productionEventList.Fitting2Events.Count != 0)
        {
            productionEventList.TotalFitting2Que = productionEventList.Fitting2Events[0].Que;
            for (int i = 1; i < productionEventList.Fitting2Events.Count; i++)
            {
                TimeSpan ts = new TimeSpan();
                ts = productionEventList.TotalFitting2Que.Add(productionEventList.Fitting2Events[i].Que);
                productionEventList.TotalFitting2Que = ts;
            }
        }
        //legger en verdi i productionEventList.TotalAssemblyQue:
        if (productionEventList.AssemblyEvents.Count != 0)
        {
            productionEventList.TotalAssemblyQue = productionEventList.AssemblyEvents[0].Que;
            for (int i = 1; i < productionEventList.AssemblyEvents.Count; i++)
            {
                TimeSpan ts = new TimeSpan();
                ts = productionEventList.TotalAssemblyQue.Add(productionEventList.AssemblyEvents[i].Que);
                productionEventList.TotalAssemblyQue = ts;
            }
        }

        return productionEventList;
    }
}