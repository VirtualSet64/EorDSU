using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EorDSU.Models
{
    /// <summary>
    /// Кафедра
    /// </summary>
    public class PersDepartment
    {
        [Key]
        public int DepId { get; set; }
        public int? DivId { get; set; }
        public string? DepName { get; set; }
        public string? DepAbr { get; set; }
        public int? CreateYear { get; set; }
        public int? IsMain { get; set; }
        public DateTime? DateAdd { get; set; }
        public string? UserLogin { get; set; }
        public int? IsActive { get; set; }
        public int? DepType { get; set; }
        public int? IsKaf { get; set; }
        public string? Address { get; set; }
        public int? ForEor { get; set; }
    }
}
