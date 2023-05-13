using Infrastructure.Common;
using DomainServices.Entities;
using Infrastructure.Repository.InterfaceRepository;
using DomainServices.DBService;

namespace Infrastructure.Repository
{
    public class FileRPDRepository : GenericRepository<FileRPD>, IFileRPDRepository
    {
        public FileRPDRepository(ApplicationContext dbContext) : base(dbContext)
        {
        }
    }
}
