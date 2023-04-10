﻿using Ifrastructure.Common.Interfaces;
using DomainServices.Entities;
using DomainServices.DtoModels;

namespace Ifrastructure.Repository.InterfaceRepository
{
    public interface IProfileRepository : IGenericRepository<Profile>
    {
        public List<DataForTableResponse> GetData();
        public List<DataForTableResponse> GetDataByKafedraId(int cafedraId);
        public List<DataForTableResponse> GetDataByFacultyId(int facultyId);
        public Task<List<Profile>> GetProfileByFacultyId(int facultyId);
        public Profile GetProfileById(int id);
        public Task<DataResponseForSvedenOOPDGU> ParsingProfileByFile(string path);
        public Task<Profile> UpdateProfile(Profile profile);
        public Task<Profile> RemoveProfile(int profileId);
    }
}
