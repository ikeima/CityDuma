using System.Windows.Controls;
using System.Windows.Input;
using System.ComponentModel;
using CityDuma.Services;
using System.Windows.Navigation;
using System;

namespace CityDuma.ViewModels
{
    public class NavigationViewModel : INotifyPropertyChanged, INavigationService
    {
        private readonly NavigationService _navigationService;

        public ICommand NavigateBackCommand { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        public NavigationViewModel(Frame navigationFrame)
        {
            _navigationService = navigationFrame.NavigationService;
            NavigateBackCommand = new RelayCommand(GoBack, CanGoBack);

            _navigationService.Navigated += (sender, args) => OnPropertyChanged(nameof(CanGoBack));
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void NavigateTo(string pageName)
        {
            _navigationService.Navigate(new Uri(pageName, UriKind.Relative));
        }

        public void NavigateTo(string pageName, object parameter)
        {
            _navigationService.Navigate(new Uri(pageName, UriKind.Relative), parameter);
        }

        public void GoBack()
        {
            if (_navigationService.CanGoBack)
                _navigationService.GoBack();
        }

        public bool CanGoBack()
        {
            return _navigationService.CanGoBack;
        }
    }
}
