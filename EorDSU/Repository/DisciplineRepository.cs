using EorDSU.Common;
using EorDSU.Common.Interfaces;
using EorDSU.Models;
using EorDSU.Repository.InterfaceRepository;
using EorDSU.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace EorDSU.Repository
{
    public class DisciplineRepository : GenericRepository<Discipline>, IDisciplineRepository
    {
        private readonly IUnitOfWork _unitOfWork;
        public DisciplineRepository(DbContext dbContext, UnitOfWork unitOfWork) : base(dbContext) 
        {
            _unitOfWork = unitOfWork;        
        }

        public ResponseForDiscipline GetDisciplinesByProfileId(int profileId)
        {
            var profile = Get().Include(x => x.Profile).Include(x => x.FileRPD).FirstOrDefault(x => x.ProfileId == profileId)?.Profile;
            List<Discipline> disciplines = GetWithInclude(x => x.ProfileId == profileId, x => x.StatusDiscipline, x => x.FileRPD).ToList();
            ResponseForDiscipline responseForDiscipline = new()
            {
                Disciplines = disciplines,
                Profile = profile,
                CaseCEdukind = profile?.CaseCEdukindId == null ? null : _unitOfWork.DSUActiveData.GetCaseCEdukindById((int)profile.CaseCEdukindId),
                CaseSDepartment = profile?.CaseSDepartmentId == null ? null : _unitOfWork.DSUActiveData.GetCaseSDepartmentById((int)profile.CaseSDepartmentId)
            };
            return responseForDiscipline;
        }

        public async Task<List<Discipline>> GetRemovableDiscipline(int facultyId)
        {
            var sd = await _unitOfWork.ProfileRepository.GetDataByFacultyId(facultyId);

            var disciplines = Get().Where(c => c.IsDeletionRequest).ToList();
            List<Discipline> removableDisciplines = new();
            foreach (var item in sd)
            {
                var r = disciplines.Where(x => item.Profile.Id == x.ProfileId);
                removableDisciplines.AddRange(r);
            }
            return removableDisciplines;
        }

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
