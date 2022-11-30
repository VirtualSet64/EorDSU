using Microsoft.AspNetCore.Identity;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;

namespace EorDSU.Models
{
    public class User
    {
        public int Id { get; set; }
        public string UserName { get; set; } = null!;
        [PasswordPropertyText]
        public string Password { get; set; } = null!;        
        public int? PersDepartmentId { get; set; }
        public bool IsDeleted { get; set; }
    }
}
