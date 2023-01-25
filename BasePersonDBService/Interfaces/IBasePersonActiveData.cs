using Models;

namespace BasePersonDBService.Interfaces
{
    public interface IBasePersonActiveData
    {
        public IQueryable<PersFilial> GetPersFilials();
        public PersFilial GetPersFilialById(int id);
        public IQueryable<PersDivision> GetPersDivisions();
        public PersDivision GetPersDivisionById(int id);
        public IQueryable<PersDepartment> GetPersDepartments();
        public PersDepartment GetPersDepartmentById(int id);
        public IQueryable<Person> GetPersons();
        public Person GetPersonById(int id);
    }
}
