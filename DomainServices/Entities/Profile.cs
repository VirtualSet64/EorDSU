﻿namespace DomainServices.Entities
{
    /// <summary>
    /// Профиль обучения
    /// </summary>
    public class Profile
    {
        public int Id { get; set; } 
        public int? EorId { get; set; } 
        /// <summary>
        /// Название профиля
        /// </summary>
        public string? ProfileName { get; set; }        
        /// <summary>
        /// Срок обучения
        /// </summary>
        public string? TermEdu { get; set; }
        /// <summary>
        /// Год обучения
        /// </summary>
        public int? Year { get; set; }
        /// <summary>
        /// Ссылка на страницу с результатами приема
        /// </summary>
        public string? LinkToPriemResult { get; set; }
        /// <summary>
        /// Ссылка на страницу с РПД
        /// </summary>
        public string? LinkToRPD { get; set; }
        /// <summary>
        /// Срок действия государственной аккредитации
        /// </summary>
        public string? ValidityPeriodOfStateAccreditasion { get; set; }
        /// <summary>
        /// Язык обучения
        /// </summary>
        public string? EducationLanguage { get; set; }
        /// <summary>
        /// Ссылка на дистанционное обучения
        /// </summary>
        public string? LinkToDistanceEducation { get; set; }
        /// <summary>
        /// Список дисциплин
        /// </summary>
        public List<Discipline>? Disciplines { get; set; }
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
        public List<ProfileKafedras>? ListPersDepartmentsId { get; set; }
        /// <summary>
        /// Дата создания
        /// </summary>
        public DateTime? CreateDate { get; set; } = DateTime.Now;
        /// <summary>
        /// Дата последнего обновления
        /// </summary>
        public DateTime? UpdateDate { get; set; }
    }
}
