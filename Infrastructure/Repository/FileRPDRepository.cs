using Ifrastructure.Common;
using DomainServices.Entities;
using Ifrastructure.Repository.InterfaceRepository;
using DomainServices.DBService;

namespace Ifrastructure.Repository
{
    public class FileRPDRepository : GenericRepository<FileRPD>, IFileRPDRepository
    {
        public FileRPDRepository(ApplicationContext dbContext) : base(dbContext)
        {
        }
    }
}
