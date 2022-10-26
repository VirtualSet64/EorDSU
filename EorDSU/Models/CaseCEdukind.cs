using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EorDSU.Models
{
    /// <summary>
    /// Форма обучения
    /// </summary>
    public class CaseCEdukind
    {
        [Key]
        public short EdukindId { get; set; }
        public string? Edukind { get; set; }
        public short? Yearedu { get; set; }
        public string? Abr { get; set; }
    }
}
