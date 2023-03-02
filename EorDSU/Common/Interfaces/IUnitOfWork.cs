using BasePersonDBService.Interfaces;
using DSUContextDBService.Interfaces;
using EorDSU.Interface;
using EorDSU.Repository.InterfaceRepository;
using EorDSU.Services.Interface;

namespace EorDSU.Common.Interfaces
{
    public interface IUnitOfWork
    {
        IProfileRepository ProfileRepository { get; }
        IDisciplineRepository DisciplineRepository { get; }
        IFileTypeRepository FileTypeRepository { get; }
        IFileModelRepository FileModelRepository { get; }
        IFileRPDRepository FileRPDRepository { get; }
        IStatusDisciplineRepository StatusDisciplineRepository { get; }
        ILevelEduRepository LevelEduRepository{ get; }
        IUmuAndFacultyRepository UmuAndFacultyRepository{ get; }
        IExcelParsingService ExcelParsingService { get; }
        ISearchEntity SearchEntity { get; }
        IDSUActiveData DSUActiveData { get; }
        IBasePersonActiveData BasePersonActiveData { get; }
    }
}
