using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;

namespace CityDuma.Views
{
    /// <summary>
    /// Логика взаимодействия для MainPage.xaml
    /// </summary>
    public partial class MainPage : Page
    {
        public MainPage()
        {
            InitializeComponent();
        }
        private void ToggleMenuButton_Click(object sender, RoutedEventArgs e)
        {
            if (MenuBorder.Width == 200)
            {
                HideMenu();
            }
            else
            {
                ShowMenu();
            }
        }

        private void ShowMenu()
        {
            var animation = new DoubleAnimation(200, new Duration(TimeSpan.FromSeconds(0.3)));
            MenuBorder.BeginAnimation(Border.WidthProperty, animation);
        }

        private void HideMenu()
        {
            var animation = new DoubleAnimation(0, new Duration(TimeSpan.FromSeconds(0.3)));
            MenuBorder.BeginAnimation(Border.WidthProperty, animation);
        }

        private void CompanyButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new CompanyView());
        }

        private void EmployeesButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new MenuView());
        }
    }
}
