using Infrastructure.Common;
using DomainServices.Entities;
using Infrastructure.Repository.InterfaceRepository;
using DomainServices.DBService;

namespace Infrastructure.Repository
{
    public class UmuAndFacultyRepository : GenericRepository<UmuAndFaculty>, IUmuAndFacultyRepository
    {
        public UmuAndFacultyRepository(ApplicationContext dbContext) : base(dbContext)
        {

        }
    }
}
