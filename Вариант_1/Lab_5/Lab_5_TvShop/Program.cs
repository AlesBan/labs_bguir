using Lab_5_TvShop;
using Lab_5_TvShop.Logging;
using Lab_5_TvShop.Models;

var tvShopRepository = new TvShopRepository();
var tvShopManager = new SaleManager(tvShopRepository);
var salesLogger = new FileSalesLogger();

tvShopRepository.ParseTvTitlesAndAddTvBoxes();

while (true)
{
    Console.WriteLine("Добро пожаловать в наш магазин телевизоров!");
    Console.WriteLine("Что я могу для вас сделать?");
    Console.WriteLine("1. Показать доступные телевизоры");
    Console.WriteLine("2. Купить телевизоры");
    Console.WriteLine("3. Добавить новые телевизоры на склад");
    Console.WriteLine("4. Выйти");

    Console.Write("Введите ваш выбор: ");
    var choice = Console.ReadLine();

    switch (choice)
    {
        case "1":
            var availableTvs = tvShopManager.GetAllTvs();
            Console.WriteLine("Доступные телевизоры:");
            foreach (var tvBox in availableTvs)
            {
                Console.WriteLine(tvBox.Info());
            }

            break;

        case "2":
            Customer.BuyTvs(tvShopManager, tvShopRepository, salesLogger);
            break;

        case "3":
            var newTvTitle = string.Empty;
            while (string.IsNullOrEmpty(newTvTitle))
            {
                Console.Write("Введите название нового телевизора: ");
                newTvTitle = Console.ReadLine()?.Trim();
            }

            var newTvPrice = 0;
            while (newTvPrice <= 0)
            {
                Console.Write("Введите цену нового телевизора: ");
                var priceStr = Console.ReadLine() ?? string.Empty;
                if (!int.TryParse(priceStr, out newTvPrice) || newTvPrice <= 0)
                {
                    Console.WriteLine("Некорректная цена!");
                }
            }

            var newTvQuantity = 0;
            while (newTvQuantity <= 0)
            {
                Console.Write("Введите количество новых телевизоров: ");
                var quantityStr = Console.ReadLine() ?? string.Empty;
                if (!int.TryParse(quantityStr, out newTvQuantity) || newTvQuantity <= 0)
                {
                    Console.WriteLine("Некорректное количество!");
                }
            }

            var newTvBox = new TvBox
            {
                TvTitle = newTvTitle,
                Price = newTvPrice,
                Count = newTvQuantity
            };

            tvShopManager.AddTvBox(newTvBox);
            Console.WriteLine("Новый телевизор добавлен на склад!");
            break;

        case "4":
            Console.WriteLine("Спасибо за посещение нашего магазина телевизоров. До свидания!");
            return;

        default:
            Console.WriteLine("Некорректный выбор!");
            break;
    }

    Console.WriteLine();
}