using DomainServices.Models;

namespace Ifrastructure.Services.Interface
{
    public interface IExcelParsingService
    {
        public Task<Profile> ParsingService(string path);
    }
}
