using Ifrastructure.Common;
using DomainServices.Models;
using Ifrastructure.Repository.InterfaceRepository;
using Microsoft.EntityFrameworkCore;
using DomainServices.DBService;

namespace Ifrastructure.Repository
{
    public class LevelEduRepository : GenericRepository<LevelEdu>, ILevelEduRepository
    {
        public LevelEduRepository(ApplicationContext dbContext) : base(dbContext)
        {

        }
    }
}
