using Lab_5_TvShop.Logging;

namespace Lab_5_TvShop.Models
{
    public static class Customer
    {
        public static void BuyTvs(SaleManager saleManager, TvShopRepository tvShopRepository,
            FileSalesLogger salesLogger)
        {
            var selectedTvs = new List<Tv>();

            while (true)
            {
                string tvTitle;
                TvBox? selectedTvBox = null;

                while (selectedTvBox == null)
                {
                    Console.Write(
                        "Введите название телевизора, который вы хотите купить (или 'q' для завершения покупки): ");
                    tvTitle = Console.ReadLine() ?? "".Trim();

                    if (tvTitle == "q")
                    {
                        break;
                    }

                    selectedTvBox = tvShopRepository.GetTvBoxByTitle(tvTitle);

                    if (selectedTvBox == null)
                    {
                        Console.WriteLine("Некорректное название телевизора!");
                    }
                }

                Console.WriteLine(
                    $"Название: {selectedTvBox!.TvTitle}, Цена: {selectedTvBox.Price}, Количество на складе: {selectedTvBox.Count}");

                int quantity;
                while (true)
                {
                    Console.Write("Введите количество, которое вы хотите купить: ");
                    if (!int.TryParse(Console.ReadLine(), out quantity) || quantity <= 0)
                    {
                        Console.WriteLine("Некорректное количество!");
                    }
                    else if (quantity > selectedTvBox.Count)
                    {
                        Console.WriteLine(
                            "Указанное количество превышает доступное количество на складе. Пожалуйста, выберите меньшее количество.");
                    }
                    else
                    {
                        break;
                    }
                }

                var tvsToBuy = Enumerable.Range(0, quantity)
                    .Select(_ => new Tv { Title = selectedTvBox.TvTitle, Price = selectedTvBox.Price })
                    .ToList();

                selectedTvs.AddRange(tvsToBuy);
                Console.WriteLine($"Выбрано: {quantity}, Общая стоимость: {selectedTvBox.Price * quantity}");
                Console.WriteLine("Телевизоры успешно добавлены в корзину!");

                Console.Write("Хотите продолжить выбор телевизоров? (Д/Н) (Y/N): ");
                if (!IsPositiveResponse(Console.ReadLine() ?? "".Trim()))
                {
                    break;
                }
            }

            if (selectedTvs.Any())
            {
                Console.WriteLine("Вы выбрали следующие телевизоры:");
                foreach (var tv in selectedTvs)
                {
                    Console.WriteLine($"Название: {tv.Title}, Цена: {tv.Price}");
                }
                Console.WriteLine("Общая стоимость: " + selectedTvs.Sum(tv => tv.Price));

                Console.Write("Подтвердить покупку (Д/Н) (Y/N): ");
                if (IsPositiveResponse(Console.ReadLine() ?? "".Trim()))
                {
                    saleManager.SellTvs(selectedTvs);
                    Console.WriteLine("Покупка успешно завершена!");

                    var tvTitleList = selectedTvs.Select(tv => tv.Title).ToList();
                    var totalPrice = selectedTvs.Sum(tv => tv.Price);
                    salesLogger.LogSale(tvTitleList, totalPrice);
                }
                else
                {
                    Console.WriteLine("Покупка отменена.");
                }
            }
            else
            {
                Console.WriteLine("Вы не выбрали ни одного телевизора. Покупка отменена.");
            }
        }

        private static bool IsPositiveResponse(string input)
        {
            return input.ToLower() == "д" || input.ToLower() == "y";
        }
    }
}