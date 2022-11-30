using EorDSU.Models;

namespace EorDSU.ResponseModel
{
    public class DataResponseForSvedenOOPDGU
    {
        public Profile? Profiles { get; set; }
        public CaseSDepartment? CaseSDepartment { get; set; }
        public CaseCEdukind? CaseCEdukind { get; set; }
        public string? SrokDeystvGosAccred { get; set; }
    }
}
