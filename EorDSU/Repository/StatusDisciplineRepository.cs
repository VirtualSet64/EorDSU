﻿using EorDSU.Common;
using EorDSU.Common.Interfaces;
using EorDSU.Models;
using EorDSU.Repository.InterfaceRepository;
using Microsoft.EntityFrameworkCore;

namespace EorDSU.Repository
{
    public class StatusDisciplineRepository : GenericRepository<StatusDiscipline>, IStatusDisciplineRepository
    {
        public StatusDisciplineRepository(DbContext dbContext) : base(dbContext)
        {

        }

        public List<StatusDiscipline> GetStatusDiscipline()
        {
            return Get(x => x.IsDeleted == false).ToList();
        }

        public async Task<StatusDiscipline> GetStatusDisciplineById(int id)
        {
            return await FindById(id);
        }

        public async Task<StatusDiscipline> RemoveStatusDiscipline(int id)
        {
            var statusDiscipline = await FindById(id);
            await Remove(statusDiscipline);
            return statusDiscipline;
        }
    }
}
