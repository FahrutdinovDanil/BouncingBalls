using BouncingBalls.DataBase;
using BouncingBalls.Windows;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BouncingBalls.Pages
{
    /// <summary>
    /// Логика взаимодействия для AgentPage.xaml
    /// </summary>
    public partial class AgentPage : Page
    {
        public Agent Agent { get; set; }
        public List<AgentType> AgentTypes { get; set; }
        public List<Product> Products { get; set; }

        public AgentPage(Agent agent)
        {
            InitializeComponent();
            Agent = agent;

            AgentTypes = DataAccess.GetAgentTypes();
            Products = DataAccess.GetProducts();

            DataContext = this;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                DataAccess.SaveAgent(Agent);
                NavigationService.GoBack();
            }
            catch
            {
                MessageBox.Show("Невозможно сохранить изменения", "Данные заолнены некорректно");
            }
        }

        private void SelectImageButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog
            {
                Filter = "*.png|*.png|*.jpeg|*.jpeg|*.jpg|*.jpg"
            };

            if (fileDialog.ShowDialog().Value)
            {
                var photo = File.ReadAllBytes(fileDialog.FileName);
                if (photo.Length > 1024 * 150)  //Размер фотографии не должен превышать 150 Кбайт
                {
                    MessageBox.Show("Размер фотографии не должен превышать 150 КБ", "Ошибка");
                    return;
                }
                Agent.Logo = photo;
                AgentLogo.Source = new BitmapImage(new Uri(fileDialog.FileName));
            }
        }

        private void ManForProductionTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Удалить данного агента?", "Предупреждение", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {
                Agent.IsDeleted = true;
                DataAccess.DeleteAgent(Agent);
                NavigationService.GoBack();
            }
        }

        private void cbProducts_TextChanged(object sender, TextChangedEventArgs e)
        {
            cbProducts.ItemsSource = Products.Where(p => p.Title.ToLower().Contains(cbProducts.Text.ToLower())).ToList();
            cbProducts.IsDropDownOpen = true;
        }

        private void cbProducts_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var product = cbProducts.SelectedItem as Product;

            if (product == null || Agent.ProductSales.Where(x => x.Product.Title == product.Title).Count() != 0)
                return;

            var productSale = new ProductSale { Agent = Agent, Product = product, SaleDate = DateTime.Now };

            if ((bool)new ProductCountWindow(productSale).ShowDialog())
            {
                Agent.ProductSales.Add(productSale);
                lvProductSales.ItemsSource = Agent.ProductSales;
                lvProductSales.Items.Refresh();
            }
        }

        private void lvProductSales_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var product = lvProductSales.SelectedItem as ProductSale;
            if (product != null)
            {
                Agent.ProductSales.Remove(product);
                DataAccess.DeleteProductSale(product);

                lvProductSales.ItemsSource = Agent.ProductSales;
                lvProductSales.Items.Refresh();
            }
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }
    }
}
