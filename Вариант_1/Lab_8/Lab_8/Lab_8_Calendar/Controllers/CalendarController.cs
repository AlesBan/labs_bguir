using Microsoft.AspNetCore.Mvc;

namespace Lab_8_Calendar.Controllers;

public class CalendarController : Controller
{
    public IActionResult Index(bool? showDate, bool? showTime)
    {
        var currentDateTime = DateTime.Now;

        var displayDate = showDate ?? true;
        var displayTime = showTime ?? true;

        ViewData["ShowDate"] = displayDate;
        ViewData["ShowTime"] = displayTime;
        ViewData["CurrentDateTime"] = currentDateTime;

        return View();
    }
}