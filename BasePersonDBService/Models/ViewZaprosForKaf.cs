namespace BasePersonDBService.Models
{
    public partial class ViewZaprosForKaf
    {
        public int? IdСотрудника { get; set; }
        public string ФИО { get; set; } = null!;
        public DateTime? ДатаРождения { get; set; }
        public int? Возраст { get; set; }
        public string? Национальность { get; set; }
        public string? Образование { get; set; }
        public string? УчСтепень { get; set; }
        public string? УчЗвание { get; set; }
        public string Филиал { get; set; } = null!;
        public string Факультет { get; set; } = null!;
        public string? Кафедра { get; set; }
        public string Должность { get; set; } = null!;
        public decimal? Ставка { get; set; }
        public string? Финансирование { get; set; }
        public string? Категория { get; set; }
        public string? ТипДолжности { get; set; }
        public int? IsFaculty { get; set; }
        public string Главная { get; set; } = null!;
        public int? Отпуск { get; set; }
        public DateTime? ДатаНачала { get; set; }
        public DateTime? ДатаОкончания { get; set; }
        public int? PosId { get; set; }
        public int? Rating { get; set; }
        public string? ОтпускName { get; set; }
        public string? Kdol { get; set; }
        public string? Пол { get; set; }
        public int TransId { get; set; }
        public int? FilId { get; set; }
        public int? DivId { get; set; }
        public int? DepId { get; set; }
        public string? Photo { get; set; }
        public int? IKafPersonId { get; set; }
        public int? IsActive { get; set; }
        public int? IsKaf { get; set; }
        public string? InstitutionName { get; set; }
        public string? EduDocName { get; set; }
        public string? EduDocSer { get; set; }
        public string? EduDocNumber { get; set; }
        public int? EduEnd { get; set; }
        public string? Speciality { get; set; }
        public string? Qualification { get; set; }
        public int? IsTemp { get; set; }
        public string? Kateg { get; set; }
        public string? KdolNew { get; set; }
        public string? PosKomment { get; set; }
        public string? StrSvid { get; set; }
        public string? Inn { get; set; }
        public DateTime? DatePriem { get; set; }
    }
}
