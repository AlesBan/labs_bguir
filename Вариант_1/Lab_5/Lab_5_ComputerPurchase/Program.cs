using Lab_5_ComputerPurchase;
using Lab_5_ComputerPurchase.Interfaces;
using Lab_5_ComputerPurchase.Logging;
using Lab_5_ComputerPurchase.Models;


var computerStorage = new ComputerStorage();
var computerShopManager = new PurchaseManager(computerStorage);
var purchaseLogger = new FilePurchaseLogger();

computerStorage.ParseCompTitlesAndAddCompBoxes();

while (true)
{
    Console.WriteLine("Что я могу для вас сделать?");
    Console.WriteLine("1. Показать доступные компьютеры");
    Console.WriteLine("2. Заказать компьютеры");
    Console.WriteLine("3. Списать компьютеры со склада");
    Console.WriteLine("4. Выйти");

    Console.Write("Введите ваш выбор: ");
    var choice = Console.ReadLine();

    switch (choice)
    {
        case "1":
            var availableComputers = computerShopManager.GetAllComputers();
            Console.WriteLine("Доступные компьютеры:");
            foreach (var computer in availableComputers)
            {
                Console.WriteLine(computer.Info());
            }

            break;

        case "2":
            OrderComputers(computerShopManager, purchaseLogger);
            break;

        case "3":
            WriteOffComputers(computerShopManager, purchaseLogger);
            break;

        case "4":
            Console.WriteLine("Спасибо за посещение нашего магазина компьютеров. До свидания!");
            return;

        default:
            Console.WriteLine("Некорректный выбор!");
            break;
    }

    Console.WriteLine();
}

static void OrderComputers(IPurchaseManager purchaseManager, IPurchaseLogger purchaseLogger)
{
    var selectedComputers = new List<Computer>();

    while (true)
    {
        ComputerBox? selectedComputer = null;

        while (selectedComputer == null)
        {
            Console.Write(
                "Введите модель компьютера, который вы хотите заказать (или 'q' для завершения заказа): ");
            var computerModel = Console.ReadLine() ?? "".Trim();

            if (computerModel == "q")
            {
                break;
            }

            selectedComputer = purchaseManager.GetComputerBoxByTitle(computerModel);

            if (selectedComputer == null)
            {
                Console.WriteLine("Некорректная модель компьютера!");
            }
        }

        Console.WriteLine(
            $"Модель: {selectedComputer!.CompTitle}, Цена: {selectedComputer.Price}, Количество на складе: {selectedComputer.Count}");

        int quantity;
        while (true)
        {
            Console.Write("Введите количество, которое вы хотите заказать: ");
            if (!int.TryParse(Console.ReadLine(), out quantity) || quantity <= 0)
            {
                Console.WriteLine("Некорректное количество!");
            }
            else
            {
                break;
            }
        }

        var computersToOrder = Enumerable.Range(0, quantity)
            .Select(_ => new Computer { Title = selectedComputer.CompTitle, Price = selectedComputer.Price })
            .ToList();

        selectedComputers.AddRange(computersToOrder);
        Console.WriteLine($"Выбрано: {quantity}, Общая стоимость: {selectedComputer.Price * quantity}");
        Console.WriteLine("Компьютеры успешно добавлены в заказ!");

        Console.Write("Хотите продолжить выбор компьютеров? (Д/Н) (Y/N): ");
        if (!IsPositiveResponse(Console.ReadLine() ?? "".Trim()))
        {
            break;
        }
    }

    if (!selectedComputers.Any())
    {
        return;
    }

    Console.WriteLine("Вы выбрали следующие компьютеры:");
    foreach (var computer in selectedComputers)
    {
        Console.WriteLine($"Модель: {computer.Title}, Цена: {computer.Price}");
    }

    Console.WriteLine("Общая стоимость: " + selectedComputers.Sum(computer => computer.Price));

    Console.Write("Подтвердить заказ (Д/Н) (Y/N): ");
    if (!IsPositiveResponse(Console.ReadLine() ?? "".Trim()))
    {
        return;
    }

    purchaseManager.PurchaseComputers(selectedComputers);
    Console.WriteLine("Заказ успешно оформлен!");

    var purchaseTotal = selectedComputers.Sum(computer => computer.Price);
    purchaseLogger.LogPurchase(selectedComputers.Select(c => c.Title), purchaseTotal);
}

static void WriteOffComputers(IPurchaseManager purchaseManager, FilePurchaseLogger purchaseLogger)
{
    var selectedComputers = new List<Computer>();

    while (true)
    {
        ComputerBox? selectedComputerBox = null;

        while (selectedComputerBox == null)
        {
            Console.Write(
                "Введите модель компьютера, который вы хотите списать (или 'q' для завершения списания): ");
            var computerModel = Console.ReadLine() ?? "".Trim();

            if (computerModel == "q")
            {
                break;
            }

            selectedComputerBox = purchaseManager.GetComputerBoxByTitle(computerModel);

            if (selectedComputerBox == null)
            {
                Console.WriteLine("Некорректная модель компьютера!");
            }
        }

        if (selectedComputerBox == null)
        {
            break;
        }

        Console.WriteLine(
            $"Модель: {selectedComputerBox.CompTitle}, Цена: {selectedComputerBox.Price}, Количество на складе: {selectedComputerBox.Count}");

        int quantity;
        while (true)
        {
            Console.Write("Введите количество, которое вы хотите списать: ");
            if (!int.TryParse(Console.ReadLine(), out quantity) || quantity <= 0)
            {
                Console.WriteLine("Некорректное количество!");
            }
            else if (quantity > selectedComputerBox.Count)
            {
                Console.WriteLine("Недостаточное количество на складе!");
            }
            else
            {
                break;
            }
        }

        var compsToWriteOff = Enumerable.Range(0, quantity)
            .Select(_ => new Computer() { Title = selectedComputerBox.CompTitle, Price = selectedComputerBox.Price })
            .ToList();

        purchaseManager.WriteOffComputers(compsToWriteOff);
        Console.WriteLine("Компьютеры успешно списаны со склада!");

        Console.Write("Хотите продолжить списание компьютеров? (Д/Н) (Y/N): ");
        if (!IsPositiveResponse(Console.ReadLine() ?? "".Trim()))
        {
            break;
        }
    }

    if (selectedComputers.Any())
    {
        Console.WriteLine("Вы списали следующие компьютеры:");
        foreach (var computer in selectedComputers)
        {
            Console.WriteLine($"Модель: {computer.Title}, Цена: {computer.Price}");
        }

        Console.Write("Подтвердить списание компьютеров? (Д/Н) (Y/N): ");
        if (IsPositiveResponse(Console.ReadLine() ?? "".Trim()))
        {
            purchaseManager.WriteOffComputers(selectedComputers);
            Console.WriteLine("Списание успешно выполнено!");

            purchaseLogger.LogPurchase(selectedComputers.Select(c => c.Title),
                selectedComputers.Sum(computer => computer.Price));
        }
        else
        {
            Console.WriteLine("Списание отменено.");
        }
    }
    else
    {
        Console.WriteLine("Вы не выбрали ни одного компьютера. Списание отменено.");
    }
}

static bool IsPositiveResponse(string input)
{
    input = input.ToLowerInvariant();
    return input == "д" || input == "да" || input == "y" || input == "yes";
}