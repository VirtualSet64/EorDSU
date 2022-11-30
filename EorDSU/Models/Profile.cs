using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EorDSU.Models
{
    /// <summary>
    /// Профиль обучения
    /// </summary>
    public class Profile
    {
        [Key]
        public int Id { get; set; } 
        /// <summary>
        /// Название профиля
        /// </summary>
        public string? ProfileName { get; set; }        
        /// <summary>
        /// Срок обучения
        /// </summary>
        public int? TermEdu { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int? Year { get; set; }
        /// <summary>
        /// Ссылка на страницу с результатами приема
        /// </summary>
        public string? LinkToPriemResult { get; set; }
        /// <summary>
        /// Ссылка на страницу с предметами
        /// </summary>
        public string? LinkToPRD { get; set; }
        /// <summary>
        /// Список дисциплин
        /// </summary>
        public List<Discipline>? Disciplines { get; set; } = new List<Discipline>();
        /// <summary>
        /// Список файлов
        /// </summary>
        public List<FileModel>? FileModels { get; set; } = new List<FileModel>();
        /// <summary>
        /// Id уровня образования
        /// </summary>
        public int? LevelEduId { get; set; }
        /// <summary>
        /// Уровень образования
        /// </summary>
        public LevelEdu? LevelEdu { get; set; }
        /// <summary>
        /// Id формы обучения
        /// </summary>
        public int? CaseCEdukindId { get; set; }
        /// <summary>
        /// Id направления
        /// </summary>
        public int? CaseSDepartmentId { get; set; }
        /// <summary>
        /// Id кафедры
        /// </summary>
        public int? PersDepartmentId { get; set; }
        public bool IsDeleted { get; set; }
    }
}
