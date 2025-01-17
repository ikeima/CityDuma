using CityDuma.Commands;
using CityDuma.Domain.Dto;
using CityDuma.Entities;
using CityDuma.Services;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;

namespace CityDuma.ViewModels
{
    public class CommissionsViewModel : INotifyPropertyChanged
    {
        private AppDbContext _dbContext;
        private readonly INavigationService _navigationService;
        private readonly ShowErrorCommand _showErrorCommand;

        public event NotifyCollectionChangedEventHandler CollectionChanged;
        public ObservableCollection<CommissionsDto> Commissions { get; set; }
        public ObservableCollection<CommissionMembersDto> CommissionMembers { get; set; }
        public ObservableCollection<ChairmanDto> Chairmans { get; set; }
        private CommissionsDto _selectedCommission;
        public CommissionsDto SelectedCommission
        {
            get => _selectedCommission;
            set 
            { 
                _selectedCommission = value;
                OnPropertyChanged(nameof(SelectedCommission));
            } 

        }

        public ICommand AddCoommissionCommand { get; set; }
        public ICommand SaveCommissionsCommand { get; set; }
        public ICommand DeleteCommissionCommand { get; set; }
        public ICommand ShowErrorCommand => _showErrorCommand;

        public CommissionsViewModel(IErrorDialogService errorDialogService)
        {
            _dbContext = new AppDbContext();

            Chairmans = new ObservableCollection<ChairmanDto>
            (
               _dbContext.MembersDuma.Select(m => new ChairmanDto
               {
                   Code = m.CodeMembersDuma,
                   FullName = m.Surname + " " + m.Name
               }).ToList()
            );

            var commissions = _dbContext.Commissions
                .Include(c => c.MembersCommission.Select(mc => mc.MembersDuma))
                .Include(c => c.MembersDuma)
                .Select(c => new CommissionsDto
                {
                    CodeMembersCommission = c.CodeMembersCommission,
                    Direction = c.Direction,
                    ChairmanCode = c.ChairmanCode,
                })
                .AsEnumerable()
                .Select(c => new CommissionsDto
                {
                    CodeMembersCommission = c.CodeMembersCommission,
                    Direction = c.Direction,
                    ChairmanCode = c.ChairmanCode,
                    Chairman = Chairmans.FirstOrDefault(ch => ch.Code == c.ChairmanCode)
                })
                .ToList();

            Commissions = new ObservableCollection<CommissionsDto>(commissions);

            AddCoommissionCommand = new RelayCommand(AddCommission);
            SaveCommissionsCommand = new RelayCommand(SaveCommissionsChanges);
            DeleteCommissionCommand = new RelayCommand(DeleteCommission, CanDeleteCommission);
            _showErrorCommand = new ShowErrorCommand(errorDialogService);
        }

        #region Commission commands
        private void AddCommission()
        {
            var newCommission = new CommissionsDto
            {
                Direction = "Новое направление", 
                ChairmanCode = Chairmans.FirstOrDefault()?.Code ?? 0,
                Chairman = Chairmans.FirstOrDefault() 
            };

            Commissions.Add(newCommission);

            var commissionEntity = new Commissions
            {
                Direction = newCommission.Direction,
                ChairmanCode = newCommission.ChairmanCode
            };

            _dbContext.Commissions.Add(commissionEntity);
            _dbContext.SaveChanges();

            newCommission.CodeMembersCommission = commissionEntity.CodeMembersCommission;
        }

        private void SaveCommissionsChanges()
        {
            foreach (var item in Commissions)
            {
                var commission = _dbContext.Commissions.FirstOrDefault(c => c.CodeMembersCommission == item.CodeMembersCommission);

                if (commission != null)
                {
                    commission.Direction = item.Direction;
                    commission.ChairmanCode = item.Chairman.Code;
                }
            }

            _dbContext.SaveChanges();
        }

        private bool CanDeleteCommission()
        {
            return SelectedCommission != null;
        }
        private void DeleteCommission()
        {
            if (SelectedCommission != null)
            {
                try
                {
                    var removedCommision = _dbContext.Commissions.FirstOrDefault(c => c.CodeMembersCommission == SelectedCommission.CodeMembersCommission);
                    _dbContext.Commissions.Remove(removedCommision);
                    _dbContext.SaveChanges();
                    Commissions.Remove(SelectedCommission);
                }
                catch (Exception ex)
                {
                    _showErrorCommand.Execute("Произошла ошибка при удалении комиссии!\nПроверьте, нет ли в удаляемой комиссии зарегистрированных членов думы.");
                }
            }
        }
        #endregion

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
