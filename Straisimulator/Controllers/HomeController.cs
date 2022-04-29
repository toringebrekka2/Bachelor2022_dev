using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Straisimulator.Data;
using Straisimulator.Models;
using Straisimulator.Services;
using Straisimulator.ViewModels;
using System.Linq;
using System.Collections.Generic;

namespace Straisimulator.Controllers;

public class HomeController : Controller
{
    private IDataFetchService _dataFetchService;
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger, IDataFetchService dataFetchService)
    {
        _logger = logger;
        _dataFetchService = dataFetchService;
    }

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult About()
    {
        //DateTime prodDay = new DateTime(prodDay1, prodDay2, prodDay3);
        //var productionDay = _dataFetchService.FetchProductionDay(prodDay);
       // ResultViewModel model = new ResultViewModel();
       // model.ProductionDay = productionDay; 
        return View();
    }
    
    public IActionResult HentEventLog()
    {
        return View();
    }

    public IActionResult HentEventLogRes(String orderId)
    {
        ProductionEventList productionEvents = _dataFetchService.FetchProductionEvents(orderId);
        if (productionEvents.ProductionEvents.Count == 0)
        {
            throw new InvalidOperationException("There are no events in the list");
        }
        else
        {
            HentEventLogResViewModel model = new HentEventLogResViewModel();
            model.ProductionEventList = productionEvents;
            return View(model);
        }
    }

    public IActionResult Resultater()
    {
        
        List<SkapVegg> skap = new List<SkapVegg>();
        
        SkapVegg skap1 = new SkapVegg(8333, "Vetilatorskap for Slimline H:72,0 cm", 82, 65, 83, 67, 64, 32,65, 54);
        SkapVegg skap2 = new SkapVegg(8332, "Overskap 80 cm m/2 dører H:72.0 cm", 85, 89, 86, 90,64, 32,65, 54);
        SkapVegg skap3 = new SkapVegg(8331, "Overskap 80 cm m/2 dører H:72.0 cm", 85, 81, 86, 82,64, 32,65, 54);
        SkapVegg skap4 = new SkapVegg(8330, "Overskap 80 cm m/2 dører H:72.0 cm", 80, 89, 81, 91,64, 32,65, 54);
        SkapVegg skap5 = new SkapVegg(8329, "Overskap 70 cm m/2 dører Retning = Venstre", 63, 71, 65, 72,64, 32,65, 54);
        SkapVegg skap6 = new SkapVegg(8328, "Kolonialseksjon 80 cm m/3 skuffer", 120, 82, 137, 137, 64, 32, 65, 54);
        SkapVegg skap7 = new SkapVegg(8327, "Hjørnebenk 100x60 cm m/50 cm dør", 49, 53, 71, 54, 64, 32, 65, 54);
        skap.Add(skap1);
        skap.Add(skap2);
        skap.Add(skap3);
        skap.Add(skap4);
        skap.Add(skap5);
        skap.Add(skap6);
        skap.Add(skap7);

        ResultaterViewModel model = new ResultaterViewModel();
        model.Skap = skap;

        return View(model);
        
    }

    public IActionResult Simulator()
    {
       return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel {RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier});
    }
}