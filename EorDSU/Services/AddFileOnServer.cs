using EorDSU.Services.Interfaces;

namespace EorDSU.Services
{
    public class AddFileOnServer : IAddFileOnServer
    {
        private readonly IWebHostEnvironment _appEnvironment;
        private readonly IConfiguration Configuration;
        public AddFileOnServer(IWebHostEnvironment appEnvironment, IConfiguration configuration)
        {
            _appEnvironment = appEnvironment;
            Configuration = configuration;
        }

        public async Task<string> CreateFile(IFormFile uploadedFile)
        {
            string path = Configuration["FileFolder"] + uploadedFile.FileName;
            using (var fileStream = new FileStream(_appEnvironment.WebRootPath + path, FileMode.Create))
                await uploadedFile.CopyToAsync(fileStream);
            return path;
        }
    }
}
