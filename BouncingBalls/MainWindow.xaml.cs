using BouncingBalls.Pages;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BouncingBalls
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            frmMain.NavigationService.Navigate(new AgentsListPage());
            frmMain.Navigated += frmMain_Navigated;
            DataContext = this;
        }
        private void frmMain_Navigated(object sender, NavigationEventArgs e)
        {
            tbTitle.Text = (frmMain.Content as Page).Title;
        }
    }
}
