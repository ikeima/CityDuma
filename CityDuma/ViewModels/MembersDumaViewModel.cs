using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;
using CityDuma.Commands;
using CityDuma.Entities;
using CityDuma.Services;

namespace CityDuma.ViewModels
{
    public class MembersDumaViewModel : INotifyPropertyChanged
    {
        private AppDbContext _dbContext;
        private MembersDuma _selectedMember;
        private readonly ShowErrorCommand _showErrorCommand;
        public ObservableCollection<MembersDuma> Members { get; set; }

        public MembersDuma SelectedMember
        {
            get => _selectedMember;
            set
            {
                _selectedMember = value;
                OnPropertyChanged(nameof(SelectedMember));
            }
        }

        public ICommand AddCommand { get; set; }
        public ICommand SaveChangesCommand { get; set; }
        public ICommand DeleteCommand { get; set; }
        public ICommand ShowErrorCommand => _showErrorCommand;

        public MembersDumaViewModel(IErrorDialogService errorDialogService)
        {
            _dbContext = new AppDbContext();
            Members = new ObservableCollection<MembersDuma>(_dbContext.MembersDuma.ToList());

            AddCommand = new RelayCommand(AddMember);
            SaveChangesCommand = new RelayCommand(SaveChanges, CanEditOrDelete);
            DeleteCommand = new RelayCommand(DeleteMember, CanEditOrDelete);

            _showErrorCommand = new Commands.ShowErrorCommand(errorDialogService);
        }

        private void AddMember()
        {
            var newMember = new MembersDuma
            {
                Surname = "New",
                Name = "Member",
                Patronymic = "",
                BirthDate = DateTime.Now,
                PhoneNumber = "",
                Experience = 0,
                DateEntryOfDuma = DateTime.Now
            };

            _dbContext.MembersDuma.Add(newMember);
            _dbContext.SaveChanges();

            Members.Add(newMember);
        }

        private void SaveChanges()
        {
            foreach (var member in Members)
            {
                var exitstingMember = _dbContext.MembersDuma.FirstOrDefault(p => p.CodeMembersDuma == member.CodeMembersDuma);
                if (exitstingMember != null)
                {
                    exitstingMember.Surname = member.Surname;
                    exitstingMember.Name = member.Name;
                    exitstingMember.Patronymic= member.Patronymic;
                    exitstingMember.BirthDate = member.BirthDate;
                    exitstingMember.PhoneNumber = member.PhoneNumber;
                    exitstingMember.Experience = member.Experience;
                    exitstingMember.DateEntryOfDuma = member.DateEntryOfDuma;
                }
                else
                {
                    _dbContext.MembersDuma.Add(member); // Добавить новый элемент, если его нет в БД
                }
            }

            _dbContext.SaveChanges();
        }

        private void DeleteMember()
        {
            try
            {
                if (SelectedMember != null && SelectedMember.CodeMembersDuma != 0)
                {
                    _dbContext.MembersDuma.Remove(SelectedMember);
                    _dbContext.SaveChanges();
                    Members.Remove(SelectedMember);
                }
            }
            catch (Exception ex)
            {
                _showErrorCommand.Execute("Произошла ошибка при удалении!\nПроверьте, не находится ли удаляемой член думы председателем комиссии или её членом.\nДля корректного удаления воспользуйтесь соответсвующими разделами приложения.");
            }
        }

        private bool CanEditOrDelete()
        {
            return SelectedMember != null;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
