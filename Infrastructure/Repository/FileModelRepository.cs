using Ifrastructure.Common;
using DomainServices.Entities;
using Ifrastructure.Repository.InterfaceRepository;
using DomainServices.DBService;

namespace Ifrastructure.Repository
{
    public class FileModelRepository : GenericRepository<FileModel>, IFileModelRepository
    {
        public FileModelRepository(ApplicationContext dbContext) : base(dbContext)
        {
        }
    }
}
