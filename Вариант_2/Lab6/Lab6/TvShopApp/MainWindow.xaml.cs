using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using TvShopApp.Models;
using TvShopApp.Models.Store;

namespace TvShopApp
{
    public partial class MainWindow : Window
    {
        private readonly TvStoreManager _tvStoreManager;
        private readonly TvStoreLogger _salesLogger;
        private readonly TvStore _tvStore;

        public MainWindow()
        {
            InitializeComponent();
            var directory = Directory.GetCurrentDirectory();
            var tvStorageFilePath = Path.Combine(directory, "tvStorage.txt");
            var customTvTitlesFilePath = Path.Combine(directory, "tvTitles.txt");

            _tvStore = new TvStore(tvStorageFilePath, customTvTitlesFilePath);
            _tvStoreManager = new TvStoreManager(_tvStore);
            _salesLogger = new TvStoreLogger();

            _tvStore.InitializeTvCategories();
            
            DisplayAllTvs();
        }

        private void DisplayAllTvs()
        {
            OutputTextBox.Text = "Fetching the list of available TVs...\n";
            var availableTvCategories = _tvStoreManager.GetAllTvCategories();
            var tvCategories = availableTvCategories.ToList();
            if (!tvCategories.Any())
            {
                OutputTextBox.Text += "Oops! There are no TVs available at the moment.";
            }
            else
            {
                OutputTextBox.Text += "Available TVs:\n";
                foreach (var tvCategory in tvCategories)
                {
                    OutputTextBox.Text += tvCategory.GetFullInfo() + "\n";
                }
            }
        }

        private void PurchaseButton_Click(object sender, RoutedEventArgs e)
        {
            DisplayAllTvs();
            OutputTextBox.Text += "Great! Let's proceed with your TV purchase.\n";
            Customer.BuyTvs(_tvStoreManager, _tvStore, _salesLogger);
            Task.Delay(1000).Wait();
            DisplayAllTvs();
        }

        private void AddTvButton_Click(object sender, RoutedEventArgs e)
        {
            DisplayAllTvs();
            OutputTextBox.Text += "Adding new TVs to the inventory...\n";
            var newTvTitle =
                Microsoft.VisualBasic.Interaction.InputBox("Enter the title of the new TV:", "New TV Title");

            var priceStr = Microsoft.VisualBasic.Interaction.InputBox("Enter the price of the new TV:", "New TV Price");
            int.TryParse(priceStr, out var newTvPrice);

            var countStr =
                Microsoft.VisualBasic.Interaction.InputBox("Enter the quantity of the new TVs:", "New TV Quantity");
            int.TryParse(countStr, out var newTvCount);

            var newTvCategory = new TvCategory()
            {
                Title = newTvTitle,
                Price = newTvPrice,
                Count = newTvCount
            };

            _tvStoreManager.AddTvCategory(newTvCategory);
            OutputTextBox.Text += "\nNew TV added to the inventory successfully!";
            Task.Delay(1000).Wait();
            DisplayAllTvs();
        }

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            DisplayAllTvs();
            OutputTextBox.Text += "Thank you for visiting our TV store. Have a great day!";
            Close();
        }
    }
}