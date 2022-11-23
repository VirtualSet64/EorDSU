using EorDSU.Models;

namespace EorDSU.ResponseModel
{
    public class AddEduPlanResponse
    {
        public Profile? Profile { get; set; }
        public LevelEdu? LevelEdu { get; set; }
        public string? Code { get; set; }
        public CaseCEdukind? caseCEdukind { get; set; }
        public List<Discipline>? Disciplines { get; set; }
        public CaseCEdukind? CaseCEdukind { get; set; }
    }
}
