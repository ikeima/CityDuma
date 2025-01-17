using CityDuma.Domain.Dto;
using CityDuma.Entities;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;

namespace CityDuma.ViewModels
{
    public class OrganizersViewModel : INotifyPropertyChanged
    {
        private AppDbContext _dbContext;

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

        private OrganizersDto _selectedOrganizer;
        public OrganizersDto SelectedOrganizer
        {
            get => _selectedOrganizer;
            set
            {
                _selectedOrganizer = value;
                OnPropertyChanged(nameof(SelectedOrganizer));
            }
        }

        public ICommand DeleteMemberCommand { get; set; }
        public ICommand SaveChangesCommand { get; set; }
        public ICommand AddMeetingCommand { get; set; }
        public OrganizersViewModel()
        {
            _dbContext = new AppDbContext();

            var organizers = _dbContext.Organizers
                .Select(o => new OrganizersDto
                {
                    OrganizerCode = o.OrganizerCode,
                    OrganizerFullName = o.LastName + " " + o.FirstName + " " + o.Patronymic,
                    BirthDate = o.BirthDate,
                    PhoneNumber = o.PhoneNumber
                })
                .ToList();

            Organizers = new ObservableCollection<OrganizersDto>(organizers);

            SaveChangesCommand = new RelayCommand(SaveChanges);
            DeleteMemberCommand = new RelayCommand(DeleteOrganizer, CanDeleteOrganizer);
            AddMeetingCommand = new RelayCommand(AddOrganizer);
        }
        private void AddOrganizer()
        {
            var newOrganizer = new OrganizersDto
            {
                OrganizerFullName = "Фамилия Имя Отчество",
                PhoneNumber = "+7"
            };
            string[] fullNameParts = newOrganizer.OrganizerFullName.Split(' ');

            
            var ogranizerEntity = new Organizers
            {
                LastName = fullNameParts[0],
                FirstName = fullNameParts[1],
                Patronymic = fullNameParts[2],
                PhoneNumber = newOrganizer.PhoneNumber
            };

            _dbContext.Organizers.Add(ogranizerEntity);
            _dbContext.SaveChanges();

            newOrganizer.OrganizerCode = ogranizerEntity.OrganizerCode;
            Organizers.Add(newOrganizer);
        }

        private void SaveChanges()
        {
            foreach (var item in Organizers)
            {
                var organizer = _dbContext.Organizers.FirstOrDefault(m => m.OrganizerCode == item.OrganizerCode);

                if (organizer != null)
                {
                    string[] fullNameParts = item.OrganizerFullName.Split(' ');
                    organizer.LastName = fullNameParts[0];
                    organizer.FirstName = fullNameParts[1];
                    organizer.Patronymic = fullNameParts[2];
                    organizer.BirthDate = item.BirthDate;
                    organizer.PhoneNumber = item.PhoneNumber;
                }

                _dbContext.SaveChanges();
            }

        }

        private bool CanDeleteOrganizer() => SelectedOrganizer != null;
        private void DeleteOrganizer()
        {
            var removedOrganizer = _dbContext.Organizers.FirstOrDefault(m => m.OrganizerCode == SelectedOrganizer.OrganizerCode);

            if (removedOrganizer != null)
            {
                _dbContext.Organizers.Remove(removedOrganizer);
                _dbContext.SaveChanges();

                Organizers.Remove(SelectedOrganizer);
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
