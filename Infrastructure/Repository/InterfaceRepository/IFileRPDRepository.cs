﻿using Ifrastructure.Common.Interfaces;
using DomainServices.Entities;
using DomainServices.DtoModels;

namespace Ifrastructure.Repository.InterfaceRepository
{
    public interface IFileRPDRepository : IGenericRepository<FileRPD>
    {
        public Task<FileRPD?> CreateFileRPD(UploadFileRPD uploadedFile);
    }
}