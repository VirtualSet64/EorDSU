using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
