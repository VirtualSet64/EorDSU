using DSUContextDBService.Models;
using DomainServices.Models;

namespace DomainServices.DtoModels
{
    public class DataResponseForSvedenOOPDGU
    {
        public Profile? Profile { get; set; }
        public CaseSDepartment? CaseSDepartment { get; set; }
        public CaseCEdukind? CaseCEdukind { get; set; }
    }
}
