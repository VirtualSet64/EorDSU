using EorDSU.Common;
using EorDSU.DBService;
using EorDSU.Models;
using EorDSU.Repository.InterfaceRepository;
using Microsoft.EntityFrameworkCore;

namespace EorDSU.Repository
{
    public class FileTypeRepository : GenericRepository<FileType>, IFileTypeRepository
    {
        public FileTypeRepository(ApplicationContext dbContext) : base(dbContext)
        {
        }
    }
}
