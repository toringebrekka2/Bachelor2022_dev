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
                ProductionId = (int) p.ProductionId,
                ProductionType = p.ProductionType,
                EventType = p.EventType
            })
            .ToList();

        DataProcessService dataProcessService = new DataProcessService();
        
        //fjerner Event objekter i prodEvents der ExtraInfo er tom
        dataProcessService.RemoveEmptyExtraInfo(prodEvents);
        
        //legger til operation- og cykeltid som timespan i hvert event i prodEvents:
        dataProcessService.AddOpAndCykAsTimeSpan(prodEvents);

        dataProcessService.AddMachine(prodEvents);

        dataProcessService.AddQue(prodEvents);
        
        ProductionEventList productionEventList = new ProductionEventList();
        productionEventList.ProductionEvents = prodEvents;
        
        //fordeler events etter ProductionType (maskin) i lister:
        dataProcessService.DistributeEvents(productionEventList);
        
        
        //legger en verdi i productionEventList.TotalOrderQue (samlet kø for hele ordren):
        productionEventList.TotalOrderQue = dataProcessService.GetTotalQueOnX(productionEventList.ProductionEvents);
        
        //legger en verdi i productionEventList.TotalDrillingQue:
        productionEventList.TotalDrillingQue = dataProcessService.GetTotalQueOnX(productionEventList.DrillingEvents);
        
        //legger en verdi i productionEventList.TotalFitting1Que:
        productionEventList.TotalFitting1Que = dataProcessService.GetTotalQueOnX(productionEventList.Fitting1Events);
        
        //legger en verdi i productionEventList.TotalFitting2Que:
        productionEventList.TotalFitting2Que = dataProcessService.GetTotalQueOnX(productionEventList.Fitting2Events);
        
        //legger en verdi i productionEventList.TotalAssemblyQue:
        productionEventList.TotalAssemblyQue = dataProcessService.GetTotalQueOnX(productionEventList.AssemblyEvents);

        //legger en verdi i TotalXCykelTime og TotalOrderCykelTime:
        dataProcessService.AddTotalCykelTime(productionEventList);
        
        return productionEventList;
    }
    
    public ProductionEventList FetchEventsWithDate(DateTime date)
    {
        //henter en join mellom ProductionEventLog, ProductionEventType og Production der TimeStamp i førstnevnte er lik 'date'"
        var productionEvents = ApplicationDbContext.ProductionEventLog
            .Include(p => p.ProductionTypes)
            .Include(p => p.Production)
            .Where(p => p.TimeStamp.Date == date.Date && p.ProductionId != null)
            .OrderBy(p => p.TimeStamp).ToList();

        List<Event> prodEvents = productionEvents.Select(p => new Event
            {
                Id = p.Id,
                TimeStamp = p.TimeStamp,
                ExtraInfo = p.ExtraInfo,
                ProductionId = (int) p.ProductionId!,
                ProductionType = p.ProductionType,
                EventType = p.EventType
            })
            .ToList();

        DataProcessService dataProcessService = new DataProcessService();
        
        //fjerner Event objekter i prodEvents der ExtraInfo er tom
        dataProcessService.RemoveEmptyExtraInfo(prodEvents);
        
        //legger til operation- og cykeltid som timespan i hvert event i prodEvents:
        dataProcessService.AddOpAndCykAsTimeSpan(prodEvents);

        dataProcessService.AddMachine(prodEvents);

        dataProcessService.AddQue(prodEvents);
        
        ProductionEventList productionEventList = new ProductionEventList();
        productionEventList.ProductionEvents = prodEvents;
        
        //fordeler events etter ProductionType (maskin) i lister:
        dataProcessService.DistributeEvents(productionEventList);
        
        
        //legger en verdi i productionEventList.TotalOrderQue (samlet kø for hele ordren):
        productionEventList.TotalOrderQue = dataProcessService.GetTotalQueOnX(productionEventList.ProductionEvents);
        
        //legger en verdi i productionEventList.TotalDrillingQue:
        productionEventList.TotalDrillingQue = dataProcessService.GetTotalQueOnX(productionEventList.DrillingEvents);
        
        //legger en verdi i productionEventList.TotalFitting1Que:
        productionEventList.TotalFitting1Que = dataProcessService.GetTotalQueOnX(productionEventList.Fitting1Events);
        
        //legger en verdi i productionEventList.TotalFitting2Que:
        productionEventList.TotalFitting2Que = dataProcessService.GetTotalQueOnX(productionEventList.Fitting2Events);
        
        //legger en verdi i productionEventList.TotalAssemblyQue:
        productionEventList.TotalAssemblyQue = dataProcessService.GetTotalQueOnX(productionEventList.AssemblyEvents);

        //legger en verdi i TotalXCykelTime og TotalOrderCykelTime:
        dataProcessService.AddTotalCykelTime(productionEventList);
        
        return productionEventList;
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
}