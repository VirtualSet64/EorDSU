using Microsoft.AspNetCore.Identity;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;

namespace EorDSU.Models
{
    public class User : IdentityUser
    {
        public int PersDepartmentId { get; set; }
    }
}
