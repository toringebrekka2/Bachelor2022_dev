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
        return View();
    }

    public IActionResult HentEventLog()
    {
        return View();
    }
    
    public IActionResult HentEventLogError()
    {
        return View();
    }
    

    public IActionResult HentEventLogRes(String orderId)
    {
        ProductionEventList productionEvents = _dataFetchService.FetchProductionEvents(orderId);
        if (productionEvents.ProductionEvents.Count == 0)
        {
            return View("HentEventLogError");
        }
        else
        {
            HentEventLogResViewModel model = new HentEventLogResViewModel();
            model.ProductionEventList = productionEvents;
            model.ProductionEventList.OrderId = orderId;
            return View(model);
        }
    }

    public IActionResult Statistikk()
    {
        return View();
    }
    public IActionResult StatistikkRes(string orderId)
    {
        ProductionEventList productionEvents = _dataFetchService.FetchProductionEvents(orderId);
        if (productionEvents.ProductionEvents.Count == 0)
        {
            return View("HentEventLogError");
        }
        else
        {
            StatistikkResViewModel model = new StatistikkResViewModel();
            model.ProductionEventList = productionEvents;
            model.ProductionEventList.OrderId = orderId;
            return View(model);
        }
    }
    
    
    public IActionResult Login()
    {
        return View();
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