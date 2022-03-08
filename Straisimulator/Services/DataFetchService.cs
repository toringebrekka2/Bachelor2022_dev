using Microsoft.EntityFrameworkCore;
using Straisimulator.Data;
using Straisimulator.Data.Entities;
using Straisimulator.Models;

namespace Straisimulator.Services;

public class DataFetchService
{
    public readonly ApplicationDbContext _applicationDbContext;
    
    public DataFetchService(ApplicationDbContext applicationDbContext)
    {
        _applicationDbContext = applicationDbContext;
    }

    public ProductionDay FetchProductionDay(DateTime proddate)
    {
        var production = _applicationDbContext.Production
            .Where(p => p.MESProductionDate == proddate)
            .OrderBy(p => p.ProductionSequence);

        var productionEvents = _applicationDbContext.ProductionEventLog
            .Include(p => p.Production)
            .Where(p => p.Production.MESProductionDate == proddate);

        List<Skap> skaps = production.Select(p => new Skap
            {
                Itmcod = p.Comment,
                Height = p.Height
            })
            .ToList();

        List<Order> orders = new List<Order>();
        string lastOrderNo = string.Empty;
        Order currentOrder = null;
        List<Skap> currentSkaps = new List<Skap>();
        foreach (Skap skap in skaps)
        {
            if (skap.OrderNo != lastOrderNo)
            {
                currentOrder = new Order(currentSkaps);

                orders.Add(currentOrder);

                currentOrder = skap.OrderNo;
            }
        }

        ProductionDay productionDay = new ProductionDay()
        {
            Orders = orders
        };

        return productionDay;
    }
}