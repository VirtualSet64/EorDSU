using EorDSU.Common;
using EorDSU.Models;
using EorDSU.Repository.InterfaceRepository;
using Microsoft.EntityFrameworkCore;

namespace EorDSU.Repository
{
    public class DisciplineRepository : GenericRepository<Discipline>, IDisciplineRepository
    {
        public DisciplineRepository(DbContext dbContext) : base(dbContext) 
        { 
        
        }

        public IEnumerable<Discipline> GetDisciplinesByProfileId(int profileId)
        {
            return GetWithInclude(x => x.ProfileId == profileId, x => x.StatusDiscipline, x => x.FileRPD);
        }

        public Discipline GetDisciplinesById(int id)
        {
            return GetWithIncludeById(x => x.Id == id, x => x.StatusDiscipline, x => x.FileRPD);
        }

        public async Task<Discipline> RemoveDiscipline(int id)
        {
            var discipline = GetWithIncludeById(x => x.Id == id, x => x.FileRPD, x => x.StatusDiscipline, x => x.Profile);
            await Remove(discipline);
            return discipline;
        }
    }
}
