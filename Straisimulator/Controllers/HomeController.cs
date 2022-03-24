using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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

    public IActionResult Resultater()
    {
        
        List<SkapVegg> skap = new List<SkapVegg>();
        
        SkapVegg skap1 = new SkapVegg(1, "Overskap 50cm", 58, 65, 153, 212);
        SkapVegg skap2 = new SkapVegg(2, "Overskap 60cm", 65, 87, 84, 233);
        skap.Add(skap1);
        skap.Add(skap2);
        
        
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