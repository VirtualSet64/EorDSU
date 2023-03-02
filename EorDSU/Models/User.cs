using Microsoft.AspNetCore.Identity;
using Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace EorDSU.Models
{
    public class User : IdentityUser
    {
        [PersonalData]
        public int? PersDepartmentId { get; set; }
        public List<UmuAndFaculty>? Faculty { get; set;}
    }
}
