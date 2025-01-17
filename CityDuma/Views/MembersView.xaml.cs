using CityDuma.Services;
using CityDuma.ViewModels;
using System.Windows;
using System.Windows.Controls;

namespace CityDuma.Views
{
    /// <summary>
    /// Логика взаимодействия для MembersView.xaml
    /// </summary>
    public partial class MembersView : Page
    {
        public MembersView()
        {
            InitializeComponent();

            var errorDialogService = new ErrorDialogService();

            DataContext = new MembersDumaViewModel(errorDialogService);
        }
    }
}
