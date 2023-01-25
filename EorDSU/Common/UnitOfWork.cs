using BasePersonDBService.DataContext;
using BasePersonDBService.Interfaces;
using BasePersonDBService.Services;
using DSUContextDBService.DataContext;
using DSUContextDBService.Interfaces;
using DSUContextDBService.Services;
using EorDSU.Common.Interfaces;
using EorDSU.DBService;
using EorDSU.Interface;
using EorDSU.Repository;
using EorDSU.Repository.InterfaceRepository;
using EorDSU.Service;
using EorDSU.Services.Interface;

namespace EorDSU.Common
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationContext _context;
        private readonly BASEPERSONMDFContext _bASEPERSONMDFContext;
        private readonly DSUContext _dSUContext;
        private readonly IWebHostEnvironment _appEnvironment;
        private readonly IConfiguration Configuration;

        public UnitOfWork(ApplicationContext context, IWebHostEnvironment appEnvironment, IConfiguration configuration, BASEPERSONMDFContext bASEPERSONMDFContext, 
            DSUContext dSUContext)
        {
            _context = context;
            _bASEPERSONMDFContext = bASEPERSONMDFContext;
            _dSUContext = dSUContext;
            _appEnvironment = appEnvironment;
            Configuration = configuration;            
        }

        public IExcelParsingService ExcelParsingService
        {
            get
            {
                IExcelParsingService _excelParsingService = new ExcelParsingService(this);
                return _excelParsingService;
            }
        }

        public ISearchEntity SearchEntity
        {
            get
            {
                ISearchEntity _searchEntity = new SearchEntityService(_context, this);
                return _searchEntity;
            }
        }

        public IDSUActiveData DSUActiveData
        {
            get
            {
                IDSUActiveData _dSUActiveData = new DSUActiveData(_dSUContext);
                return _dSUActiveData;
            }
        }

        public IBasePersonActiveData BasePersonActiveData
        {
            get
            {
                IBasePersonActiveData _basePersonActiveData = new BasePersonActiveData(_bASEPERSONMDFContext);
                return _basePersonActiveData;
            }
        }

        IProfileRepository IUnitOfWork.ProfileRepository
        {
            get
            {
                IProfileRepository profileRepository = new ProfileRepository(_context, this, Configuration, _appEnvironment);
                return profileRepository;
            }
        }

        IDisciplineRepository IUnitOfWork.DisciplineRepository
        {
            get
            {
                IDisciplineRepository disciplineRepository = new DisciplineRepository(_context);
                return disciplineRepository;
            }
        }

        IFileModelRepository IUnitOfWork.FileModelRepository
        {
            get
            {
                IFileModelRepository fileModelRepository = new FileModelRepository(_context, _appEnvironment, Configuration);
                return fileModelRepository;
            }
        }

        IFileRPDRepository IUnitOfWork.FileRPDRepository
        {
            get
            {
                IFileRPDRepository fileRPDRepository = new FileRPDRepository(_context, _appEnvironment, Configuration);
                return fileRPDRepository;
            }
        }

        IStatusDisciplineRepository IUnitOfWork.StatusDisciplineRepository
        {
            get
            {
                IStatusDisciplineRepository statusDisciplineRepository = new StatusDisciplineRepository(_context);
                return statusDisciplineRepository;
            }
        }
    }
}
