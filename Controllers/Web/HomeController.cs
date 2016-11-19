using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using TripPlanner.Services;
using TripPlanner.ViewModels;

namespace TripPlanner.Controllers.Web
{
  public class HomeController : Controller
  {
    private IMailService _mailService;
    private IConfigurationRoot _config;

    public HomeController(IMailService mailService, IConfigurationRoot config)
    {
      _mailService = mailService;
      _config = config;
    }

    public IActionResult Index()
    {
      return View();
    }

    public IActionResult Contact()
    {
      return View();
    }

    [HttpPost]
    public IActionResult Contact(ContactViewModel model)
    {
      if (model.Email.Contains("aol.com"))
      {
        ModelState.AddModelError("", "We don't support AOL addresses");
      }

      if (ModelState.IsValid)
      {
        _mailService.SendMail(_config["MailSettings:ToAddress"], 
          model.Email, "From Trip Planner", model.Message);

        ModelState.Clear();

        ViewBag.UserMessage = "Message Sent!";
      }

      return View();
    }

    public IActionResult About()
    {
      return View();
    }
  }
}
