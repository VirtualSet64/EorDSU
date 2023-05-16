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
        private string FileJsonPathForOopDgu { get; set; }
        private string FileJson2PathForOopDgu { get; set; }
        private string FileJsonPathForOpop2 { get; set; }
        private string FileJson2PathForOpop2 { get; set; }

        public GenerateJsonService(IProfileRepository profileRepository, IDisciplineRepository disciplineRepository, IConfiguration configuration)
        {
            _profileRepository = profileRepository;
            _disciplineRepository = disciplineRepository;
            FileJsonPathForOopDgu = configuration["FileJsonForOopDgu"];
            FileJson2PathForOopDgu = configuration["FileJson2ForOopDgu"];
            FileJsonPathForOpop2 = configuration["FileJsonForOpop2"];
            FileJson2PathForOpop2 = configuration["FileJson2ForOpop2"];
        }

        public void GenerateJsonFileForOopDgu()
        {
            try
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
                    string text = File.ReadAllText(FileJson2PathForOopDgu);
                    File.WriteAllText(FileJsonPathForOopDgu, json);
                    File.Delete(FileJson2PathForOopDgu);
                }
                catch (Exception)
                {
                    try
                    {
                        File.WriteAllText(FileJson2PathForOopDgu, json);
                        File.Delete(FileJsonPathForOopDgu);
                    }
                    catch (Exception ex)
                    {
                        throw;
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public string GetGeneratedJsonFileForOopDgu()
        {
            try
            {
                return File.ReadAllText(FileJson2PathForOopDgu);
            }
            catch (Exception)
            {
                try
                {
                    return File.ReadAllText(FileJsonPathForOopDgu);
                }
                catch (Exception)
                {
                    GenerateJsonFileForOopDgu();
                    return File.ReadAllText(FileJson2PathForOopDgu);
                    throw;
                }
            }
        }

        public void GenerateJsonFileForOpop2()
        {
            var profileDto = _profileRepository.GetDataOpop2();
            profileDto.ForEach(x => x.Disciplines = _disciplineRepository.Get().Include(d => d.FileRPD)
                                                                               .Where(c => c.ProfileId == x.Profile.Id && c.Code.Contains("Б2") == true).ToList());


            string json = JsonConvert.SerializeObject(profileDto, Formatting.Indented,
                new JsonSerializerSettings
                {
                    PreserveReferencesHandling = PreserveReferencesHandling.Objects
                });

            try
            {
                string text = File.ReadAllText(FileJson2PathForOpop2);
                File.WriteAllText(FileJsonPathForOpop2, json);
                File.Delete(FileJson2PathForOpop2);
            }
            catch (Exception)
            {
                try
                {
                    File.WriteAllText(FileJson2PathForOpop2, json);
                    File.Delete(FileJsonPathForOpop2);
                }
                catch (Exception ex)
                {
                    throw;
                }

            }
        }

        public string GetGeneratedJsonFileForOpop2()
        {
            try
            {
                return File.ReadAllText(FileJson2PathForOpop2);
            }
            catch (Exception)
            {
                try
                {
                    return File.ReadAllText(FileJsonPathForOpop2);
                }
                catch (Exception)
                {
                    GenerateJsonFileForOpop2();
                    return File.ReadAllText(FileJson2PathForOpop2);
                    throw;
                }
            }
        }
    }
}
