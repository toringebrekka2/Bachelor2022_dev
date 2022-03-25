using Straisimulator.Models;

namespace Straisimulator.Services;

public interface IDataFetchService
{
    ProductionDay FetchProductionDay(DateTime prodDate);
}