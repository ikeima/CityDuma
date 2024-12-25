using CityDuma.Domain.Dto;
using CityDuma.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;

namespace CityDuma.ViewModels
{
    public class CommissionsMemberViewModel : INotifyPropertyChanged
    {
        private readonly AppDbContext _dbContext;

        public ObservableCollection<CommissionMembersDto> CommissionMembers { get; set; }
        public ObservableCollection<CommissionsDto> Commissions { get; set; }

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

            var commissions = _dbContext.Commissions.Select(c => new CommissionsDto
            {
                CodeMembersCommission = c.CodeMembersCommission,
                Direction = c.Direction
            }).ToList();

            // Загружаем данные о членах комиссии и связываем их с комиссиями
            var members = _dbContext.MembersDuma
                 .Include(m => m.MembersCommission)  // Предположим, что это связь "один к одному" с комиссией
                 .ThenInclude(mc => mc.Commissions)  // Включаем информацию о комиссии
                 .Select(m => new CommissionMembersDto
                 {
                     CodeMembersDuma = m.CodeMembersDuma,
                     Surname = m.Surname,
                     Name = m.Name,
                     Patronymic = m.Patronymic,
                     Commission = m.MembersCommission != null
                                ? commissions.FirstOrDefault(c => c.CodeMembersCommission == m.MembersCommission.CodeMembersCommission)
                                : null
                 })
                 .ToList();

            Commissions = new ObservableCollection<CommissionsDto>(commissions);
            CommissionMembers = new ObservableCollection<CommissionMembersDto>(members);

            SaveChangesCommand = new RelayCommand(SaveChanges);
            DeleteMemberCommand = new RelayCommand(DeleteMember, CanDeleteMember);
        }


        private void SaveChanges()
        {
            foreach (var member in CommissionMembers)
            {
                var memberCommission = _dbContext.MembersCommission
                    .FirstOrDefault(mc => mc.CodeMembersDuma == member.CodeMembersDuma);

                if (memberCommission != null)
                {
                    memberCommission.CodeMembersCommission = member.Commission.CodeMembersCommission;
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

        private void DeleteMember()
        {
            if (SelectedMember != null)
            {
                var member = _dbContext.MembersDuma.FirstOrDefault(m => m.CodeMembersDuma == SelectedMember.CodeMembersDuma);
                if (member != null)
                {
                    _dbContext.MembersDuma.Remove(member);
                    _dbContext.SaveChanges();
                    CommissionMembers.Remove(SelectedMember);
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
