using EorDSU.Common;
using EorDSU.Common.Interfaces;
using EorDSU.Models;
using EorDSU.Repository.InterfaceRepository;
using Microsoft.EntityFrameworkCore;

namespace EorDSU.Repository
{
    public class StatusDisciplineRepository : GenericRepository<StatusDiscipline>, IStatusDisciplineRepository
    {
        public StatusDisciplineRepository(DbContext dbContext) : base(dbContext)
        {

        }

        public IQueryable<StatusDiscipline> GetStatusDiscipline()
        {
            return Get(x => x.IsDeleted == false);
        }

        public StatusDiscipline GetStatusDisciplineById(int id)
        {
            return GetStatusDisciplineById(id);
        }

        public async Task<StatusDiscipline> RemoveStatusDiscipline(int id)
        {
            var statusDiscipline = GetStatusDisciplineById(id);
            await Remove(statusDiscipline);
            return statusDiscipline;
        }
    }
}
