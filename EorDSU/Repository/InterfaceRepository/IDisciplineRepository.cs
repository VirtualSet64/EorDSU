using EorDSU.Common.Interfaces;
using EorDSU.Models;
using EorDSU.ViewModels;

namespace EorDSU.Repository.InterfaceRepository
{
    public interface IDisciplineRepository : IGenericRepository<Discipline>
    {
        public DataForTableResponse GetDisciplinesByProfileId(int profileId);
        public Task<List<Discipline>> GetRemovableDiscipline(int facultyId);
        public Discipline GetDisciplinesById(int id);
        public Task<Discipline> RemoveDiscipline(int id);
        public Task<Discipline> RequestDeleteDiscipline(int id);
    }
}
