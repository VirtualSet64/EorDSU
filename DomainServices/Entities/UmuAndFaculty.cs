namespace DomainServices.Entities
{
    public class UmuAndFaculty
    {
        public int Id { get; set; }
        public string? UserId { get; set; }
        public User? User { get; set; }
        public int FacultyId { get;set; }
    }
}
