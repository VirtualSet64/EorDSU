using EorDSU.Common.Interfaces;
using EorDSU.Models;

namespace EorDSU.Repository.InterfaceRepository
{
    public interface IStatusDisciplineRepository : IGenericRepository<StatusDiscipline>
    {
        public List<StatusDiscipline> GetStatusDiscipline();
        public List<StatusDiscipline> GetRemovableStatusDiscipline();
        public StatusDiscipline GetStatusDisciplineById(int id);
        public Task<StatusDiscipline> RemoveStatusDiscipline(int id);
    }
}
