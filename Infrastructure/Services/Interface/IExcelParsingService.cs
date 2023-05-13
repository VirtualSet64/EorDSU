using DomainServices.Entities;

namespace Infrastructure.Services.Interface
{
    public interface IExcelParsingService
    {
        public Task<Profile> ParsingService(string path);
    }
}
