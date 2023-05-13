using Infrastructure.Common;
using DomainServices.Entities;
using Infrastructure.Repository.InterfaceRepository;
using DomainServices.DBService;

namespace Infrastructure.Repository
{
    public class LevelEduRepository : GenericRepository<LevelEdu>, ILevelEduRepository
    {
        public LevelEduRepository(ApplicationContext dbContext) : base(dbContext)
        {

        }
    }
}
