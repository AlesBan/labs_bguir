using System.Windows;
using System.Windows.Controls;
using Lab_5_TvShop.Models;
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

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var button = (Button)sender;

            switch (button.Name)
            {
                case "ShowAllButton":
                    OutputTextBox.Text = "";
                    var availableTvs = _tvShopManager.GetAllTvs();
                    OutputTextBox.Text = "Available TVs:\n";
                    foreach (var tvBox in availableTvs)
                    {
                        OutputTextBox.Text += tvBox.Info() + "\n";
                    }
                    break;

                case "BuyButton":
                    OutputTextBox.Text = "";
                    Customer.BuyTvs(_tvShopManager, _tvShopRepository, _salesLogger);
                    break;

                case "AddButton":
                    OutputTextBox.Text = "";

                    var newTvTitle = Microsoft.VisualBasic.Interaction.InputBox("Enter TV Title:", "Add TV");
                    var newTvPriceStr = Microsoft.VisualBasic.Interaction.InputBox("Enter TV Price:", "Add TV");
                    var newTvQuantityStr = Microsoft.VisualBasic.Interaction.InputBox("Enter TV Quantity:", "Add TV");

                    if (string.IsNullOrEmpty(newTvTitle) || string.IsNullOrEmpty(newTvPriceStr) || string.IsNullOrEmpty(newTvQuantityStr))
                    {
                        MessageBox.Show("Please enter all the required information.", "Invalid data", MessageBoxButton.OK, MessageBoxImage.Warning);
                        break;
                    }

                    if (int.TryParse(newTvPriceStr, out int newTvPrice) && int.TryParse(newTvQuantityStr, out int newTvQuantity))
                    {
                        var newTvBox = new TvBox
                        {
                            TvTitle = newTvTitle,
                            Price = newTvPrice,
                            Count = newTvQuantity
                        };

                        _tvShopManager.AddTvBox(newTvBox);
                        OutputTextBox.Text = "New TV added to inventory!";
                    }
                    else
                    {
                        OutputTextBox.Text = "Invalid data!";
                    }
                    break;
                
                case "ExitButton":
                    OutputTextBox.Text = "";
                    OutputTextBox.Text = "Thank you for visiting our TV shop. Goodbye!";
                    Close();
                    break;
            }
        }
    }
}