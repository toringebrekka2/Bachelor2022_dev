using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Straisimulator.Data;
using Straisimulator.Models;
using Straisimulator.Services;
using Straisimulator.ViewModels;

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

    public IActionResult Result(int prodDay1, int prodDay2, int prodDay3)
    {
        DateTime prodDay = new DateTime(prodDay1, prodDay2, prodDay3);
        var productionDay = _dataFetchService.FetchProductionDay(prodDay);
        ResultViewModel model = new ResultViewModel();
        model.ProductionDay = productionDay;
        return View(model);
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel {RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier});
    }
}