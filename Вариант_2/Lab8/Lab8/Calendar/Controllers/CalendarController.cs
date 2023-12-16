using System;
using Microsoft.AspNetCore.Mvc;

namespace Calendar.Controllers
{
    public class CalendarController : Controller
    {
        public IActionResult Index(bool? showDate = false, bool? showTime = false)
        {
            DateTime currentDate = DateTime.Now;
            ViewBag.ShowDate = showDate ?? false;
            ViewBag.ShowTime = showTime ?? false;
            ViewBag.CurrentDate = currentDate;
            return View();
        }
    }
}