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

        //henter en tabell med Production-rader som har samme MESProductionDate som parameter "prodDate":
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
        //denne loopen kjører på hver rad i variabelen "production" (hvert skap) og lager nye Order objekter som skapene blir fordelt i:
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
            //Events = prodEvents
        };
        return productionDay;
    }

    public ProductionEventList fetchProductionEvents(string OrderId)
    {
        //henter en join mellom Production og ProductionEventLog der Production har samme MESProductionDate som "prodDate":
        var productionEvents = ApplicationDbContext.ProductionEvent
            .Include(p => p.ProductionEventType)
            .Include(p => p.Production)
            .Where(p => p.Production.OrderNumber == OrderId);
        
        List<ProductionEvent> prodEvents = productionEvents.Select(p => new ProductionEvent
            {
                Id = p.Id,
                TimeStamp = p.TimeStamp,
                ExtraInfo = p.ExtraInfo,
                ProductionId = p.ProductionId,
                EventType = p.EventType
            })
            .ToList();
        
        ProductionEventList productionEventList = new ProductionEventList();
        productionEventList.ProductionEvents = prodEvents;
        return productionEventList;
    }

    //henter hele productionEventTypes tabellen, men tar kun med EventType (som er id):
    /*var productionEventTypes = ApplicationDbContext.ProductionEventTypes

    /*List<ProductionEventTypes> eventTypes = productionEventTypes.Select(e => new ProductionEventTypes
        {
            EventType = e.EventType,
            DescriptionE = e.DescriptionE,
        })
        .ToList();*/
}