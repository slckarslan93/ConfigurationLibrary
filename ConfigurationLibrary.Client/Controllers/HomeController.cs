using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using ConfigurationLibrary.UI.Models;
using Microsoft.AspNetCore.Authorization;

namespace ConfigurationLibrary.UI.Controllers;

[Authorize]
public class HomeController : Controller
{
    [HttpGet]
    public IActionResult Index()
    {
        return View();
    }

  
}
