using Microsoft.AspNetCore.Http;

namespace DomainServices.DtoModels
{
    public class UploadFileForFileModel
    {
        public int? FileId { get;set; }
        public IFormFile? UploadedFile { get; set; }
        public string? FileName { get; set; }
        public int FileType { get; set; }
        public int ProfileId { get; set; }
        public string? Ecp { get; set; }
    }
}
