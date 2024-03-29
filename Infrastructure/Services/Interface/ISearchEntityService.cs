﻿using DomainServices.Entities;

namespace Infrastructure.Services.Interface
{
    public interface ISearchEntityService
    {
        public Task<int?> SearchEdukind(string text);
        public Task<int?> SearchCaseSDepartment(string text);
        public Task<StatusDiscipline> SearchStatusDiscipline(string text);
        public Task<LevelEdu> SearchLevelEdu(string text);
        public Task<FileType> SearchFileType(string text);
    }
}
