using EorDSU.Common;
using EorDSU.Models;
using EorDSU.Repository.InterfaceRepository;
using Microsoft.EntityFrameworkCore;

namespace EorDSU.Repository
{
    public class UmuAndFacultyRepository : GenericRepository<UmuAndFaculty>, IUmuAndFacultyRepository
    {
        public UmuAndFacultyRepository(DbContext dbContext) : base(dbContext)
        {

        }
    }
}
