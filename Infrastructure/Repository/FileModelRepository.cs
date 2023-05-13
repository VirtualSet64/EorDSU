using Infrastructure.Common;
using DomainServices.Entities;
using Infrastructure.Repository.InterfaceRepository;
using DomainServices.DBService;

namespace Infrastructure.Repository
{
    public class FileModelRepository : GenericRepository<FileModel>, IFileModelRepository
    {
        public FileModelRepository(ApplicationContext dbContext) : base(dbContext)
        {
        }
    }
}
