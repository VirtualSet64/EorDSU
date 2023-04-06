using DSUContextDBService.Interfaces;
using Ifrastructure.Common;
using DomainServices.Entities;
using Ifrastructure.Repository.InterfaceRepository;
using DomainServices.DtoModels;
using Microsoft.EntityFrameworkCore;
using DomainServices.DBService;

namespace Ifrastructure.Repository
{
    public class DisciplineRepository : GenericRepository<Discipline>, IDisciplineRepository
    {
        private readonly IDSUActiveData _dSUActiveData;
        private readonly IProfileRepository _profileRepository;
        public DisciplineRepository(ApplicationContext dbContext, IDSUActiveData dSUActiveData, IProfileRepository profileRepository) : base(dbContext)
        {
            _dSUActiveData = dSUActiveData;
            _profileRepository = profileRepository;
        }

        public DataForTableResponse GetDisciplinesByProfileId(int profileId)
        {
            var profile = Get().Include(x => x.Profile).Include(x => x.FileRPD).FirstOrDefault(x => x.ProfileId == profileId)?.Profile;
            List<Discipline> disciplines = GetWithInclude(x => x.ProfileId == profileId, x => x.StatusDiscipline, x => x.FileRPD).ToList();
            DataForTableResponse responseForDiscipline = new()
            {
                Disciplines = disciplines,
                Profile = profile,
                CaseCEdukind = profile?.CaseCEdukindId == null ? null : _dSUActiveData.GetCaseCEdukindById((int)profile.CaseCEdukindId),
                CaseSDepartment = profile?.CaseSDepartmentId == null ? null : _dSUActiveData.GetCaseSDepartmentById((int)profile.CaseSDepartmentId)
            };
            return responseForDiscipline;
        }

        //public async Task<List<Discipline>> GetRemovableDiscipline(int facultyId)
        //{
        //    var profiles = await _profileRepository.GetProfileByFacultyId(facultyId);

        //    var disciplines = Get().Where(c => c.IsDeletionRequest).ToList();
        //    List<Discipline> removableDisciplines = new();
        //    foreach (var item in profiles)
        //    {
        //        removableDisciplines.AddRange(disciplines.Where(x => item.Id == x.ProfileId));
        //    }
        //    return removableDisciplines;
        //}

        public Discipline GetDisciplinesById(int id)
        {
            return GetWithIncludeById(x => x.Id == id, x => x.StatusDiscipline, x => x.FileRPD);
        }

        public async Task<Discipline> RemoveDiscipline(int id)
        {
            var discipline = GetWithIncludeById(x => x.Id == id, x => x.FileRPD, x => x.StatusDiscipline, x => x.Profile);
            await Remove(discipline);
            return discipline;
        }

        public async Task<Discipline> RequestDeleteDiscipline(int id)
        {
            var discipline = FindById(id);
            discipline.IsDeletionRequest = true;
            await Update(discipline);
            return discipline;
        }
    }
}
