namespace SvedenOop.Services.Interfaces
{
    public interface IAddFileOnServer
    {
        public Task<string> CreateFile(IFormFile uploadedFile);
    }
}
