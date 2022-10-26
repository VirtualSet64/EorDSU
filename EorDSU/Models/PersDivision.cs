using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EorDSU.Models
{
    /// <summary>
    /// Факультет
    /// </summary>
    public class PersDivision
    {
        public int DivId { get; set; }
        [Key]
        public int FilId { get; set; }
        [Key]
        public string DivName { get; set; } = null!;
        public string? DivAbr { get; set; }
        public int? CreateYear { get; set; }
        public DateTime? DateAdd { get; set; }
        public string? UserLogin { get; set; }
        public int? IsFaculty { get; set; }
        public int? IsActive { get; set; }
        public int? OrgStr { get; set; }
        public int? OrgRate { get; set; }
        public int? ForEor { get; set; }
        public string? AbbrEng { get; set; }
        public int? OldId { get; set; }
    }
}
