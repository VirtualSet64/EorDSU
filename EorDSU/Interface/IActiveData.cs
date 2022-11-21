using EorDSU.Models;

namespace EorDSU.Interface
{
    public interface IActiveData
    {
        public List<PersDivision> GetPersDivisions();
        public List<PersDepartment> GetPersDepartments();
        public List<Person> GetPerson(); 
    }
}
