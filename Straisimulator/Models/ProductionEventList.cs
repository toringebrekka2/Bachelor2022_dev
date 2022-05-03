using Straisimulator.Data.Entities;

namespace Straisimulator.Models;

public class ProductionEventList
{
    public string OrderId { get; set; }
    public List<Event> ProductionEvents { get; set; }
    public List<Event> DrillingEvents { get; set; }
    public List<Event> Fitting1Events { get; set; }
    public List<Event> Fitting2Events { get; set; }
    public List<Event> AssemblyEvents { get; set; }
    public List<Event> OtherOrUndefinedEvents { get; set; }
    public TimeSpan TotalOrderQue { get; set; }
    public TimeSpan TotalDrillingQue { get; set; }
    public TimeSpan TotalFitting1Que { get; set; }
    public TimeSpan TotalFitting2Que { get; set; }
    public TimeSpan TotalAssemblyQue { get; set; }
    public TimeSpan TotalOtherQue { get; set; }

    public ProductionEventList()
    {
        DrillingEvents = new List<Event>();
        Fitting1Events = new List<Event>();
        Fitting2Events = new List<Event>();
        AssemblyEvents = new List<Event>();
        OtherOrUndefinedEvents = new List<Event>();
    }
}