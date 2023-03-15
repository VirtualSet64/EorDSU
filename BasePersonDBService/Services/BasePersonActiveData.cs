using BasePersonDBService.DataContext;
using BasePersonDBService.Interfaces;
using BasePersonDBService.Models;

namespace BasePersonDBService.Services
{
    public class BasePersonActiveData : IBasePersonActiveData
    {
        private readonly BASEPERSONMDFContext _bASEPERSONMDFContext;
        public BasePersonActiveData(BASEPERSONMDFContext bASEPERSONMDFContext)
        {
            _bASEPERSONMDFContext = bASEPERSONMDFContext;
        }

        public IQueryable<PersDepartment> GetPersDepartmentByDivisionId(int id)
        {
            return _bASEPERSONMDFContext.PersDepartments.Where(x => x.DivId == id && x.IsActive == 1 && x.IsKaf == 1 && x.IsMain == 0);
        }

        public PersDepartment GetPersDepartmentById(int id)
        {
            return _bASEPERSONMDFContext.PersDepartments.FirstOrDefault(x => x.DepId == id);
        }

        public PersDepartment GetPersDepartmentByName(string name)
        {
            return _bASEPERSONMDFContext.PersDepartments.FirstOrDefault(x => x.DepName == name);
        }

        public IQueryable<PersDepartment> GetPersDepartments()
        {
            return _bASEPERSONMDFContext.PersDepartments.Where(x => x.IsActive == 1 && x.IsKaf == 1 && x.IsMain == 0);
        }

        public PersDivision GetPersDivisionById(int id)
        {
            return _bASEPERSONMDFContext.PersDivisions.FirstOrDefault(x => x.DivId == id);
        }

        public IQueryable<PersDivision> GetPersDivisions()
        {
            return _bASEPERSONMDFContext.PersDivisions.Where(x => x.IsActive == 1 && x.IsFaculty == 1 && x.ForEor == 1);
        }

        public PersFilial GetPersFilialById(int id)
        {
            return _bASEPERSONMDFContext.PersFilials.FirstOrDefault(x => x.FilId == id);
        }

        public IQueryable<PersFilial> GetPersFilials()
        {
            return _bASEPERSONMDFContext.PersFilials.Where(x => x.IsActive == 1);
        }

        public Person GetPersonById(int id)
        {
            return _bASEPERSONMDFContext.People.FirstOrDefault(x => x.PersonId == id);
        }

        public IQueryable<Person> GetPersons()
        {
            return _bASEPERSONMDFContext.People;
        }

        public IQueryable<ViewZaprosForKaf> GetPrepods()
        {
            return _bASEPERSONMDFContext.ViewZaprosForKafs.Where(x => x.IsActive == 1 && x.Категория == "ППС");
        }
    }
}
