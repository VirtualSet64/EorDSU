namespace DomainServices.DtoModels.Account
{
    public class UserIncludeRolesViewModel
    {
        public string? UserId { get; set; }
        public string? UserName { get; set; }
        public int? PersDepartmentId { get; set; }
        public string? Role { get; set; }
    }
}
