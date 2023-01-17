using EorDSU.Models;

namespace EorDSU.Services.Interface
{
    public interface IExcelParsingService
    {
        public Task<Profile> ParsingService(string path);
    }
}
