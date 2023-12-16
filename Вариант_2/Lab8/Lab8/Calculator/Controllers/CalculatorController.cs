using System;
using System.Data;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace Calculator.Controllers
{
    public class CalculatorController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Calculate(string expression)
        {
            if (!string.IsNullOrEmpty(expression))
            {
                try
                {
                    var result = new DataTable().Compute(expression, null);
                    return Json(new { result = result.ToString() });
                }
                catch (Exception ex)
                {
                    return Json(new { result = "Ошибка: " + ex.Message });
                }
            }
            else
            {
                return Json(new { result = "Введите выражение" });
            }
        }
    }
}