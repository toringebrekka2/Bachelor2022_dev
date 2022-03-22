namespace Straisimulator.Data.Entities;

public class ProductionEventLog
{
    public int Id { get; set; }
    public Production Production { get; set; }
    public DateTime TimeStamp { get; set; }
    public ProductionEventTypes EventType { get; set; }
}