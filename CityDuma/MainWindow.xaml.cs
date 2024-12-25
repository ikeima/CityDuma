using CityDuma.ViewModels;
using CityDuma.Views;
using System.Windows;
using System.Windows.Navigation;

namespace CityDuma
{
    public partial class MainWindows : Window
    {
        public readonly NavigationViewModel _navigationViewModel;

        public MainWindows()
        {
            InitializeComponent();

            _navigationViewModel = new NavigationViewModel(MainFrame);

            DataContext = _navigationViewModel;

            //var commissionViewModel = new CommissionsViewModel(_navigationViewModel);
            //var commissionsView = new CommissionsView();
            //var commissionMembersView = new CommissionMembersView();
            //commissionsView.DataContext = commissionViewModel;
            //commissionMembersView.DataContext = commissionViewModel;

            MainFrame.Navigate(new MainPage());
        }
    }
}