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
        //TROR IKKE DENNE FUNKER
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
                    //hvis op-tid er større enn cyk-tid - 0s kø
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
        return productionEventList;
    }
}