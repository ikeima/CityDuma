using CityDuma.Commands;
using CityDuma.Services;
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

            var errorDialogService = new ErrorDialogService();

            DataContext = new CommissionsViewModel(errorDialogService);

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //NavigationService.Navigate(new CommissionMembersView());
        }
    }
}
