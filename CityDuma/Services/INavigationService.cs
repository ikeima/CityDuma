namespace CityDuma.Services
{
    public interface INavigationService
    {
        void NavigateTo(string pageName);
        void NavigateTo(string pageName, object parameter);
        void GoBack();
        bool CanGoBack();
    }
}
