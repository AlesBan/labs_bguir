using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using TvShopApp.Models.Store;

namespace TvShopApp.Models
{
    public static class Customer
    {
        public static void BuyTvs(TvStoreManager saleManager, TvStore tvShopRepository, TvStoreLogger salesLogger)
        {
            var selectedTvs = new List<Tv>();

            while (true)
            {
                var selectedTvCategory = GetSelectedTvCategory(tvShopRepository);

                if (selectedTvCategory == null)
                {
                    break;
                }

                var quantity = GetValidQuantity(selectedTvCategory);

                var tvsToBuy = CreateTvsToBuy(selectedTvCategory, quantity);
                selectedTvs.AddRange(tvsToBuy);

                DisplaySelectedTv(selectedTvCategory, quantity);

                if (!ContinueSelectingTvs())
                {
                    break;
                }
            }

            if (selectedTvs.Count > 0)
            {
                DisplaySelectedTvs(selectedTvs);

                if (ConfirmPurchase())
                {
                    CompletePurchase(saleManager, selectedTvs, salesLogger);
                }
                else
                {
                    MessageBox.Show("Purchase canceled.");
                }
            }
            else
            {
                MessageBox.Show("You haven't selected any TVs. Purchase canceled.");
            }
        }

        private static TvCategory GetSelectedTvCategory(TvStore tvStore)
        {
            var tvTitle = Microsoft.VisualBasic.Interaction.InputBox("Enter the title of the TV you want to buy:");

            if (string.IsNullOrEmpty(tvTitle))
            {
                return null;
            }

            var selectedTvCategory = tvStore.GetTvCategory(tvTitle);

            if (selectedTvCategory == null)
            {
                MessageBox.Show("Invalid TV title!");
                return null;
            }

            return selectedTvCategory;
        }

        private static int GetValidQuantity(TvCategory selectedTvCategory)
        {
            var quantity = 0;

            while (true)
            {
                var input = Microsoft.VisualBasic.Interaction.InputBox(
                    $"Enter the quantity you want to buy (available: {selectedTvCategory.Count}):");

                if (string.IsNullOrEmpty(input))
                {
                    break;
                }

                if (!int.TryParse(input, out quantity) || quantity <= 0)
                {
                    MessageBox.Show("Invalid quantity!");
                }
                else if (quantity > selectedTvCategory.Count)
                {
                    MessageBox.Show(
                        "The specified quantity exceeds the available quantity in stock. Please choose a smaller quantity.");
                }
                else
                {
                    break;
                }
            }

            return quantity;
        }

        private static IEnumerable<Tv> CreateTvsToBuy(TvCategory selectedTvCategory, int quantity)
        {
            var tvsToBuy = new List<Tv>();

            if (selectedTvCategory == null)
            {
                return tvsToBuy;
            }

            for (var i = 0; i < quantity; i++)
            {
                var tv = new Tv { Title = selectedTvCategory.Title, Price = selectedTvCategory.Price };
                tvsToBuy.Add(tv);
            }

            return tvsToBuy;
        }

        private static void DisplaySelectedTv(TvCategory selectedTvCategory, int quantity)
        {
            var message = new StringBuilder();
            message.AppendLine($"Title: {selectedTvCategory.Title}");
            message.AppendLine($"Price: {selectedTvCategory.Price}");
            message.AppendLine($"Quantity in stock: {selectedTvCategory.Count}");
            message.AppendLine($"Selected: {quantity}");
            message.AppendLine($"Total cost: {selectedTvCategory.Price * quantity}");

            MessageBox.Show(message.ToString(), "Selected TV", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private static bool ContinueSelectingTvs()
        {
            var result = MessageBox.Show("Do you want to continue selecting TVs?", "Confirmation",
                MessageBoxButton.YesNo, MessageBoxImage.Question);
            return result == MessageBoxResult.Yes;
        }

        private static void DisplaySelectedTvs(List<Tv> selectedTvs)
        {
            var message = new StringBuilder();
            message.AppendLine("You have selected the following TVs:");

            foreach (var tv in selectedTvs)
            {
                message.AppendLine($"Title: {tv.Title}, Price: {tv.Price}");
            }

            var totalCost = selectedTvs.Sum(tv => tv.Price);
            message.AppendLine($"Total cost: {totalCost}");

            MessageBox.Show(message.ToString(), "Selected TVs", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private static bool ConfirmPurchase()
        {
            var result = MessageBox.Show("Confirm the purchase?", "Confirmation", MessageBoxButton.YesNo,
                MessageBoxImage.Question);
            return result == MessageBoxResult.Yes;
        }

        private static void CompletePurchase(TvStoreManager saleManager, List<Tv> selectedTvs,
            TvStoreLogger salesLogger)
        {
            saleManager.SellTvs(selectedTvs);
            var tvTitleList = selectedTvs.ConvertAll(tv => tv.Title);
            var tvPrice = selectedTvs.Sum(tv => tv.Price);
            salesLogger.LogSale(tvTitleList, tvPrice);
            MessageBox.Show("Purchase completed successfully!", "Success", MessageBoxButton.OK,
                MessageBoxImage.Information);
        }
    }
}