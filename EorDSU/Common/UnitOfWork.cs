using BasePersonDBService.DataContext;
using BasePersonDBService.Interfaces;
using BasePersonDBService.Services;
using DSUContextDBService.DataContext;
using DSUContextDBService.Interfaces;
using DSUContextDBService.Services;
using EorDSU.Common.Interfaces;
using EorDSU.DBService;
using EorDSU.Interface;
using EorDSU.Models;
using EorDSU.Repository;
using EorDSU.Repository.InterfaceRepository;
using EorDSU.Service;

namespace EorDSU.Common
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationContext _context;
        private readonly BASEPERSONMDFContext _bASEPERSONMDFContext;
        private readonly DSUContext _dSUContext;
        private readonly IWebHostEnvironment _appEnvironment;
        private readonly IConfiguration Configuration;
        private IBasePersonActiveData _basePersonActiveData;
        private IDSUActiveData _dSUActiveData;
        private IExcelParsingService _excelParsingService;
        private ISearchEntity _searchEntity;

        private IProfileRepository profileRepository;
        private IDisciplineRepository disciplineRepository;
        private IFileModelRepository fileModelRepository;
        private IFileRPDRepository fileRPDRepository;

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
                _excelParsingService ??= new ExcelParsingService(this);
                return _excelParsingService;
            }
        }

        public ISearchEntity SearchEntity
        {
            get
            {
                _searchEntity ??= new SearchEntityService(_context, this);
                return _searchEntity;
            }
        }

        public IDSUActiveData DSUActiveData
        {
            get
            {
                _dSUActiveData ??= new DSUActiveData(_dSUContext);
                return _dSUActiveData;
            }
        }

        public IBasePersonActiveData BasePersonActiveData
        {
            get
            {
                _basePersonActiveData ??= new BasePersonActiveData(_bASEPERSONMDFContext);
                return _basePersonActiveData;
            }
        }

        IProfileRepository IUnitOfWork.ProfileRepository
        {
            get
            {
                profileRepository ??= new ProfileRepository(_context, this, Configuration);
                return profileRepository;
            }
        }

        IDisciplineRepository IUnitOfWork.DisciplineRepository
        {
            get
            {
                disciplineRepository ??= new DisciplineRepository(_context);
                return disciplineRepository;
            }
        }

        IFileModelRepository IUnitOfWork.FileModelRepository
        {
            get
            {
                fileModelRepository ??= new FileModelRepository(_context, _appEnvironment, Configuration, this);
                return fileModelRepository;
            }
        }

        IFileRPDRepository IUnitOfWork.FileRPDRepository
        {
            get
            {
                fileRPDRepository ??= new FileRPDRepository(_context, _appEnvironment, Configuration);
                return fileRPDRepository;
            }
        }
    }
}
