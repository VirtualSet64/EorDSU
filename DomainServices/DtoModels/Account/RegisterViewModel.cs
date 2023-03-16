﻿using System.ComponentModel.DataAnnotations;

namespace DomainServices.DtoModels.Account
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

        [Display(Name = "Роль")]
        public string? Role { get; set; }
        [Display(Name = "Роль")]
        public List<int>? Faculties { get; set; }
    }
}
