using DSUContextDBService.Models;

namespace DSUContextDBService.Interfaces
{
    public interface IDSUActiveData
    {
        public IQueryable<CaseSStudent> GetCaseSStudents();
        public CaseSStudent GetCaseSStudentById(int id);
        public IQueryable<CaseSDepartment> GetCaseSDepartments();
        public CaseSDepartment GetCaseSDepartmentById(int id);
        public IQueryable<CaseSDepartment> GetCaseSDepartmentByFacultyId(int? id);
        public IQueryable<CaseSSpecialization> GetCaseSSpecializations();
        public CaseSSpecialization GetCaseSSpecializationById(int id);
        public IQueryable<CaseSTeacher> GetCaseSTeachers();
        public CaseSTeacher GetCaseSTeacherById(int id);
        public IQueryable<CaseCStatus> GetCaseCStatus();
        public CaseCStatus GetCaseCStatusById(int id);
        public IQueryable<CaseCEdue> GetCaseCEdues();
        public CaseCEdue GetCaseCEdueById(int id);
        public IQueryable<CaseCEdukind> GetCaseCEdukinds();
        public CaseCEdukind GetCaseCEdukindById(int id);
        public IQueryable<CaseCPlat> GetCaseCPlats();
        public CaseCPlat GetCaseCPlatById(int id);
        public IQueryable<CaseUkoExam> GetCaseUkoExams();
        public CaseUkoExam GetCaseUkoExamById(int id);
        public IQueryable<CaseUkoModule> GetCaseUkoModules();
        public CaseUkoModule GetCaseUkoModuleById(int id);
        public IQueryable<CaseUkoZachet> GetCaseUkoZachets();
        public CaseUkoZachet GetCaseUkoZachetById(int id);
    }
}
