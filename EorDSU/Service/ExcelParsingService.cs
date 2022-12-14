using EorDSU.DBService;
using EorDSU.Interface;
using EorDSU.Models;
using EorDSU.ResponseModel;
using Microsoft.EntityFrameworkCore;
using Sentry;
using System.Collections.Generic;
using Excel = Microsoft.Office.Interop.Excel;

namespace EorDSU.Service
{
    public class ExcelParsingService
    {
        private readonly ApplicationContext _applicationContext;
        private readonly DSUContext _dSUContext;
        private readonly ISearchEntity _searchEntity;
        private readonly IActiveData _activeData;

        public ExcelParsingService(ApplicationContext applicationContext, DSUContext dSUContext, ISearchEntity searchEntity, IActiveData activeData)
        {
            _applicationContext = applicationContext;
            _dSUContext = dSUContext;
            _searchEntity = searchEntity;
            _activeData = activeData;
        }

        public async Task<Profile> ParsingService(string path)
        {
            try
            {
                Excel.Application ObjWorkExcel = new(); //открыть эксель
                Excel.Workbook ObjWorkBook = ObjWorkExcel.Workbooks.Open(@path, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing); //открыть файл

                Profile profile = new();
                TitulPage(ObjWorkBook, ObjWorkExcel, profile);
                PlanSvodPage(ObjWorkBook, ObjWorkExcel, profile);

                ClearAndExitExcel(ObjWorkBook, ObjWorkExcel);

                var profilesFromDB = _activeData.GetProfiles().ToList();

                if (!profilesFromDB.Any(x => x.ProfileName == profile.ProfileName &&
                                 x.TermEdu == profile.TermEdu &&
                                 x.CaseCEdukindId == profile.CaseCEdukindId &&
                                 x.CaseSDepartmentId == profile.CaseSDepartmentId &&
                                 x.LevelEdu == profile.LevelEdu &&
                                 x.Year == profile.Year))
                    await _applicationContext.Profiles.AddAsync(profile);

                await _applicationContext.SaveChangesAsync();
                return profile;
            }
            catch (Exception ex)
            {
                SentrySdk.CaptureException(ex);
                throw;
            }
        }

        /// <summary>
        /// Парсинг титульного листа
        /// </summary>
        /// <param name="ObjWorkBook"></param>
        /// <param name="profile"></param>
        private void TitulPage(Excel.Workbook ObjWorkBook, Excel.Application ObjWorkExcel, Profile profile)
        {
            try
            {
                Excel.Worksheet ObjWorkSheet = (Excel.Worksheet)ObjWorkBook.Sheets[1]; //получить 1 лист
                var lastCell = ObjWorkSheet.Cells.SpecialCells(Excel.XlCellType.xlCellTypeLastCell);//1 ячейку
                string[,] list = new string[lastCell.Column, lastCell.Row]; // массив значений с листа равен по размеру листу

                for (int i = 0; i < lastCell.Column; i++) //по всем колонкам
                    for (int j = 0; j < lastCell.Row; j++) // по всем строкам

                        list[i, j] = ObjWorkSheet.Cells[j + 1, i + 1].Text.ToString();//считываем текст в строку

                profile.CaseCEdukindId = _searchEntity.SearchEdukind(list[2, 41].Split(" ")[^1])?.EdukindId;
                //[^1] = List[List.Lenght - 1]

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

                var code = list[3, 26];

                profile.ProfileName = list[3, 29];
                profile.TermEdu = int.Parse(list[2, 42].Split(" ")[^1][0].ToString());
                profile.CaseSDepartmentId = _searchEntity.SearchCaseSDepartment(list[3, 28].Split(" ")[^1])?.DepartmentId;

                if (profile.CaseSDepartmentId == null)
                {
                    profile.CaseSDepartmentId = _searchEntity.SearchCaseSDepartment(list[3, 28].Split(code)[^1].Trim())?.DepartmentId;
                    if (profile.CaseSDepartmentId == null)
                    {
                        string v = list[3, 28].Split(code)[^1].Trim() + $" ({profile.LevelEdu.Name})";
                        profile.CaseSDepartmentId = _searchEntity.SearchCaseSDepartment(v)?.DepartmentId;
                    }
                }

                profile.PersDepartmentId = _searchEntity.SearchPersDepartment("Кафедра " + list[3, 36].ToLower())?.DepId;
                profile.Year = int.Parse(list[22, 39]);
            }
            catch (Exception ex)
            {
                ClearAndExitExcel(ObjWorkBook, ObjWorkExcel);
                SentrySdk.CaptureException(ex);
                throw;
            }
        }

        /// <summary>
        /// Парсинг страницы с дисциплинами
        /// </summary>
        /// <param name="ObjWorkBook"></param>
        /// <param name="profile"></param>
        private void PlanSvodPage(Excel.Workbook ObjWorkBook, Excel.Application ObjWorkExcel, Profile profile)
        {
            try
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
                            Profile = profile,
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
                            Profile = profile,
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
                            Profile = profile,
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
                        Profile = profile,
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
                        Profile = profile,
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
                        Profile = profile,
                        ProfileId = profile.Id,
                    };
                    profile.Disciplines.Add(discipline);
                }

                for (int i = giaCount + 2; i < list.GetLength(1); i++)
                {
                    Discipline discipline = new()
                    {
                        DisciplineName = list[2, i],
                        Profile = profile,
                        ProfileId = profile.Id,
                    };
                    var sda = list[0, giaCount + 1].Trim();

                    if (sda.Length < 2)
                        discipline.StatusDiscipline = _searchEntity.SearchStatusDiscipline($"ФТД.Факультативы".Trim());
                    else
                        discipline.StatusDiscipline = _searchEntity.SearchStatusDiscipline($"ФТД.Факультативы. {list[0, giaCount + 1]}".Trim());

                    profile.Disciplines.Add(discipline);
                }
            }
            catch (Exception ex)
            {
                ClearAndExitExcel(ObjWorkBook, ObjWorkExcel);
                SentrySdk.CaptureException(ex);
                throw;
            }
        }

        private static void ClearAndExitExcel(Excel.Workbook ObjWorkBook, Excel.Application ObjWorkExcel)
        {
            ObjWorkBook.Close(false, Type.Missing, Type.Missing); //закрыть не сохраняя
            ObjWorkExcel.Quit(); // выйти из экселя
            GC.Collect(); // убрать за собой
        }
    }
}
