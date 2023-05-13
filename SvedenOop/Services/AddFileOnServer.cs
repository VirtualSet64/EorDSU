using SvedenOop.Services.Interfaces;

namespace SvedenOop.Services
{
    public class AddFileOnServer : IAddFileOnServer
    {
        private readonly IWebHostEnvironment _appEnvironment;
        private readonly IConfiguration Configuration;
        private readonly IHostEnvironment _hostEnvironment;
        private string FileFolderPath { get; set; }
        public AddFileOnServer(IWebHostEnvironment appEnvironment, IHostEnvironment hostEnvironment, IConfiguration configuration)
        {
            _appEnvironment = appEnvironment;
            Configuration = configuration;
            _hostEnvironment = hostEnvironment;
            FileFolderPath = _hostEnvironment.ContentRootPath + Configuration["FileFolderFullPath"];
        }

        public async Task<string> CreateFile(IFormFile uploadedFile)
        {
            string path = FileFolderPath + uploadedFile.FileName;
            using (var fileStream = new FileStream(_appEnvironment.WebRootPath + path, FileMode.Create))
                await uploadedFile.CopyToAsync(fileStream);
            return path;
        }
    }
}
