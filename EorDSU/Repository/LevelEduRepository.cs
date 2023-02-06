using EorDSU.Common;
using EorDSU.Models;
using EorDSU.Repository.InterfaceRepository;
using Microsoft.EntityFrameworkCore;

namespace EorDSU.Repository
{
    public class LevelEduRepository : GenericRepository<LevelEdu>, ILevelEduRepository
    {
        public LevelEduRepository(DbContext dbContext) : base(dbContext)
        {

        }
    }
}
