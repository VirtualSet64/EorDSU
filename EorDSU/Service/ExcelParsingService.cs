using EorDSU.DBService;
using EorDSU.Interface;
using EorDSU.Models;
using EorDSU.ResponseModel;
using System.Collections.Generic;
using Excel = Microsoft.Office.Interop.Excel;

namespace EorDSU.Service
{
    public class ExcelParsingService
    {
        private readonly ApplicationContext _applicationContext;
        private readonly DSUContext _dSUContext;
        private readonly ISearchEntity _searchEntity;
        public ExcelParsingService(ApplicationContext applicationContext, DSUContext dSUContext, ISearchEntity searchEntity)
        {
            _applicationContext = applicationContext;
            _dSUContext = dSUContext;
            _searchEntity = searchEntity;
        }

        public Profile ParsingService(string path)
        {
            Excel.Application ObjWorkExcel = new(); //открыть эксель
            Excel.Workbook ObjWorkBook = ObjWorkExcel.Workbooks.Open(@path, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing); //открыть файл

            Profile profile = new()
            {
                CaseSDepartment = new(),
                PersDepartment = new(),
                CaseCEdukind = new(),
                Disciplines = new(),
                FileModels = new(),
                LevelEdu = new()
            };
            TitulPage(ObjWorkBook, profile);
            PlanSvodPage(ObjWorkBook, profile);

            ObjWorkBook.Close(false, Type.Missing, Type.Missing); //закрыть не сохраняя
            ObjWorkExcel.Quit(); // выйти из экселя
            GC.Collect(); // убрать за собой

            var sd = _applicationContext.Profiles.ToList();
            if (!_applicationContext.Profiles.Any(x => x.ProfileName == profile.ProfileName))
            {
                _applicationContext.Profiles.Add(profile);
            }

            _applicationContext.SaveChanges();
            return profile;
        }

        private void TitulPage(Excel.Workbook ObjWorkBook, Profile profile)
        {
            Excel.Worksheet ObjWorkSheet = (Excel.Worksheet)ObjWorkBook.Sheets[1]; //получить 1 лист
            var lastCell = ObjWorkSheet.Cells.SpecialCells(Excel.XlCellType.xlCellTypeLastCell);//1 ячейку
            string[,] list = new string[lastCell.Column, lastCell.Row]; // массив значений с листа равен по размеру листу

            for (int i = 0; i < lastCell.Column; i++) //по всем колонкам
                for (int j = 0; j < lastCell.Row; j++) // по всем строкам

                    list[i, j] = ObjWorkSheet.Cells[j + 1, i + 1].Text.ToString();//считываем текст в строку

            //addEduPlanResponse.CaseCEdukind = _searchEntity.SearchEdukind(list[2, 41].Split(" ")[^1]);
            //[^1] = List[List.Lenght - 1]

            profile.ProfileName = list[3, 29];
            profile.TermEdu = int.Parse(list[2, 42].Split(" ")[^1][0].ToString());
            //profile.CaseSDepartmentId = _searchEntity.SearchCaseSDepartment(list[3, 28].Split(" ")[^1]).DepartmentId;
            //profile.PersDepartmentId = _searchEntity.SearchPersDepartment(list[3, 36]).DepId;
            profile.Year = int.Parse(list[22, 39]);

            var code = list[3, 26];

            profile.CaseSDepartment ??= _searchEntity.SearchCaseSDepartment(list[3, 28].Split(code)[^1].Trim());
            if (list[7, 24].Split(" ")[^1] == "Специалистов")
            {
                profile.LevelEdu = _searchEntity.SearchLevelEdu("специалитет");
            }
            if (list[7, 24].Split(" ")[^1] == "магистратуры")
            {
                profile.LevelEdu = _searchEntity.SearchLevelEdu("магистратура");
            }
            else
            {
                var tempLevelEdu = list[7, 24].Split(" ")[^1];
                profile.LevelEdu = _searchEntity.SearchLevelEdu(tempLevelEdu.Remove(tempLevelEdu.Length - 1));
            }

            if (profile.CaseSDepartmentId == null)
            {
                string v = list[3, 28].Split(code)[^1].Trim() + $" ({profile.LevelEdu.Name})";
                profile.CaseSDepartment = _searchEntity.SearchCaseSDepartment(v);
            }
        }

        private void PlanSvodPage(Excel.Workbook ObjWorkBook, Profile profile)
        {
            var ObjWorkSheet = (Excel.Worksheet)ObjWorkBook.Sheets[3]; //получить 3 лист
            var lastCell = ObjWorkSheet.Cells.SpecialCells(Excel.XlCellType.xlCellTypeLastCell);//1 ячейку
            var list = new string[lastCell.Column, lastCell.Row]; // массив значений с листа равен по размеру листу

            for (int i = 0; i < lastCell.Column; i++) //по всем колонкам
                for (int j = 0; j < lastCell.Row; j++) // по всем строкам
                {
                    list[i, j] = ObjWorkSheet.Cells[j + 1, i + 1].Text.ToString();//считываем текст в строку
                }

            profile.Disciplines = new();

            int mandatoryDisciplinesCount = 0;
            int unmandatoryDisciplinesCount = 0;
            int complexModulesCount = 0;
            int pacticMandatoryCount = 0;
            int pacticUnmandatoryCount = 0;
            int giaCount = 0;
            for (int i = 5; i < list.Length; i++)
            {
                if (list[0, i].Trim() == "Часть, формируемая участниками образовательных отношений" && mandatoryDisciplinesCount == 0)
                {
                    mandatoryDisciplinesCount = i;
                }
                if (list[0, i].Trim() == "К.М.Комплексные модули")
                {
                    unmandatoryDisciplinesCount = i;
                }
                if (list[0, i].Trim() == "Блок 2.Практика")
                {
                    complexModulesCount = i;
                }
                if (list[0, i].Trim() == "Часть, формируемая участниками образовательных отношений" && mandatoryDisciplinesCount != 0)
                {
                    pacticMandatoryCount = i;
                }
                if (list[0, i].Trim() == "Блок 3.Государственная итоговая аттестация")
                {
                    pacticUnmandatoryCount = i;
                }
                if (list[0, i].Trim() == "ФТД.Факультативы")
                {
                    giaCount = i;
                    break;
                }
            }

            for (int i = 5; i < mandatoryDisciplinesCount; i++)
            {
                bool isModule = false;
                for (int j = 0; j < list[2, i].Split(" ").Length; j++)
                {
                    if (list[2, i].Split(" ")[j] == "модуль" || list[2, i].Split(" ")[j] == "Модуль")
                    {
                        isModule = true;
                    }
                }
                if (isModule == false)
                {
                    Discipline discipline = new()
                    {
                        DisciplineName = list[2, i],
                        StatusDiscipline = _searchEntity.SearchStatusDiscipline("Обязательная часть "),
                        ProfileId = profile.Id,
                    };
                    profile.Disciplines.Add(discipline);
                }
            }
            for (int i = mandatoryDisciplinesCount + 1; i < unmandatoryDisciplinesCount || (unmandatoryDisciplinesCount == 0 && i < complexModulesCount); i++)
            {
                bool isDisciplinesPoViboru = false;
                var sd = list[2, i].Split(" ")[0];
                if (sd == "Дисциплины" || list[2, i].Split(" ")[0] == "модуль" || list[2, i].Split(" ")[0] == "Модуль")
                {
                    isDisciplinesPoViboru = true;
                }
                if (isDisciplinesPoViboru == false)
                {
                    Discipline discipline = new()
                    {
                        DisciplineName = list[2, i],
                        StatusDiscipline = _searchEntity.SearchStatusDiscipline("Часть, формируемая участниками образовательных отношений "),
                        ProfileId = profile.Id,
                    };
                    profile.Disciplines.Add(discipline);
                }
            }

            if (unmandatoryDisciplinesCount > 0)
            {
                for (int i = unmandatoryDisciplinesCount + 2; i < complexModulesCount; i++)
                {
                    Discipline discipline = new()
                    {
                        DisciplineName = list[2, i],
                        StatusDiscipline = _searchEntity.SearchStatusDiscipline("К.М.Комплексные модули "),
                        ProfileId = profile.Id,
                    };
                    profile.Disciplines.Add(discipline);
                }
            }
            for (int i = complexModulesCount + 2; i < pacticMandatoryCount; i++)
            {
                Discipline discipline = new()
                {
                    DisciplineName = list[2, i],
                    StatusDiscipline = _searchEntity.SearchStatusDiscipline("Блок 2.Практика. Обязательная часть "),
                    ProfileId = profile.Id,
                };
                profile.Disciplines.Add(discipline);
            }

            for (int i = pacticMandatoryCount + 1; i < pacticUnmandatoryCount; i++)
            {
                Discipline discipline = new()
                {
                    DisciplineName = list[2, i],
                    StatusDiscipline = _searchEntity.SearchStatusDiscipline("Блок 2.Практика. Часть, формируемая участниками образовательных отношений "),
                    ProfileId = profile.Id,
                };
                profile.Disciplines.Add(discipline);
            }

            for (int i = pacticUnmandatoryCount + 2; i < giaCount; i++)
            {
                Discipline discipline = new()
                {
                    DisciplineName = list[2, i],
                    StatusDiscipline = _searchEntity.SearchStatusDiscipline("Блок 3.Государственная итоговая аттестация. "),
                    ProfileId = profile.Id,
                };
                profile.Disciplines.Add(discipline);
            }

            for (int i = giaCount + 2; i < list.GetLength(1); i++)
            {
                Discipline discipline = new()
                {
                    DisciplineName = list[2, i],
                    StatusDiscipline = _searchEntity.SearchStatusDiscipline($"ФТД.Факультативы. {list[0, 49]}".Trim()),
                    ProfileId = profile.Id,
                };
                profile.Disciplines.Add(discipline);
            }
        }
    }
}
