using DSUContextDBService.Models;
using DomainServices.Entities;

namespace DomainServices.DtoModels
{
    public class DataResponseForSvedenOOPDGU
    {
        public Profile? Profile { get; set; }
        public CaseSDepartment? CaseSDepartment { get; set; }
        public CaseCEdukind? CaseCEdukind { get; set; }
    }
}
