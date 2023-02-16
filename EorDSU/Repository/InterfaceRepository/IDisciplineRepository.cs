using EorDSU.Common.Interfaces;
using EorDSU.Models;
using EorDSU.ViewModels;

namespace EorDSU.Repository.InterfaceRepository
{
    public interface IDisciplineRepository : IGenericRepository<Discipline>
    {
        public ResponseForDiscipline GetDisciplinesByProfileId(int profileId);
        public Discipline GetDisciplinesById(int id);
        public Task<Discipline> RemoveDiscipline(int id);
    }
}
