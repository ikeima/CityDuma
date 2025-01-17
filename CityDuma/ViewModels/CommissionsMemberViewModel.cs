using CityDuma.Domain.Dto;
using CityDuma.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

namespace CityDuma.ViewModels
{
    public class CommissionsMemberViewModel : INotifyPropertyChanged
    {
        private readonly AppDbContext _dbContext;

        public ObservableCollection<CommissionMembersDto> CommissionMembers { get; set; }
        private ObservableCollection<CommissionsDto> _commissions;
        public ObservableCollection<CommissionsDto> Commissions
        {
            get => _commissions;
            set
            {
                _commissions = value;
                OnPropertyChanged(nameof(Commissions));
            }
        }

        private CommissionMembersDto _selectedMember;
        public CommissionMembersDto SelectedMember
        {
            get => _selectedMember;
            set
            {
                _selectedMember = value;
                OnPropertyChanged(nameof(SelectedMember));
            }
        }
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

        public ICommand DeleteMemberCommand { get; set; }
        public ICommand SaveChangesCommand { get; set; }

        public CommissionsMemberViewModel()
        {
            _dbContext = new AppDbContext();

            var chairmans = _dbContext.Commissions
                .Select(c => new ChairmanDto
                {
                    Code = c.ChairmanCode,
                    FullName = c.MembersDuma.Surname + c.MembersDuma.Name + c.MembersDuma.Patronymic
                })
                .ToList();

            var commissions = _dbContext.Commissions
                .Include(c => c.MembersDuma)
                .ThenInclude(c => c.MembersCommission)
                .AsEnumerable()
                .Select(c => 
                    new CommissionsDto
                    {
                        CodeMembersCommission = c.CodeMembersCommission,
                        Chairman = chairmans.FirstOrDefault(ch => ch.Code == c.ChairmanCode),
                        Direction = c.Direction
                    })
                .ToList();

            var members = _dbContext.MembersDuma
                .Include(m => m.Commissions)
                .ThenInclude(m => m.MembersCommission)
                .AsEnumerable()
                .Select(m =>
                    new CommissionMembersDto
                    {
                        CodeMembersDuma = m.CodeMembersDuma,
                        Name = m.Name,
                        Surname = m.Surname,
                        Patronymic = m.Patronymic,
                        Commission = commissions.FirstOrDefault(c => c.CodeMembersCommission == m.MembersCommission?.CodeMembersCommission)
                    })
                .ToList();

            Commissions = new ObservableCollection<CommissionsDto>(commissions);
            CommissionMembers = new ObservableCollection<CommissionMembersDto>(members);

            SaveChangesCommand = new RelayCommand(SaveChanges);
            DeleteMemberCommand = new RelayCommand(DeleteMemberFromCommission, CanDeleteMember);
        }


        private void SaveChanges()
        {
            foreach (var member in CommissionMembers)
            {
                var memberCommission = _dbContext.MembersCommission
                    .FirstOrDefault(mc => mc.CodeMembersDuma == member.CodeMembersDuma);

                if (memberCommission != null)
                {
                    memberCommission.CodeMembersCommission = member.Commission?.CodeMembersCommission;
                }
                else if (member.Commission != null)
                {
                    _dbContext.MembersCommission.Add(new MembersCommission
                    {
                        CodeMembersDuma = member.CodeMembersDuma,
                        CodeMembersCommission = member.Commission.CodeMembersCommission
                    });
                }
            }

            _dbContext.SaveChanges();
        }

        private bool CanDeleteMember() => SelectedMember != null;

        private void DeleteMemberFromCommission()
        {
            if (SelectedMember != null)
            {
                var memberCommission = _dbContext.MembersCommission
                    .FirstOrDefault(mc => mc.CodeMembersDuma == SelectedMember.CodeMembersDuma);
                
                if (memberCommission != null)
                {
                    _dbContext.MembersCommission.Remove(memberCommission);
                    _dbContext.SaveChanges();
                }

                SelectedMember.Commission = null;
                CommissionMembers = new ObservableCollection<CommissionMembersDto>(CommissionMembers);

                OnPropertyChanged(nameof(CommissionMembers));
                OnPropertyChanged(nameof(SelectedMember));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged == null)
            {
                Console.WriteLine("No subscribers for PropertyChanged!");
            }
            Console.WriteLine($"Property changed: {propertyName}");
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
