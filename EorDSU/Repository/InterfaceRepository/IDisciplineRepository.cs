using EorDSU.Common.Interfaces;
using EorDSU.Models;

namespace EorDSU.Repository.InterfaceRepository
{
    public interface IDisciplineRepository : IGenericRepository<Discipline>
    {
        public IEnumerable<Discipline> GetDisciplinesByProfileId(int profileId);
        public Discipline GetDisciplinesById(int id);
        public Task<Discipline> RemoveDiscipline(int id);
    }
}
