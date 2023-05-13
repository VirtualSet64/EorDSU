using Infrastructure.Common;
using DomainServices.Entities;
using Infrastructure.Repository.InterfaceRepository;
using DomainServices.DBService;

namespace Infrastructure.Repository
{
    public class FileTypeRepository : GenericRepository<FileType>, IFileTypeRepository
    {
        public FileTypeRepository(ApplicationContext dbContext) : base(dbContext)
        {
        }
    }
}
