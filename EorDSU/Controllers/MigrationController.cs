using AngleSharp;
using DomainServices.Entities;
using DSUContextDBService.Interfaces;
using DSUContextDBService.Models;
using EorDSU.eor;
using EorDSU.Services.Interfaces;
using Ifrastructure.Interface;
using Ifrastructure.Repository.InterfaceRepository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Office.Interop.Excel;
using Newtonsoft.Json.Linq;
using Sentry.Protocol;
using Profile = DomainServices.Entities.Profile;

namespace EorDSU.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MigrationController : Controller
    {
        private readonly IProfileRepository _profileRepository;
        private readonly IDisciplineRepository _disciplineRepository;
        private readonly IStatusDisciplineRepository _statusDisciplineRepository;
        private readonly ISearchEntityService _searchEntityService;
        private readonly EORContext _eORContext;

        public MigrationController(IProfileRepository profileRepository, IDisciplineRepository disciplineRepository, IStatusDisciplineRepository statusDisciplineRepository,
                                   ISearchEntityService searchEntityService, EORContext eORContext)
        {
            _profileRepository = profileRepository;
            _disciplineRepository = disciplineRepository;
            _statusDisciplineRepository = statusDisciplineRepository;
            _searchEntityService = searchEntityService;
            _eORContext = eORContext;
        }

        [Route("MigrationProfiles")]
        [HttpGet]
        public async Task<IActionResult> MigrationProfiles()
        {
            List<Profile> profiles = new();

            string departmentName;
            string departmentCode;

            var config = Configuration.Default.WithDefaultLoader();
            var address = "http://dgu.ru/sveden/OOP_DGU";
            var context = BrowsingContext.New(config);
            var document = await context.OpenAsync(address);
            var body = document.Body;
            body.InnerHtml = body.InnerHtml.Replace("<strong>", "");
            body.InnerHtml = body.InnerHtml.Replace("</strong>", "");
            body.InnerHtml = body.InnerHtml.Replace("&nbsp;", "");
            Profile profileDb;
            foreach (var profile in body.InnerHtml.Split("<tr itemprop=\"eduOp\">").Skip(3))
            {
                try
                {
                    string profileId = "";
                    var year = profile.Split("<td colspan=\"5\">")[1].Split("</td>")[0];
                    if (year == "")
                    {
                        year = "0";
                    }

                    var levelEdu = await _searchEntityService.SearchLevelEdu(profile.Split("<td itemprop=\"eduLevel\">")[1].Split("</td>")[0]);

                    departmentCode = profile.Split("<td itemprop=\"eduCode\">")[1].Split("</td>")[0];
                    departmentName = profile.Split("<td itemprop=\"eduName\">")[1].Split("</td>")[0];

                    var profileName = profile.Split("<td itemprop=\"eduProf\">")[1].Split("</td>")[0];

                    var caseSDepartment = _eORContext.Profiles.FirstOrDefault(x => x.Title == profileName)?.DeptId ?? await _searchEntityService.SearchCaseSDepartment(departmentName);

                    profileDb = new()
                    {
                        CaseCEdukindId = await _searchEntityService.SearchEdukind(profile.Split("<td itemprop=\"eduForm\">")[1].Split("</td>")[0]),
                        Year = int.Parse(year),
                        TermEdu = profile.Split("<td>")[1].Split("</td>")[0],
                        EducationLanguage = profile.Split("<td colspan=\"2\">")[1].Split("</td>")[0],
                        LevelEduId = levelEdu.Id,
                        CaseSDepartmentId = caseSDepartment,
                        ProfileName = profileName,
                    };

                    if (levelEdu.Name == "Аспирантура")
                        IsPostGraduate(profile, profileDb);
                    else
                        profileId = IsOtherGraduate(profile, profileDb, profileId);

                    if (profileId != "")
                    {
                        profileDb.EorId = int.Parse(profileId);
                    }

                    #region fileModels
                    if (profile.Split("<td colspan=\"3\"><a href=\"").Length > 1)
                    {
                        var fgos = profile.Split("<td colspan=\"3\"><a href=\"")[1].Split("\">")[0];
                        var fgosSplitSlash = fgos.Split("/");
                        var fgosName = profile.Split("<td colspan=\"3\"><a href=\"")[1].Split("\">")[1].Split("</a>")[0];
                        var typeFgos = await _searchEntityService.SearchFileType("ФГОС");

                        profileDb.FileModels.Add(new FileModel()
                        {
                            LinkToFile = fgos,
                            Name = fgosSplitSlash[^1],
                            OutputFileName = fgosName,
                            FileTypeId = typeFgos.Id
                        });
                    }

                    if (profile.Split("<td colspan=\"3\" itemprop=\"opMain\"><a href=\"").Length > 1)
                    {
                        var opop = profile.Split("<td colspan=\"3\" itemprop=\"opMain\"><a href=\"")[1].Split("\">")[0];
                        var opopSplitSlash = opop.Split("/");
                        var opopName = profile.Split("<td colspan=\"3\" itemprop=\"opMain\"><a href=\"")[1].Split("\">")[1].Split("</a>")[0];
                        var opopType = await _searchEntityService.SearchFileType("ОПОП");

                        profileDb.FileModels.Add(new FileModel()
                        {
                            LinkToFile = opop,
                            Name = opopSplitSlash[^1],
                            OutputFileName = opopName,
                            FileTypeId = opopType.Id
                        });
                    }

                    if (profile.Split("<td itemprop=\"educationPlan\"><a href=\"").Length > 1)
                    {
                        var eduPlan = profile.Split("<td itemprop=\"educationPlan\"><a href=\"")[1].Split("\">")[0];
                        var eduPlanSplitSlash = eduPlan.Split("/");
                        var eduPlanName = profile.Split("<td itemprop=\"educationPlan\"><a href=\"")[1].Split("\">")[1].Split("</a>")[0];
                        var eduPlanType = await _searchEntityService.SearchFileType("Учебный план");

                        profileDb.FileModels.Add(new FileModel()
                        {
                            LinkToFile = eduPlan,
                            Name = eduPlanSplitSlash[^1],
                            OutputFileName = eduPlanName,
                            FileTypeId = eduPlanType.Id
                        });
                    }

                    if (profile.Split("<td colspan=\"2\" itemprop=\"educationShedule\"><a href=\"").Length > 1)
                    {
                        var grafik = profile.Split("<td colspan=\"2\" itemprop=\"educationShedule\"><a href=\"")[1].Split("\">")[0];
                        var grafikSplitSlash = grafik.Split("/");
                        var grafikName = profile.Split("<td colspan=\"2\" itemprop=\"educationShedule\"><a href=\"")[1].Split("\">")[1].Split("</a>")[0];
                        var grafikType = await _searchEntityService.SearchFileType("Календарный график");

                        profileDb.FileModels.Add(new FileModel()
                        {
                            LinkToFile = grafik,
                            Name = grafikSplitSlash[^1],
                            OutputFileName = grafikName,
                            FileTypeId = grafikType.Id
                        });
                    }

                    if (profile.Split("<td colspan=\"2\"><a href=\"").Length > 1)
                    {
                        var gia = profile.Split("<td colspan=\"2\"><a href=\"")[1].Split("\">")[0];
                        var giaSplitSlash = gia.Split("/");
                        var giaName = profile.Split("<td colspan=\"2\"><a href=\"")[1].Split("\">")[1].Split("</a>")[0];
                        var giaType = await _searchEntityService.SearchFileType("Программа ГИА");

                        profileDb.FileModels.Add(new FileModel()
                        {
                            LinkToFile = gia,
                            Name = giaSplitSlash[^1],
                            OutputFileName = giaName,
                            FileTypeId = giaType.Id
                        });
                    }

                    if (profile.Split("<td colspan=\"3\"><a href=\"/sveden/Content/files/Matriza").Length > 1)
                    {
                        var competentionMatrix = profile.Split("<td colspan=\"3\"><a href=\"/sveden/Content/files/Matriza")[1].Split("\">")[0];
                        var compMatrix = "/sveden/Content/files/Matriza" + competentionMatrix;
                        var competentionMatrixSplitSlash = compMatrix.Split("/");
                        var competentionMatrixName = profile.Split("<td colspan=\"3\"><a href=\"/sveden/Content/files/Matriza")[1].Split("\">")[1].Split("</a>")[0];
                        var competentionMatrixType = await _searchEntityService.SearchFileType("Матрицы компетенций");

                        profileDb.FileModels.Add(new FileModel()
                        {
                            LinkToFile = compMatrix,
                            Name = competentionMatrixSplitSlash[^1],
                            OutputFileName = competentionMatrixName,
                            FileTypeId = competentionMatrixType.Id
                        });
                    }

                    if (profile.Split("<td colspan=\"3\"><a href=\"/sveden/Content/files/AOPOP").Length > 1)
                    {
                        var aopopPre = profile.Split("<td colspan=\"3\"><a href=\"/sveden/Content/files/AOPOP")[1].Split("\">")[0];
                        var aopop = "/sveden/Content/files/AOPOP" + aopopPre;
                        var aopopSplitSlash = aopop.Split("/");
                        var aopopName = profile.Split("<td colspan=\"3\"><a href=\"/sveden/Content/files/AOPOP")[1].Split("\">")[1].Split("</a>")[0];
                        var aopopType = await _searchEntityService.SearchFileType("АОПОП");

                        profileDb.FileModels.Add(new FileModel()
                        {
                            LinkToFile = aopop,
                            Name = aopopSplitSlash[^1],
                            OutputFileName = aopopName,
                            FileTypeId = aopopType.Id
                        });
                    }

                    if (profile.Split("<td><a href=\"/sveden/Content/files/RPV").Length > 1)
                    {
                        var rpvPre = profile.Split("<td><a href=\"/sveden/Content/files/RPV")[1].Split("\">")[0];
                        var rpv = "/sveden/Content/files/RPV" + rpvPre;
                        var rpvSplitSlash = rpv.Split("/");
                        var rpvName = profile.Split("<td><a href=\"/sveden/Content/files/RPV")[1].Split("\">")[1].Split("</a>")[0];
                        var rpvType = await _searchEntityService.SearchFileType("РПВ");

                        profileDb.FileModels.Add(new FileModel()
                        {
                            LinkToFile = rpv,
                            Name = rpvSplitSlash[^1],
                            OutputFileName = rpvName,
                            FileTypeId = rpvType.Id
                        });
                    }

                    if (profile.Split("<td colspan=\"5\" itemprop=\"methodology\"><a href=\"").Length > 1)
                    {
                        var metodMaterials = profile.Split("<td colspan=\"5\" itemprop=\"methodology\"><a href=\"")[1].Split("\" ")[0];
                        var metodMaterialsSplitSlash = metodMaterials.Split("/");
                        var metodMaterialsName = profile.Split("<td colspan=\"5\" itemprop=\"methodology\"><a href=\"")[1].Split("\">")[1].Split("</a>")[0];
                        var metodMaterialsType = await _searchEntityService.SearchFileType("Методические материалы для обеспечения ОП");

                        profileDb.FileModels.Add(new FileModel()
                        {
                            LinkToFile = metodMaterials,
                            Name = metodMaterialsSplitSlash[^1],
                            OutputFileName = metodMaterialsName,
                            FileTypeId = metodMaterialsType.Id
                        });
                    }
                    #endregion

                    profiles.Add(profileDb);
                }
                catch (Exception ex)
                {
                    throw;
                }
            }
            await _profileRepository.CreateRange(profiles);
            return Ok();
        }

        private static void IsPostGraduate(string profile, Profile profileDb)
        {
            if (profile.Split("<td itemprop=\"educationRpd\"><a href=\"").Length > 1)
            {
                profileDb.LinkToRPD = profile.Split("<td itemprop=\"educationRpd\"><a href=\"")[1].Split("\">")[0];
            }
            else
            {
                if (profile.Split("<td colspan=\"2\" itemprop=\"educationRpd\" rowspan=\"2\"><a href=\"").Length > 1)
                {
                    profileDb.LinkToRPD = profile.Split("<td colspan=\"2\" itemprop=\"educationRpd\" rowspan=\"2\"><a href=\"")[1].Split("\">")[0];
                }
                else
                {
                    if (profile.Split("<td colspan=\"2\" itemprop=\"educationRpd\"><a href=\"").Length > 1)
                    {
                        profileDb.LinkToRPD = profile.Split("<td colspan=\"2\" itemprop=\"educationRpd\"><a href=\"")[1].Split("\">")[0];
                    }

                    else
                    {
                        if (profile.Split("<td colspan=\"2\" itemprop=\"educationRpd\">").Length > 1)
                        {
                            if (profile.Split("<td colspan=\"2\" itemprop=\"educationRpd\">")[1].Split("</td>")[0] == "")
                            {
                                profileDb.LinkToRPD = null;
                            }
                        }
                        else
                        {
                            if (profile.Split("<td itemprop=\"educationRpd\">").Length > 1)
                            {
                                if (profile.Split("<td itemprop=\"educationRpd\">")[1].Split("</td>")[0] == "")
                                {
                                    profileDb.LinkToRPD = null;
                                }
                            }
                            else
                            {
                                if (profile.Split("<td colspan=\"2\" itemprop=\"educationRpd\" rowspan=\"2\"><a href=\"").Length > 1)
                                    profileDb.LinkToRPD = profile.Split("<td colspan=\"2\" itemprop=\"educationRpd\" rowspan=\"2\"><a href=\"")[1].Split("\">")[0];
                                else
                                {
                                    if (profile.Split("<td itemprop=\"educationRpd\"><a href=\"").Length > 1)
                                        profileDb.LinkToRPD = profile.Split("<td itemprop=\"educationRpd\"><a href=\"")[1].Split("\">")[0];
                                }
                            }
                        }
                    }
                }
            }
        }

        private static string IsOtherGraduate(string profile, Profile profileDb, string profileId)
        {
            if (profile.Split("itemprop=\"educationRpd\"").Length > 1)
            {
                if (profile.Split("profileId=").Length > 1)
                {
                    profileId = profile.Split("profileId=")[1].Split("\">")[0];
                }
                else
                {
                    if (profile.Split("<td colspan=\"2\" itemprop=\"educationRpd\">").Length > 1)
                    {
                        if (profile.Split("<td colspan=\"2\" itemprop=\"educationRpd\">")[1].Split("</td>")[0] == "")
                            profileDb.LinkToRPD = null;
                        else
                        {
                            if (profile.Split("<td colspan=\"2\" itemprop=\"educationRpd\"><a href=\"").Length > 1)
                                profileId = profile.Split("<td colspan=\"2\" itemprop=\"educationRpd\"><a href=\"")[1].Split("\">")[0].Split("/")[^1];
                            else
                                profileId = profile.Split("<td itemprop=\"educationRpd\"><a href=\"")[1].Split("\">")[0].Split("/")[^1];
                        }
                    }
                    else
                    {
                        if (profile.Split("<td colspan=\"2\" itemprop=\"educationRpd\"><a href=\"").Length > 1)
                            profileId = profile.Split("<td colspan=\"2\" itemprop=\"educationRpd\"><a href=\"")[1].Split("\">")[0].Split("/")[^1];
                        else
                            profileId = profile.Split("<td itemprop=\"educationRpd\"><a href=\"")[1].Split("\">")[0].Split("/")[^1];
                    }
                }
            }
            return profileId;
        }


        [Route("MigrationDisciplines")]
        [HttpGet]
        public async Task<IActionResult> MigrationDisciplines()
        {
            var profilesFromEor = await _eORContext.Profiles.Include(x => x.Disciplines).ThenInclude(s => s.Educators)
                                                      .Include(x => x.Disciplines).ThenInclude(c => c.Status).ToListAsync();
            var profilesFromDb = _profileRepository.Get();

            var disciplinesFromDb = new List<DomainServices.Entities.Discipline>();

            var statusDisciplineFromEor = _eORContext.Statuses.ToList();

            List<StatusDiscipline> statusDisciplines = new();

            foreach (var item in statusDisciplineFromEor)
            {
                statusDisciplines.Add(new StatusDiscipline()
                {
                    Name = item.Name,
                });
            }
            await _statusDisciplineRepository.CreateRange(statusDisciplines);

            List<Profile> profiles = new();
            int count = 0;
            try
            {
                foreach (var item in profilesFromEor)
                {
                    count++;
                    profiles = await profilesFromDb.Where(x => x.EorId == item.Id).ToListAsync();
                    foreach (var profile in profiles)
                    {
                        foreach (var discipline in item.Disciplines)
                        {
                            disciplinesFromDb.Add(new DomainServices.Entities.Discipline()
                            {
                                DisciplineName = discipline.DisciplineName,
                                Code = discipline.Status.Abr,
                                ProfileId = profile.Id,
                                FileRPD = new FileRPD()
                                {
                                    Name = discipline.Link,
                                    PersonId = discipline.Educators.Count == 0 ? null : discipline.Educators.First().PersonId
                                },
                                StatusDisciplineId = discipline.StatusId
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            await _disciplineRepository.CreateRange(disciplinesFromDb);
            return Ok();
        }
    }
}
