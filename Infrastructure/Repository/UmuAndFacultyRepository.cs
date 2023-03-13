using Ifrastructure.Common;
using DomainServices.Models;
using Ifrastructure.Repository.InterfaceRepository;
using DomainServices.DBService;

namespace Ifrastructure.Repository
{
    public class UmuAndFacultyRepository : GenericRepository<UmuAndFaculty>, IUmuAndFacultyRepository
    {
        public UmuAndFacultyRepository(ApplicationContext dbContext) : base(dbContext)
        {

        }
    }
}
