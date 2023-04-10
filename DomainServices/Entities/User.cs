using Microsoft.AspNetCore.Identity;

namespace DomainServices.Entities
{
    public class User : IdentityUser
    {
        public int? PersDepartmentId { get; set; }
        public List<UmuAndFaculty>? Faculty { get; set; }
    }
}
