using Ifrastructure.Common;
using DomainServices.Entities;
using Ifrastructure.Repository.InterfaceRepository;
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
