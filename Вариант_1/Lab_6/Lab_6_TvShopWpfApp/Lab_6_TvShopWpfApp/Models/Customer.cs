using System.Collections.Generic;
using System.Linq;
using System.Windows;
using Lab_5_TvShop;
using Lab_5_TvShop.Models;
using Lab_6_TvShopWpfApp.Logging;

namespace Lab_6_TvShopWpfApp.Models
{
    public static class Customer
    {
        public static void BuyTvs(SaleManager saleManager, TvShopRepository tvShopRepository,
            FileSalesLogger salesLogger)
        {
            var selectedTvs = new List<Tv>();

            while (true)
            {
                TvBox? selectedTvBox = null;

                while (selectedTvBox == null)
                {
                    var tvTitle = Microsoft.VisualBasic.Interaction.InputBox(
                        "Введите название телевизора, который вы хотите купить:",
                        "Выбор телевизора");

                    if (string.IsNullOrEmpty(tvTitle))
                    {
                        break;
                    }

                    selectedTvBox = tvShopRepository.GetTvBoxByTitle(tvTitle);

                    if (selectedTvBox == null)
                    {
                        MessageBox.Show("Некорректное название телевизора!", "Ошибка", MessageBoxButton.OK,
                            MessageBoxImage.Error);
                    }
                }

                if (selectedTvBox == null)
                {
                    break;
                }

                MessageBox.Show(
                    $"Название: {selectedTvBox.TvTitle}\nЦена: {selectedTvBox.Price}\nКоличество на складе: {selectedTvBox.Count}",
                    "Информация о телевизоре", MessageBoxButton.OK, MessageBoxImage.Information);

                int quantity;
                while (true)
                {
                    var input = Microsoft.VisualBasic.Interaction.InputBox(
                        $"Введите количество, которое вы хотите купить:\n" +
                        $"Доступно: {selectedTvBox.Count}", "Количество",
                        "");

                    if (!int.TryParse(input, out quantity) || quantity <= 0)
                    {
                        MessageBox.Show("Некорректное количество!", "Ошибка", MessageBoxButton.OK,
                            MessageBoxImage.Error);
                    }
                    else if (quantity > selectedTvBox.Count)
                    {
                        MessageBox.Show(
                            "Указанное количество превышает доступное количество на складе. Пожалуйста, выберите меньшее количество.",
                            "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
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
                MessageBox.Show($"Выбрано: {quantity}\nОбщая стоимость: {selectedTvBox.Price * quantity}",
                    "Телевизоры добавлены", MessageBoxButton.OK, MessageBoxImage.Information);

                var response = MessageBox.Show("Хотите продолжить выбор телевизоров?", "Продолжить выбор",
                    MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (response == MessageBoxResult.No)
                {
                    break;
                }
            }

            if (selectedTvs.Any())
            {
                string selectedTvInfo = "Вы выбрали следующие телевизоры:\n";
                foreach (var tv in selectedTvs)
                {
                    selectedTvInfo += $"Название: {tv.Title}, Цена: {tv.Price}\n";
                }

                selectedTvInfo += "Общая стоимость: " + selectedTvs.Sum(tv => tv.Price);

                var confirmResponse = MessageBox.Show(selectedTvInfo, "Подтвердить покупку", MessageBoxButton.YesNo,
                    MessageBoxImage.Question);
                if (confirmResponse == MessageBoxResult.Yes)
                {
                    saleManager.SellTvs(selectedTvs);
                    MessageBox.Show("Покупка успешно завершена!", "Покупка завершена", MessageBoxButton.OK,
                        MessageBoxImage.Information);

                    var tvTitleList = selectedTvs.Select(tv => tv.Title).ToList();
                    var totalPrice = selectedTvs.Sum(tv => tv.Price);
                    salesLogger.LogSale(tvTitleList, totalPrice);
                }
                else
                {
                    MessageBox.Show("Покупка отменена.", "Отмена покупки", MessageBoxButton.OK,
                        MessageBoxImage.Information);
                }
            }
            else
            {
                MessageBox.Show("Вы не выбрали ни одного телевизора. Покупка отменена.", "Отмена покупки",
                    MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
    }
}