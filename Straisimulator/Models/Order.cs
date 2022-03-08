namespace Straisimulator.Models;

public class Order
{
    public List<Skap> Skaps { get; set; }

    public Order(List<Skap> skaps)
    {
        Skaps = skaps;
    }
}