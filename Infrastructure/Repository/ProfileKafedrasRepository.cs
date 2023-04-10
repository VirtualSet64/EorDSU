using DomainServices.DBService;
using DomainServices.Entities;
using Ifrastructure.Common;
using Infrastructure.Repository.InterfaceRepository;

namespace Infrastructure.Repository
{
    public class ProfileKafedrasRepository : GenericRepository<ProfileKafedras>, IProfileKafedrasRepository
    {
        public ProfileKafedrasRepository(ApplicationContext dbContext) : base(dbContext)
        {

        }
    }
}
