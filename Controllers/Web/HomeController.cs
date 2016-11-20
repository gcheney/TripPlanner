using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using TripPlanner.Services;
using TripPlanner.ViewModels;
using TripPlanner.Data;

namespace TripPlanner.Controllers.Web
{
  public class HomeController : Controller
  {
    private IMailService _mailService;
    private IConfigurationRoot _config;
    private ITripPlannerRepository _repo;

    public HomeController(IMailService mailService, IConfigurationRoot config, 
      ITripPlannerRepository repo)
    {
      _mailService = mailService;
      _config = config;
      _repo = repo;
    }

    public IActionResult Index()
    {
      var data = _repo.GetAllTrips();
      return View(data);
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
