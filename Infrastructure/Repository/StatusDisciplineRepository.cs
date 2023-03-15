using Ifrastructure.Common;
using DomainServices.Entities;
using Ifrastructure.Repository.InterfaceRepository;
using Microsoft.EntityFrameworkCore;
using DomainServices.DBService;

namespace Ifrastructure.Repository
{
    public class StatusDisciplineRepository : GenericRepository<StatusDiscipline>, IStatusDisciplineRepository
    {
        public StatusDisciplineRepository(ApplicationContext dbContext) : base(dbContext)
        {
        }

        public List<StatusDiscipline> GetStatusDiscipline()
        {
            return Get(x => x.IsDeletionRequest == false).ToList();
        }

        public async Task<List<StatusDiscipline>> GetRemovableStatusDiscipline()
        {
            return await Get().Include(x => x.Disciplines).Where(c => c.IsDeletionRequest == true).ToListAsync();
        }

        public async Task<StatusDiscipline> RequestDeleteStatusDiscipline(int id)
        {
            var statusDiscipline = FindById(id);
            statusDiscipline.IsDeletionRequest = true;
            await Update(statusDiscipline);
            return statusDiscipline;
        }

        public async Task RemoveStatusDiscipline(int id)
        {
            var statusDiscipline = Get().Include(x => x.Disciplines).FirstOrDefault(c => c.Id == id);
            if (statusDiscipline != null)
                await Remove(statusDiscipline);
        }
    }
}
