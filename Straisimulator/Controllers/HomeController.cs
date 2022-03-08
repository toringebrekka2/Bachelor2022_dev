using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Straisimulator.Data;
using Straisimulator.Models;
using Straisimulator.Services;
using Straisimulator.ViewModels;

namespace Straisimulator.Controllers;

public class HomeController : Controller
{
    private readonly DataFetchService _dataFetchService;
    private readonly ILogger<HomeController> _logger;

    public HomeController(DataFetchService dataFetchService, ILogger<HomeController> logger)
    {
        _dataFetchService = dataFetchService;
        _logger = logger;
    }

    public IActionResult Index(DateTime prodDate)
    {
        var productionDay = _dataFetchService.FetchProductionDay(prodDate);
        IndexViewModel model = new IndexViewModel();
        model.ProductionDay = productionDay;
        
        return View(model);
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel {RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier});
    }
}