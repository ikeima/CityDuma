using System.Windows;
using System.Windows.Controls;

namespace CityDuma.Views
{
    /// <summary>
    /// Логика взаимодействия для CompanyView.xaml
    /// </summary>
    public partial class CompanyView : Page
    {
        public CompanyView()
        {
            InitializeComponent();
        }

        private void NavigateToMeetings(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new MeetingsView());
        }

        private void NavigateToOrganizers(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new OrganizersView());
        }
    }
}
