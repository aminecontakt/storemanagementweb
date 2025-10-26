using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using StoreManagementWeb.Models;
using StoreManagementWeb.Helpers;

namespace StoreManagementWeb.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly DatabaseHelper _databaseHelper;

    public HomeController(ILogger<HomeController> logger, DatabaseHelper databaseHelper)
    {
        _logger = logger;
        _databaseHelper = databaseHelper;
    }

    public IActionResult Index()
    {
        try
        {
            _databaseHelper.CreateTableIfNotExists();
            ViewBag.Message = "Connexion à la base de données réussie !";
        }
        catch (Exception ex)
        {
            ViewBag.Error = $"Erreur de connexion : {ex.Message}";
            _logger.LogError(ex, "Erreur de connexion à la base de données");
        }
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
