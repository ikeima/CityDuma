using System.Windows;
using System.Windows.Controls;

namespace CityDuma.Views
{
    /// <summary>
    /// Логика взаимодействия для MenuView.xaml
    /// </summary>
    public partial class MenuView : Page
    {
        public MenuView()
        {
            InitializeComponent();
        }

        private void NavigateToCommissions(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new CommissionsView());
        }

        private void NavigateToCommissionMembers(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new CommissionMembersView());
        }

        private void NavigateToMembersView(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new MembersView());
        }
    }
}
