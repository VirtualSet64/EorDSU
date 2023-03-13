using EorDSU.Common;
using EorDSU.DBService;
using EorDSU.Models;
using EorDSU.Repository.InterfaceRepository;
using Microsoft.EntityFrameworkCore;

namespace EorDSU.Repository
{
    public class UmuAndFacultyRepository : GenericRepository<UmuAndFaculty>, IUmuAndFacultyRepository
    {
        public UmuAndFacultyRepository(ApplicationContext dbContext) : base(dbContext)
        {

        }
    }
}
