namespace DomainServices.DtoModels.Account
{
    public class EditViewModel
    {
        public string Id { get; set; } = null!;
        public string Login { get; set; } = null!;
        public string Password { get; set; } = null!;
        public int? PersDepartmentId { get; set; }
        public List<int>? Faculties { get; set; }
        public string? Role { get; set; }
    }
}
