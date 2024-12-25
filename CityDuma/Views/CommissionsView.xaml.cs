using CityDuma.ViewModels;
using System.Windows;
using System.Windows.Controls;

namespace CityDuma.Views
{
    /// <summary>
    /// Логика взаимодействия для CommissionsView.xaml
    /// </summary>
    public partial class CommissionsView : Page
    {
        public CommissionsView()
        {
            InitializeComponent();

            DataContext = new CommissionsViewModel();

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //NavigationService.Navigate(new CommissionMembersView());
        }
    }
}
