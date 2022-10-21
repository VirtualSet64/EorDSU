using EorDSU.DBService;
using EorDSU.Models;
using EorDSU.Repository;
using IronPdf;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Sentry;

namespace EorDSU.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ParsingController : Controller
    {
        private readonly ActiveDataRepository _activeDataRepository;
        public ParsingController(ActiveDataRepository activeDataRepository)
        {
            _activeDataRepository = activeDataRepository;
        }

        [HttpGet]
        public List<PersDivision> Index()
        {
            return _activeDataRepository.GetPersDivisions();
        }

        //[HttpGet]
        //public string Index()
        //{
        //    try
        //    {
        //        using PdfDocument PDF = PdfDocument.FromFile("C://Users/programmsit/Downloads/socialInformatic.pdf");
        //        //Using ExtractAllText() method, extract every single text from an pdf
        //        string AllText = PDF.ExtractAllText();
        //        bool tempBoolForDepartment = false;
        //        bool tempBoolForObrazovProg = false;
        //        bool tempBoolForProfile = false;
        //        bool tempBoolForLevelEdu = false;
        //        bool tempBoolForFormEdu = false;
        //        string department = "";
        //        string cathedra = "";
        //        string obrazovProg = "";
        //        string profile = "";
        //        string levelEdu = "";
        //        string formEdu = "";
        //        string statusDisc = "";
        //        string year = "";
        //        AllText = AllText.Replace("\r", "");
        //        foreach (var text in AllText.Split('\n'))
        //        {
        //            if (tempBoolForDepartment)
        //            {
        //                department = text;
        //                tempBoolForDepartment = false;
        //            }
        //            if (text == "РАБОЧАЯ ПРОГРАММА ДИСЦИПЛИНЫ")
        //            {
        //                tempBoolForDepartment = true;
        //            }

        //            if (text.Split(" ")[0] == "Кафедра")
        //            {
        //                cathedra = text;
        //            }

        //            if (tempBoolForObrazovProg)
        //            {
        //                obrazovProg = text;
        //                tempBoolForObrazovProg = false;
        //            }
        //            if (text == "Образовательная программа")
        //            {
        //                tempBoolForObrazovProg = true;
        //            }
        //            else if (text.Split(":")[0] == "Образовательная программа")
        //            {
        //                obrazovProg = text.Split(":")[1];
        //            }

        //            if (tempBoolForLevelEdu)
        //            {
        //                levelEdu = text;
        //                tempBoolForLevelEdu = false;
        //            }

        //            if (text == "Уровень высшего образования")
        //            {
        //                tempBoolForProfile = false;
        //                tempBoolForLevelEdu = true;
        //            }
        //            else if (text.Split(":")[0] == "Уровень высшего образования")
        //            {
        //                levelEdu = text.Split(":")[1];
        //                tempBoolForProfile = false;
        //            }

        //            if (tempBoolForProfile)
        //            {
        //                profile += text + "\r";
        //            }
        //            if (text == "Профиль подготовки" || text == "Профили подготовки: ")
        //            {
        //                tempBoolForProfile = true;
        //            }

        //            if (tempBoolForFormEdu)
        //            {
        //                formEdu = text;
        //                tempBoolForFormEdu = false;
        //            }
        //            if (text == "Форма обучения")
        //            {
        //                tempBoolForFormEdu = true;
        //            }
        //            else if (text.Split(":")[0] == "Форма обучения")
        //            {
        //                formEdu = text.Split(":")[1];
        //            }

        //            if (text.Split(":")[0] == "Статус дисциплины")
        //            {
        //                statusDisc = text.Split(":")[1];
        //            }

        //            if (text.Split(",")[0] == "Махачкала")
        //            {
        //                year = text.Split(",")[1];
        //            }
        //            else if (text.Split(" ")[0] == "Махачкала")
        //            {
        //                year = text.Split(" ")[1];
        //            }
        //        }
        //        //View text in an Label or textbox
        //        return AllText;
        //    }
        //    catch (Exception ex)
        //    {
        //        SentrySdk.CaptureException(ex);
        //        throw;
        //    }
        //}
    }
}
