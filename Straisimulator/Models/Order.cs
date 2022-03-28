namespace Straisimulator.Models;

public class Order
{
    public List<Skap> Skap { get; set; }
    public string OrderNumber { get; set; }

    public Order()
    {
        Skap = new List<Skap>();
    }
}