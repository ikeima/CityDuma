using CityDuma.Services;
using System;
using System.Windows.Input;

namespace CityDuma.Commands
{
    public class ShowErrorCommand : ICommand
    {
        private readonly IErrorDialogService _errorDialogService;

        public ShowErrorCommand(IErrorDialogService errorDialogService)
        {
            _errorDialogService = errorDialogService;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            string errorMessage = parameter.ToString(); 
            _errorDialogService.ShowErrorDialog(errorMessage);
        }

        public event EventHandler CanExecuteChanged;
    }
}
