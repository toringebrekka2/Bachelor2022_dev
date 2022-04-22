namespace Straisimulator.Models;

public class Event
{
    public int Id { get; set; }
    public DateTime TimeStamp { get; set; }
    public string ExtraInfo { get; set; }
    public int ProductionId { get; set; }
    public List<TimeSpan> OpAndCykAsTimeSpan { get; set; }
}