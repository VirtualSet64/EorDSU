using Infrastructure.Repository.InterfaceRepository;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using SvedenOop.Services.Interfaces;

namespace SvedenOop.Services
{
    public class GenerateJsonService : IGenerateJsonService
    {
        private readonly IProfileRepository _profileRepository;
        private readonly IDisciplineRepository _disciplineRepository;
        private readonly IHostEnvironment _hostEnvironment;
        private string FileJsonPath { get; set; }
        private string FileJson2Path { get; set; }

        public GenerateJsonService(IProfileRepository profileRepository, IDisciplineRepository disciplineRepository, IHostEnvironment hostEnvironment, IConfiguration configuration)
        {
            _profileRepository = profileRepository;
            _disciplineRepository = disciplineRepository;
            _hostEnvironment = hostEnvironment;
            FileJsonPath = configuration["FileJson"];
            FileJson2Path = configuration["FileJson2"];
        }

        public void GenerateJsonFile()
        {
            var profileDto = _profileRepository.GetDataForOopDgu();
            profileDto.ForEach(x => x.Disciplines = _disciplineRepository.Get()
                                                                         .Include(d => d.FileRPD)
                                                                         .Where(c => c.ProfileId == x.Profile.Id && c.Code.Contains("Б2") == true)
                                                                         .ToList());

            string json = JsonConvert.SerializeObject(profileDto, Formatting.Indented,
                new JsonSerializerSettings
                {
                    PreserveReferencesHandling = PreserveReferencesHandling.Objects
                });

            try
            {
                string text = File.ReadAllText(FileJson2Path);
                File.WriteAllText(FileJsonPath, json);
                File.Delete(FileJson2Path);
            }
            catch (Exception)
            {
                File.WriteAllText(FileJson2Path, json);
                File.Delete(FileJsonPath);
            }
        }

        public string GetGeneratedJsonFile()
        {
            try
            {
                return File.ReadAllText(FileJson2Path);
            }
            catch (Exception)
            {
                return File.ReadAllText(FileJsonPath);
            }
        }
    }
}
