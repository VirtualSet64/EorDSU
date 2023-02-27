using System.ComponentModel.DataAnnotations;

namespace EorDSU.ViewModels.Account
{
    public class RegisterViewModel
    {
        [Required]
        [Display(Name = "Кафедра")]
        public int PersDepartmentId { get; set; }

        [Required]
        [Display(Name = "Логин")]
        public string Login { get; set; } = null!;

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Пароль")]
        public string Password { get; set; } = null!;

        [Required]
        [Display(Name = "Роль")]
        public string Role { get; set; } = null!;
    }
}
