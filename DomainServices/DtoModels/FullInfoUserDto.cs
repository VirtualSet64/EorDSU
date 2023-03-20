using BasePersonDBService.Models;
using DomainServices.Entities;

namespace DomainServices.DtoModels
{
    public class FullInfoUserDto
    {
        public User? User { get; set; }
        public List<PersDivision>? Faculties { get; set; }
        public PersDepartment? Department { get; set; }
        public string? Role { get; set; }
    }
}
