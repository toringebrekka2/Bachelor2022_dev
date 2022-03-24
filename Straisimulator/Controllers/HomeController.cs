using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Straisimulator.Data;
using Straisimulator.Models;
//using Straisimulator.Services;
using Straisimulator.ViewModels;

namespace Straisimulator.Controllers;

public class HomeController : Controller
{
    /*private ApplicationDbContext _applicationDbContext;
    private DataFetchService _dataFetchService;*/
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger/*, ApplicationDbContext applicationDbContext*/)
    {
        _logger = logger;
        /*_applicationDbContext = applicationDbContext;*/
    }

    public IActionResult Index(/*DateTime prodDay*/)
    {
        /*
        _dataFetchService = new DataFetchService(_applicationDbContext);
        var productionDay = _dataFetchService.FetchProductionDay(prodDay);
        IndexViewModel model = new IndexViewModel();
        model.ProductionDay = productionDay;*/
        
        return View(/*model*/);
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel {RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier});
    }
}