using System.ComponentModel.DataAnnotations;

namespace EorDSU.ViewModels
{
    public class RegisterViewModel
    {
        [Required]
        [Display(Name = "Кафедра")]
        public int PersDepartmentId { get; set; }

        [Required]
        [Display(Name = "Логин")]
        public string Login { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Пароль")]
        public string Password { get; set; }
    }
}
