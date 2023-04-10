using DomainServices.Entities;
using DSUContextDBService.Models;

namespace DomainServices.DtoModels
{
    public class DataForOpopTable
    {
        public Profile? Profile { get; set; }
        public CaseSDepartment? CaseSDepartment { get; set; }
        public CaseCEdukind? CaseCEdukind { get; set; }
    }
}
