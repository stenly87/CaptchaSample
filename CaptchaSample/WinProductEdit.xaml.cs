using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Windows;
using System.Windows.Media.Imaging;

namespace CaptchaSample
{
    /// <summary>
    /// Логика взаимодействия для WinProductEdit.xaml
    /// </summary>
    public partial class WinProductEdit : Window, INotifyPropertyChanged
    {
        public bool Editable { get; set; }
        public List<Category> Categories { get; set; }
        public List<Manufacturer> Manufacturers { get; set; }
        public List<Provider> Providers { get; set; }

        public WinProductEdit(Product selectedProduct)
        {
            InitializeComponent();
            Categories = DB.Instance.Categories.ToList();
            Manufacturers = DB.Instance.Manufacturers.ToList();
            Providers = DB.Instance.Providers.ToList();
            if (string.IsNullOrEmpty(selectedProduct.ProductArticleNumber))
            {
                SelectedProduct = selectedProduct;
                Editable = true;
            }
            else
                SelectedProduct = selectedProduct.Clone();

            DataContext = this;
        }

        public Product SelectedProduct { get; set; }

        public event PropertyChangedEventHandler? PropertyChanged;
        void Signal(string prop) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));

        private void SaveClose(object sender, RoutedEventArgs e)
        {
            { 
                // проверки
            }
            if (Editable)
                DB.Instance.Products.Add(SelectedProduct);
            else
            {
                var original = DB.Instance.Products.
                    Find(SelectedProduct.ProductArticleNumber);
                DB.Instance.Products.Entry(original)
                    .CurrentValues.SetValues(SelectedProduct);
            }
            DB.Instance.SaveChanges();
            Close();
        }

        private void SelectPhoto(object sender, RoutedEventArgs e)
        {
            string dir = Environment.CurrentDirectory + @"\Images\";
            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "Images|*.png;";
            if (dlg.ShowDialog() == true)
            {
                var test = new BitmapImage(new Uri(dlg.FileName));
                if (test.PixelWidth > 400 || test.PixelHeight > 300)
                {
                    MessageBox.Show("Картинка слишком большая");
                    return;
                }
                string newFile = dir + new FileInfo(dlg.FileName).Name;
                File.Copy(dlg.FileName, newFile, true);
                SelectedProduct.ProductPhoto = File.ReadAllBytes(newFile);
                Signal("SelectedProduct");
            }
        }
    }

    public static class ProductExtension
    {
        public static Product Clone(this Product product)
        {
            var values = DB.Instance.Products.Entry(product).CurrentValues.Clone();
            var clone = (Product)values.ToObject();
            clone.ProductCategory = product.ProductCategory;
            clone.ProductManufacturer = product.ProductManufacturer;
            clone.ProductProvider = product.ProductProvider;
            return clone;
        }
    }
}
