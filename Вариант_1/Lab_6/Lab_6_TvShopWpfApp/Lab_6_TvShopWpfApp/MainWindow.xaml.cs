using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Lab_6_TvShopWpfApp.Logging;
using Lab_6_TvShopWpfApp.Models;

namespace Lab_6_TvShopWpfApp
{
    public partial class MainWindow
    {
        private readonly TvShopRepository _tvShopRepository;
        private readonly SaleManager _tvShopManager;
        private readonly FileSalesLogger _salesLogger;

        public MainWindow()
        {
            InitializeComponent();

            _tvShopRepository = new TvShopRepository(OutputTextBox.AppendText);
            _tvShopManager = new SaleManager(_tvShopRepository);
            _salesLogger = new FileSalesLogger();

            _tvShopRepository.ParseTvTitlesAndAddTvBoxes();
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            var button = (Button)sender;

            switch (button.Name)
            {
                case "ShowAllButton":
                    await ShowAllTvsAsync();
                    break;

                case "BuyButton":
                    Customer.BuyTvs(_tvShopManager, _tvShopRepository, _salesLogger);
                    await Task.Delay(1000);
                    await ShowAllTvsAsync();
                    break;

                case "AddButton":
                    await AddNewTvAsync();
                    break;

                case "ExitButton":
                    await ExitApplicationAsync();
                    break;
            }
        }

        private Task ShowAllTvsAsync()
        {
            OutputTextBox.Text = "";
            var availableTvs = _tvShopManager.GetAllTvs();
            OutputTextBox.Text = "Доступные телевизоры:\n";
            foreach (var tvBox in availableTvs)
            {
                OutputTextBox.Text += tvBox.Info() + "\n";
            }

            return Task.CompletedTask;
        }

        private async Task AddNewTvAsync()
        {
            var newTvTitle =
                Microsoft.VisualBasic.Interaction.InputBox("Введите название телевизора:", "Добавление телевизора");
            var newTvPriceStr =
                Microsoft.VisualBasic.Interaction.InputBox("Введите цену телевизора:", "Добавление телевизора");
            var newTvQuantityStr =
                Microsoft.VisualBasic.Interaction.InputBox("Введите количество телевизоров:", "Добавление телевизора");

            if (string.IsNullOrEmpty(newTvTitle) || string.IsNullOrEmpty(newTvPriceStr) ||
                string.IsNullOrEmpty(newTvQuantityStr))
            {
                MessageBox.Show("Пожалуйста, введите всю необходимую информацию.", "Неверные данные",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (int.TryParse(newTvPriceStr, out var newTvPrice) &&
                int.TryParse(newTvQuantityStr, out int newTvQuantity))
            {
                var newTvBox = new TvBox
                {
                    TvTitle = newTvTitle,
                    Price = newTvPrice,
                    Count = newTvQuantity
                };

                _tvShopManager.AddTvBox(newTvBox);
                OutputTextBox.Text = "Новый телевизор добавлен в инвентарь!";
            }
            else
            {
                OutputTextBox.Text = "Неверные данные!";
            }

            await Task.Delay(1000);
            await ShowAllTvsAsync();
        }

        private Task ExitApplicationAsync()
        {
            OutputTextBox.Text = "";
            OutputTextBox.Text = "Спасибо за посещение нашего магазина телевизоров. До свидания!";
            Close();
            return Task.CompletedTask;
        }
    }
}