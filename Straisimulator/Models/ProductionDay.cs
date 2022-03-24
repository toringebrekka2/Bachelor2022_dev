using Straisimulator.Data.Entities;
namespace Straisimulator.Models;

public class ProductionDay
{
    public List<Order> Orders { get; set; }
    public List<ProductionEvent> Events { get; set; }
}