using Ifrastructure.Common.Interfaces;
using DomainServices.Models;

namespace Ifrastructure.Repository.InterfaceRepository
{
    public interface IStatusDisciplineRepository : IGenericRepository<StatusDiscipline>
    {
        public List<StatusDiscipline> GetStatusDiscipline();
        public List<StatusDiscipline> GetRemovableStatusDiscipline();
        public Task<StatusDiscipline> RequestDeleteStatusDiscipline(int id);
        public Task RemoveStatusDiscipline(int id);
    }
}
