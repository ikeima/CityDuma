using System;
using System.ComponentModel;

namespace CityDuma.Domain.Dto
{
    public class CommissionMembersDto
    {
        public short CodeMembersDuma { get; set; }
        public string Surname { get; set; }
        public string Name { get; set; }
        public string Patronymic { get; set; }
        private CommissionsDto _commission;
        public CommissionsDto Commission
        {
            get => _commission;
            set
            {
                Console.WriteLine($"Commission changed for {Surname}: {value?.Direction ?? "null"}");
                _commission = value;
                OnPropertyChanged(nameof(Commission));
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged == null)
            {
                Console.WriteLine("CommissionMembersDto No subscribers for PropertyChanged!");
            }
            else
            {
                Console.WriteLine($"CommissionMembersDto Property changed: {propertyName}");
            }
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
