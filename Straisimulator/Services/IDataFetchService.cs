using Straisimulator.Data.Entities;
using Straisimulator.Models;

namespace Straisimulator.Services;

public interface IDataFetchService
{
    ProductionDay FetchProductionDay(DateTime prodDate);
    ProductionEventList FetchProductionEvents(String orderId);
}