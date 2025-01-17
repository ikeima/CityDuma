using System.Windows;

namespace CityDuma.Services
{
    public class ErrorDialogService : IErrorDialogService
    {
        public void ShowErrorDialog(string message)
        {
            MessageBox.Show(message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}
