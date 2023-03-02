using EorDSU.Common.Interfaces;
using EorDSU.Models;
using Models;

namespace EorDSU.Repository.InterfaceRepository
{
    public interface IFileRPDRepository : IGenericRepository<FileRPD>
    {
        public Task<FileRPD?> CreateFileRPD(IFormFile uploadedFile, Person author, int disciplineId, string? ecp);
    }
}
