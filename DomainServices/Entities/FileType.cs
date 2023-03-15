namespace DomainServices.Entities
{
    public class FileType
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public bool IsDeleted { get; set; }

        //FGOS,//ФГОС
        //OPOP,//ОПОП
        //EduPlan,//Учебный план
        //KalendarEduPlan,//Календарный учебный график
        //GIA,//Программа ГИА
        //PriemResult,//Результаты приема
        //MatrixCompetition,//Матрицы компетенций
        //AOPOP,//АОПОП
        //DistantEdu,//Дистанционное обучение
        //RPV,//РП восп.работы
        //RPP,//Рабочие программы практик
        //MetodMaterialForObespecOP,//Методические материалы для обеспечения ОП
    }
}
