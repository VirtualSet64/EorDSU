using DSUContextDBService.Models;
using EorDSU.Models;
using Models;

namespace EorDSU.Interface
{
    public interface ISearchEntity
    {
        public Task<short?> SearchEdukind(string text);
        public Task<int?> SearchCaseSDepartment(string text);
        public Task<int?> SearchPersDepartment(string text);
        public Task<StatusDiscipline> SearchStatusDiscipline(string text);
        public Task<LevelEdu> SearchLevelEdu(string text);
    }
}
