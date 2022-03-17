using Microsoft.EntityFrameworkCore;
using Straisimulator.Data;
using Straisimulator.Data.Entities;
using Straisimulator.Models;

namespace Straisimulator.Services;

public class DataFetchService
{
    public readonly ApplicationDbContext ApplicationDbContext;
    
    public DataFetchService(ApplicationDbContext applicationDbContext)
    {
        ApplicationDbContext = applicationDbContext;
    }

    // TODO: flette inn productionEvents i ProductionDay for å hente kø-tid
    public ProductionDay FetchProductionDay(DateTime prodDate)
    {
        //henter en tabell med Production-rader som har samme MESProductionDate som parameter "prodDate":
        var production = ApplicationDbContext.Production
            .Where(p => p.MESProductionDate == prodDate)
            .OrderBy(p => p.ProductionSequence);

        //henter en join mellom Production og ProductionEventLog der Production har samme MESProductionDate som "prodDate"
        var productionEvents = ApplicationDbContext.ProductionEventLog
            .Include(p => p.Production)
            .Where(p => p.Production.MESProductionDate == prodDate);

        //lager ett Skap objekt for hver rad i "production" og legger i en liste:
        List<Skap> skaps = production.Select(p => new Skap
            {
                OrderNumber = p.OrderNumber,
                ItemCom = p.Comment,
                
            })
            .ToList();
        
        //oppretter en liste med denne dagens ordrer:
        List<Order> orders = new List<Order>();
        
        //holder styr på hvilken ordre loopen er på:
        string lastOrderNumber = string.Empty;
        Order currentOrder = null;

        //denne loopen kjører på hver rad i variabelen "production" (hvert skap) og lager nye Order objekter som skapene blir fordelt i:
        foreach (Skap skap in skaps)
        {
            if (skap.OrderNumber.Equals(lastOrderNumber) == false )
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
        
        //til slutt returneres denne dagens produksjon (et ProductionDay objekt som holder lista med denne dagens ordrer (som hver holder en liste med skap)):
        return productionDay;
    }
}