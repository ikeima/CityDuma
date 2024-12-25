namespace CityDuma.Domain.Dto
{
    public class CommissionMembersDto
    {
        public short CodeMembersDuma { get; set; }
        public string Surname { get; set; }
        public string Name { get; set; }
        public string Patronymic { get; set; }
        public CommissionsDto Commission { get; set; }
    }
}
