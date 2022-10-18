using EorDSU.Models;

namespace EorDSU.Interface
{
    public interface IDSURepository
    {
        public List<Faculty> GetFaculty();
        public List<Cathedra> GetCathedra();
        public List<Department> GetDepartment();
        public List<Discipline> GetDiscipline();
    }
}
