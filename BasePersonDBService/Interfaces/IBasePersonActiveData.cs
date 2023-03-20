using BasePersonDBService.Models;

namespace BasePersonDBService.Interfaces
{
    public interface IBasePersonActiveData
    {
        public IQueryable<PersFilial> GetPersFilials();
        public PersFilial GetPersFilialById(int id);
        public IQueryable<PersDivision> GetPersDivisions();
        public PersDivision GetPersDivisionById(int id);
        public IQueryable<PersDepartment> GetPersDepartments();
        public PersDepartment GetPersDepartmentById(int? id);
        public PersDepartment GetPersDepartmentByName(string name);
        public IQueryable<PersDepartment> GetPersDepartmentByDivisionId(int id);
        public IQueryable<Person> GetPersons();
        public Person GetPersonById(int id);
        public IQueryable<ViewZaprosForKaf> GetPrepods();
    }
}
