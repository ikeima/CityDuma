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

            DataContext = new MembersDumaViewModel();
        }
    }
}
