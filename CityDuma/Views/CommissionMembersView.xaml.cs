using CityDuma.ViewModels;
using System.Diagnostics;
using System.Windows.Controls;

namespace CityDuma.Views
{
    /// <summary>
    /// Логика взаимодействия для CommissionMembersView.xaml
    /// </summary>
    public partial class CommissionMembersView : Page
    {
        public CommissionMembersView()
        {
            InitializeComponent();

            DataContext = new CommissionsMemberViewModel();
        }
    }
}
