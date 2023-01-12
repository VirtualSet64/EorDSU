using EorDSU.Models;

namespace EorDSU.Interface
{
    public interface IExcelParsingService
    {
        public Task<Profile> ParsingService(string path);
    }
}
