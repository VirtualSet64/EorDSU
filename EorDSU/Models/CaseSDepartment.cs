using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EorDSU.Models
{
    /// <summary>
    /// Направление
    /// </summary>
    public class CaseSDepartment
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Key]
        public int DepartmentId { get; set; }
        public int FacId { get; set; }
        public string? DeptName { get; set; }
        public string? Abr { get; set; }
        public string? Code { get; set; }
        public string? Qualification { get; set; }
        public string? Godequalif { get; set; }
        public bool Deleted { get; set; }
    }
}
