using SvedenOop.Services.Interfaces;

namespace SvedenOop.Services
{
    public class AddFileOnServer : IAddFileOnServer
    {
        private readonly IWebHostEnvironment _appEnvironment;
        private readonly IConfiguration Configuration;
        private readonly IHostEnvironment _hostEnvironment;
        public AddFileOnServer(IWebHostEnvironment appEnvironment, IConfiguration configuration, IHostEnvironment hostEnvironment)
        {
            _appEnvironment = appEnvironment;
            Configuration = configuration;
            _hostEnvironment = hostEnvironment;
        }

        public async Task<string> CreateFile(IFormFile uploadedFile)
        {
            string path = _hostEnvironment.ContentRootPath + Configuration["FileFolder"] + uploadedFile.FileName;
            using (var fileStream = new FileStream(_appEnvironment.WebRootPath + path, FileMode.Create))
                await uploadedFile.CopyToAsync(fileStream);
            return path;
        }
    }
}
