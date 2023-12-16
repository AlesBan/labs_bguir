using Microsoft.AspNetCore.Mvc;

namespace Lab_8_Calculator.Controllers;

public class CalculatorController : Controller
{
    public IActionResult Index()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Calculate(double num1, double num2, string operation)
    {
        var result = operation switch
        {
            "add" => num1 + num2,
            "subtract" => num1 - num2,
            "multiply" => num1 * num2,
            "divide" => num1 / num2,
            _ => 0
        };

        ViewBag.Result = result;
        return View("Index");
    }
}