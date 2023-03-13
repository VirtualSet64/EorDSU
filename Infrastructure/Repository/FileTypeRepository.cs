using Ifrastructure.Common;
using DomainServices.Models;
using Ifrastructure.Repository.InterfaceRepository;
using DomainServices.DBService;

namespace Ifrastructure.Repository
{
    public class FileTypeRepository : GenericRepository<FileType>, IFileTypeRepository
    {
        public FileTypeRepository(ApplicationContext dbContext) : base(dbContext)
        {
        }
    }
}
