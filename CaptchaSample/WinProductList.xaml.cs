using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace CaptchaSample
{
    /// <summary>
    /// Логика взаимодействия для WinProductList.xaml
    /// </summary>
    public partial class WinProductList : Window
    {
        public string FIO { get; }
        private readonly User user;

        public List<Product> Products { get; set; }
        public Product SelectedProduct { get; set; }

        public WinProductList(User user)
        {
            InitializeComponent();
            this.user = user;
            FIO = $"{user.UserSurname} {user.UserName} {user.UserPatronymic}";

            FillProducts();
            DataContext = this;
        }

        private void FillProducts()
        {
            Products = DB.Instance.Products.
                Include(s => s.ProductProvider).
                Include(s => s.ProductManufacturer).
                Include(s => s.ProductCategory).ToList();
        }

        private void buttonExitToLogin(object sender, RoutedEventArgs e)
        {
            new MainWindow().Show();
            Close();
        }
    }
}
