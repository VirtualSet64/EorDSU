using DomainServices.DBService;
using DomainServices.Entities;
using Ifrastructure.Common;
using Infrastructure.Repository.InterfaceRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repository
{
    public class ProfileKafedrasRepository : GenericRepository<ProfileKafedras>, IProfileKafedrasRepository
    {
        public ProfileKafedrasRepository(ApplicationContext dbContext) : base(dbContext)
        {

        }
    }
}
