﻿using EorDSU.DBService;
using EorDSU.Interface;
using EorDSU.Models;
using EorDSU.Repository;
using EorDSU.Service;
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
        private readonly ISearchEntity _searchEntity;
        public ParsingController(ISearchEntity searchEntity)
        {
            _searchEntity = searchEntity;
        }

        [HttpGet]
        public RPD ParsingRPD()
        {
            try
            {
                using PdfDocument PDF = PdfDocument.FromFile("C://Users/programmsit/Downloads/ibv.pdf");
                //Using ExtractAllText() method, extract every single text from an pdf
                string AllText = PDF.ExtractAllText();
                RPD rpd = new()
                {
                    ListProfile = new List<Profile>()
                };
                bool tempBoolForDepartment = false;
                bool tempBoolForObrazovProg = false;
                bool tempBoolForProfile = false;
                bool tempBoolForLevelEdu = false;
                bool tempBoolForFormEdu = false;

                string statusDisc = "";
                AllText = AllText.Replace("\r", "");
                foreach (var text in AllText.Split('\n'))
                {
                    if (tempBoolForDepartment)
                    {
                        rpd.Discipline = _searchEntity.SearchDiscipline(text);
                        tempBoolForDepartment = false;
                    }
                    if (text == "РАБОЧАЯ ПРОГРАММА ДИСЦИПЛИНЫ")
                    {
                        tempBoolForDepartment = true;
                    }

                    if (tempBoolForObrazovProg)
                    {
                        rpd.PersDepartment = _searchEntity.SearchPersDepartment(text);
                        tempBoolForObrazovProg = false;
                    }
                    if (text == "Образовательная программа")
                    {
                        tempBoolForObrazovProg = true;
                    }
                    else if (text.Split(":")[0] == "Образовательная программа")
                    {
                        rpd.PersDepartment = _searchEntity.SearchPersDepartment(text.Split(":")[1]);
                    }

                    if (tempBoolForLevelEdu)
                    {
                        rpd.LevelEdu = _searchEntity.SearchLevelEdu(text);
                        tempBoolForLevelEdu = false;
                    }

                    if (text == "Уровень высшего образования")
                    {
                        tempBoolForProfile = false;
                        tempBoolForLevelEdu = true;
                    }
                    else if (text.Split(":")[0] == "Уровень высшего образования")
                    {
                        rpd.LevelEdu = _searchEntity.SearchLevelEdu(text.Split(":")[1]);
                        tempBoolForProfile = false;
                    }

                    if (tempBoolForProfile)
                    {
                        rpd.ListProfile.Add(_searchEntity.SearchProfile(text));
                    }
                    if (text == "Профиль подготовки" || text == "Профили подготовки: " || text == "Профиль подготовки:")
                    {
                        tempBoolForProfile = true;
                    }

                    if (tempBoolForFormEdu)
                    {
                        rpd.Edukind = _searchEntity.SearchEdukind(text);
                        tempBoolForFormEdu = false;
                    }
                    if (text == "Форма обучения")
                    {
                        tempBoolForFormEdu = true;
                    }
                    else if (text.Split(":")[0] == "Форма обучения")
                    {
                        rpd.Edukind = _searchEntity.SearchEdukind(text.Split(":")[1]);
                    }

                    if (text.Split(":")[0] == "Статус дисциплины")
                    {
                        statusDisc = text.Split(":")[1];
                    }

                    if (text.Split(",")[0] == "Махачкала")
                    {
                        rpd.Year = text.Split(",")[1];
                    }
                    else if (text.Split(" ")[0] == "Махачкала")
                    {
                        rpd.Year = text.Split(",")[1];
                    }
                }
                //View text in an Label or textbox
                return rpd;
            }
            catch (Exception ex)
            {
                SentrySdk.CaptureException(ex);
                throw;
            }
        }
    }
}
