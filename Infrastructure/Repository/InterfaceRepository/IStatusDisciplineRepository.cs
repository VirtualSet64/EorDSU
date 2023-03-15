using Ifrastructure.Common.Interfaces;
using DomainServices.Entities;

namespace Ifrastructure.Repository.InterfaceRepository
{
    public interface IStatusDisciplineRepository : IGenericRepository<StatusDiscipline>
    {
        public List<StatusDiscipline> GetStatusDiscipline();
        public Task<List<StatusDiscipline>> GetRemovableStatusDiscipline();
        public Task<StatusDiscipline> RequestDeleteStatusDiscipline(int id);
        public Task RemoveStatusDiscipline(int id);
    }
}
