using Microsoft.AspNetCore.Http;

namespace DomainServices.DtoModels
{
    public class UploadFileRPD
    {
        public IFormFile? UploadedFile { get; set; }
        public int? AuthorId { get; set; }
        public int? DisciplineId { get; set; }
        public string? Ecp { get; set; }
    }
}
