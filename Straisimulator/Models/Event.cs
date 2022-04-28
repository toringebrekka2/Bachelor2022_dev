using Straisimulator.Data.Entities;

namespace Straisimulator.Models;

public class Event
{
    public int Id { get; set; }
    public DateTime TimeStamp { get; set; }
    public string ExtraInfo { get; set; }
    public int ProductionId { get; set; }
    public List<TimeSpan> OpAndCykAsTimeSpan { get; set; }
    public int ProductionType { get; set; }
    public string Maskin { get; set; }
    public int EventType { get; set; }
    
    public TimeSpan Que { get; set; }
}