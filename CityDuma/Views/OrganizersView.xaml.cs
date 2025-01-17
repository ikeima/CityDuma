using CityDuma.ViewModels;
using System.Linq;
using System.Windows.Controls;

namespace CityDuma.Views
{
    /// <summary>
    /// Логика взаимодействия для OrganizersView.xaml
    /// </summary>
    public partial class OrganizersView : Page
    {
        private bool isUpdating = false;
        public OrganizersView()
        {
            InitializeComponent();

            DataContext = new OrganizersViewModel();
        }

        private void TextBox_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            // Разрешаем ввод только цифр
            if (!char.IsDigit(e.Text, 0))
            {
                e.Handled = true;
            }
        }

    }
}
