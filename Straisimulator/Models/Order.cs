namespace Straisimulator.Models;

public class Order
{
    public List<Skap> Skap { get; set; }

    public Order(List<Skap> skap)
    {
        Skap = skap;
    }
}