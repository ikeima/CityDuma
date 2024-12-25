using CityDuma.Domain.Dto;
using CityDuma.Entities;
using CityDuma.Services;

//using System.Data.Entity;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;

namespace CityDuma.ViewModels
{
    public class CommissionsViewModel : INotifyCollectionChanged
    {
        private AppDbContext _dbContext;
        private readonly INavigationService _navigationService;
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

        //public ICommand AddCommissionMemberCommand { get; set; }
        //public ICommand SaveCommissionMembersCommand { get; set; }
        //public ICommand DeleteCommissionMemberCommand { get; set; }

        public CommissionsViewModel(/*INavigationService navigationService*/)
        {
            //_navigationService = navigationService;
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

            //AddCommissionMemberCommand = new RelayCommand(AddCommissionMember);
            //SaveCommissionMembersCommand = new RelayCommand(SaveCommissionMembersChanges);
            //DeleteCommissionMemberCommand = new RelayCommand(DeleteSelectedMember);
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
                var removedCommision = _dbContext.Commissions.FirstOrDefault(c => c.CodeMembersCommission == SelectedCommission.CodeMembersCommission);
                _dbContext.Commissions.Remove(removedCommision);
                _dbContext.SaveChanges();
                Commissions.Remove(SelectedCommission);
            }
        }
        #endregion

        #region CommissionMembers commands
        private void AddCommissionMember()
        {

        }

        private void SaveCommissionMembersChanges()
        {

        }

        private void DeleteSelectedMember()
        {
            //var selectedMember = Commissions.FirstOrDefault();
            //if (selectedMember != null)
            //{
            //    var member = _dbContext.MembersDuma.FirstOrDefault(m => m.CodeMembersDuma == selectedMember.CodeMembersDuma);
            //    var commission = _dbContext.Commissions.FirstOrDefault(c => c.CodeMembersCommission == selectedMember.CodeMembersCommission);

            //    if (member != null && commission != null)
            //    {
            //        var memberCommission = _dbContext.MembersCommission
            //       .FirstOrDefault(mc => mc.CodeMembersCommission == commission.CodeMembersCommission && mc.CodeMembersDuma == member.CodeMembersDuma);

            //        if (memberCommission != null)
            //        {
            //            _dbContext.MembersCommission.Remove(memberCommission);

            //            _dbContext.SaveChanges();
            //            Commissions.Remove(selectedMember);
            //        }
            //    }
            //}
        }

        private void LoadCommissionMembers(CommissionsDto commission)
        {
            if (commission != null)
            {
                var commissionMembers = _dbContext.MembersDuma
                    .Include(md => md.MembersCommission)
                    .Where(md => md.MembersCommission.CodeMembersCommission == commission.CodeMembersCommission)
                    .Select(md => new CommissionMembersDto
                    {
                        CodeMembersDuma = md.CodeMembersDuma,
                        Name = md.Name,
                        Surname = md.Surname,
                        Patronymic = md.Patronymic
                    })
                    .ToList();

                CommissionMembers = new ObservableCollection<CommissionMembersDto>(commissionMembers);
                
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
