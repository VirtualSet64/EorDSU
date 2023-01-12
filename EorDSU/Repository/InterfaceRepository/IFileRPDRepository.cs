﻿using EorDSU.Common.Interfaces;
using EorDSU.Models;

namespace EorDSU.Repository.InterfaceRepository
{
    public interface IFileRPDRepository : IGenericRepository<FileRPD>
    {
        public Task<FileRPD> CreateFileRPD(IFormFile uploadedFile, int disciplineId);
        public Task<FileRPD> EditFileRPD(IFormFile uploadedFile, int disciplineId);
    }
}
