namespace Models
{
    public partial class Person
    {
        public int PersonId { get; set; }
        public int? FolderId { get; set; }
        public string Surname { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string Patronymic { get; set; } = null!;
        public string? Sex { get; set; }
        public string? Photo { get; set; }
        public DateTime? Birthdate { get; set; }
        public string? Birthplace { get; set; }
        public string? BirthplaceOkato { get; set; }
        public string? CitizenshipOkin { get; set; }
        public string? NationalityOkin { get; set; }
        public string? Inn { get; set; }
        public string? StrSvid { get; set; }
        public string? AdditionalInfo { get; set; }
        public string? UserLogin { get; set; }
        public DateTime? DateInsert { get; set; }
        public DateTime? OrderDate { get; set; }
        public DateTime? DatePriem { get; set; }
        public int? IsLeave { get; set; }
        public string? TabN { get; set; }
        public string? Login { get; set; }
        public string? Password { get; set; }
        public int? IKafPersonId { get; set; }
        public string? Kdol { get; set; }
        public string? KafLogin { get; set; }
        public string? KafPassword { get; set; }
        public Guid? AspNetUserId { get; set; }
    }
}
