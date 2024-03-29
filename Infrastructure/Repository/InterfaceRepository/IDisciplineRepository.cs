﻿using Infrastructure.Common.Interfaces;
using DomainServices.Entities;
using DomainServices.DtoModels;

namespace Infrastructure.Repository.InterfaceRepository
{
    public interface IDisciplineRepository : IGenericRepository<Discipline>
    {
        public DataForTableResponse GetDisciplinesByProfileId(int profileId);
        public Task<List<Discipline>> GetRemovableDiscipline(int facultyId);
        public Discipline GetDisciplinesById(int id);
        public Task<Discipline> RemoveDiscipline(int id);
        public Task<Discipline> RequestDeleteDiscipline(int id);
    }
}
