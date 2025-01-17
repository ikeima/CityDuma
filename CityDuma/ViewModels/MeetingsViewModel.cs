using CityDuma.Domain.Dto;
using CityDuma.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TaskbarClock;
using System.Text.RegularExpressions;
using System.Windows.Input;

namespace CityDuma.ViewModels
{
    public class MeetingsViewModel : INotifyPropertyChanged, IDataErrorInfo
    {
        private AppDbContext _dbContext;
        private ObservableCollection<MeetingsDto> _meetings;
        public ObservableCollection<MeetingsDto> Meetings
        {
            get => _meetings;
            set
            {
                _meetings = value;
                OnPropertyChanged(nameof(Meetings));
            }
        }

        public ObservableCollection<CommissionsDto> _commissions;
        public ObservableCollection<CommissionsDto> Commissions
        {
            get => _commissions;
            set
            {
                _commissions = value;
                OnPropertyChanged(nameof(Commissions));
            }
        }
        private ObservableCollection<OrganizersDto> _organizers;
        public ObservableCollection<OrganizersDto> Organizers
        {
            get => _organizers;
            set
            {
                _organizers = value;
                OnPropertyChanged(nameof(Organizers));
            }
        }

        private MeetingsDto _selectedMeeting;
        public MeetingsDto SelectedMeeting
        {
            get => _selectedMeeting;
            set
            {
                _selectedMeeting = value;
                OnPropertyChanged(nameof(SelectedMeeting));
            }
        }


        private string startTime;
        public string StartTime
        {
            get => startTime;
            set
            {
                if (startTime != value)
                {
                    startTime = value;
                    OnPropertyChanged(nameof(StartTime));
                }
            }
        }

        public string Error => null;
        public string this[string columnName]
        {
            get
            {
                if (columnName == nameof(StartTime))
                {
                    var regex = new Regex(@"^([01]?[0-9]|2[0-3]):([0-5]?[0-9])$");
                    if (!regex.IsMatch(StartTime))
                    {
                        return "Время должно быть в формате hh:mm";
                    }
                }

                return null;
            }
        }


        public ICommand DeleteMemberCommand { get; set; }
        public ICommand SaveChangesCommand { get; set; }
        public ICommand AddMeetingCommand { get; set; }

        public MeetingsViewModel()
        {
            _dbContext = new AppDbContext();

            var commissions = _dbContext.Commissions
                .Select(c => new CommissionsDto
                {
                    CodeMembersCommission = c.CodeMembersCommission,
                    Direction = c.Direction,
                })
                .ToList();

            var organizers = _dbContext.Organizers
                .Select(o => new OrganizersDto
                {
                    OrganizerCode = o.OrganizerCode,
                    OrganizerFullName = o.LastName + " " + o.FirstName + " " + o.Patronymic
                })
                .ToList();

            var meetings = _dbContext.Meetings
                .AsEnumerable()
                .Select(m => new MeetingsDto
                {
                    MeetingCode = m.MeetingCode,
                    Commission = commissions.FirstOrDefault(c => c.CodeMembersCommission == m.Commission.Value),
                    Organizer = organizers.FirstOrDefault(md => md.OrganizerCode == m.Organizer),
                    DateOfMeeting = m.DateOfMeeting,
                    StartTime = m.StartTime,
                    Duration = m.Duration
                });



            Commissions = new ObservableCollection<CommissionsDto>(commissions);
            Organizers = new ObservableCollection<OrganizersDto>(organizers);
            Meetings = new ObservableCollection<MeetingsDto>(meetings);



            SaveChangesCommand = new RelayCommand(SaveChanges);
            DeleteMemberCommand = new RelayCommand(DeleteMeeting, CanDeleteMeeting);
            AddMeetingCommand = new RelayCommand(AddMeeting);

        }

        private void AddMeeting()
        {
            var newMeeting = new MeetingsDto
            {
                Commission = Commissions.FirstOrDefault(),
                Organizer = Organizers.FirstOrDefault(),
                StartTime = new System.TimeSpan(12, 00, 00)
            };

            Meetings.Add(newMeeting);

            var meetingEntity = new Meetings
            {
                Commission = newMeeting.Commission.CodeMembersCommission,
                Organizer = newMeeting.Organizer.OrganizerCode,
                StartTime = newMeeting.StartTime,
            };

            _dbContext.Meetings.Add(meetingEntity);
            _dbContext.SaveChanges();

            newMeeting.MeetingCode = meetingEntity.MeetingCode;
        }

        private void SaveChanges()
        {
            foreach (var item in Meetings)
            {
                var meeting = _dbContext.Meetings.FirstOrDefault(m => m.MeetingCode == item.MeetingCode);

                if (meeting != null)
                {
                    meeting.Organizer = item.Organizer.OrganizerCode;
                    meeting.Commission = item.Commission.CodeMembersCommission;
                    meeting.Duration = item.Duration;
                    meeting.StartTime = item.StartTime;
                    meeting.DateOfMeeting = item.DateOfMeeting;
                }

                _dbContext.SaveChanges();

            }

        }

        private bool CanDeleteMeeting() => SelectedMeeting != null;
        private void DeleteMeeting()
        {
            var removedMeeting = _dbContext.Meetings.FirstOrDefault(m => m.MeetingCode == SelectedMeeting.MeetingCode);

            if (removedMeeting != null)
            {
                _dbContext.Meetings.Remove(removedMeeting);
                _dbContext.SaveChanges();

                Meetings.Remove(SelectedMeeting);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
