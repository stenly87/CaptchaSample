﻿using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    public partial class WinProductList : Window, INotifyPropertyChanged
    {
        public string CountView { get; set; }

        public string FIO { get; }
        private readonly User user;
        private string searchText = "";
        private int selectedSorting;
        private Manufacturer selectedManufactorer;

        public event PropertyChangedEventHandler? PropertyChanged;
        void Signal(string prop) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));

        public Product SelectedProduct { get; set; }

        public List<Manufacturer> Manufactorers { get; set; }
        public List<Product> Products { get; set; }
        public List<string> Sorting { get; set; } = new List<string>() { "Без сортировки", "Стоимость по убыванию", "Стоимость по возрастанию" };
        public Visibility IsAdminVisibility { get; set; }

        public int SelectedSorting
        {
            get => selectedSorting;
            set
            {
                selectedSorting = value;
                Search();
            }
        }
        public string SearchText
        {
            get => searchText;
            set
            {
                searchText = value;
                Search();
            }
        }
        public Manufacturer SelectedManufactorer
        {
            get => selectedManufactorer;
            set
            {
                selectedManufactorer = value;
                Search();
            }
        }

        private void Search()
        {
            var result = DB.Instance.Products.
                Include(s => s.ProductProvider).
                Include(s => s.OrderProducts).
                Include(s => s.ProductManufacturer).
                Include(s => s.ProductCategory).Where(s =>
                    s.ProductProvider.ProductProvider.Contains(searchText) ||
                    s.ProductManufacturer.ProductManufacturer.Contains(searchText) ||
                    s.ProductArticleNumber.Contains(searchText) ||
                    s.ProductCategory.ProductCategory.Contains(searchText) ||
                    s.ProductDescription.Contains(searchText) ||
                    s.ProductName.Contains(searchText)
                );
            if (SelectedManufactorer.ManufacturerId != 0)
                result = result.Where(s => s.ProductManufacturerId == SelectedManufactorer.ManufacturerId);
            if (selectedSorting == 1)
                result = result.OrderByDescending(s => s.ProductCost);
            else if (selectedSorting == 2)
                result = result.OrderBy(s => s.ProductCost);
            Products = result.ToList();
            Signal(nameof(Products));

            CountView = $"Записей: {Products.Count} из {DB.Instance.Products.Count()}";
            Signal(nameof(CountView));
        }

        public WinProductList(User user)
        {
            InitializeComponent();

            this.user = user;
            FIO = $"{user.UserSurname} {user.UserName} {user.UserPatronymic}";
            IsAdminVisibility = 
                user.UserRole == 1 ? // Администратор
                Visibility.Visible :
                Visibility.Collapsed;
            FillManufactorers();
            Search();
            DataContext = this;
        }

        private void FillManufactorers()
        {
            Manufactorers = new List<Manufacturer>();
            Manufactorers.Add(new Manufacturer { ProductManufacturer = "Все производители" });
            Manufactorers.AddRange(DB.Instance.Manufacturers.ToList());

            selectedManufactorer = Manufactorers.FirstOrDefault();
        }


        private void buttonExitToLogin(object sender, RoutedEventArgs e)
        {
            new MainWindow().Show();
            Close();
        }

        private void AddProduct(object sender, RoutedEventArgs e)
        {
            new WinProductEdit(new Product()).ShowDialog();
            Search();
        }

        private void EditProduct(object sender, RoutedEventArgs e)
        {
            if (SelectedProduct != null)
            {
                new WinProductEdit(SelectedProduct).ShowDialog();
                Search();
            }
        }

        private void RemoveProduct(object sender, RoutedEventArgs e)
        {
            if (SelectedProduct != null)
            {
                if (SelectedProduct.OrderProducts.Count == 0)
                {
                    if (MessageBox.Show("Удалить выбранный товар?", "Внимание!", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                    {
                        DB.Instance.Products.Remove(SelectedProduct);
                        DB.Instance.SaveChanges();
                        Search();
                    }
                }
                else
                    MessageBox.Show("Выбранный товар нельзя удалить, поскольку он фигурирует в заказах");
            }
        }

        private void EditProduct(object sender, MouseButtonEventArgs e)
        {
            if (user.UserRole != 1)
                return;
            if (SelectedProduct != null)
            {
                new WinProductEdit(SelectedProduct).ShowDialog();
                Search();
            }
        }
    }
}
