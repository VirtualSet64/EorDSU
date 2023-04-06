namespace DomainServices.Entities
{
    public class ProfileKafedras
    {
        public int Id { get; set; }
        public int ProfileId { get; set; }
        public Profile? Profile { get; set; }
        public int PersDepartmentId { get; set; }
    }
}
