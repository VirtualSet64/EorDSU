using EorDSU.Interface;
using EorDSU.Models;
using EorDSU.Services.Interface;
using Excel = Microsoft.Office.Interop.Excel;

namespace EorDSU.Service
{
    public class ExcelParsingService : IExcelParsingService
    {
        private readonly ISearchEntityService _searchEntity;

        public ExcelParsingService(ISearchEntityService searchEntity)
        {
            _searchEntity = searchEntity;
        }

        public async Task<Profile> ParsingService(string path)
        {
            Excel.Application ObjWorkExcel = new(); //открыть эксель
            Excel.Workbook ObjWorkBook = ObjWorkExcel.Workbooks.Open(@path, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing); //открыть файл

            Profile profile = new();
            await TitulPage(ObjWorkBook, profile);

            ClearAndExitExcel(ObjWorkBook, ObjWorkExcel);

            return profile;
        }

        /// <summary>
        /// Парсинг титульного листа
        /// </summary>
        /// <param name="ObjWorkBook"></param>
        /// <param name="profile"></param>
        private async Task TitulPage(Excel.Workbook ObjWorkBook, Profile profile)
        {
            Excel.Worksheet ObjWorkSheet = (Excel.Worksheet)ObjWorkBook.Sheets[1]; //получить 1 лист
            var lastCell = ObjWorkSheet.Cells.SpecialCells(Excel.XlCellType.xlCellTypeLastCell);//1 ячейку
            string[,] list = new string[lastCell.Column, lastCell.Row]; // массив значений с листа равен по размеру листу

            for (int i = 0; i < lastCell.Column; i++) //по всем колонкам
                for (int j = 0; j < lastCell.Row; j++) // по всем строкам

                    list[i, j] = ObjWorkSheet.Cells[j + 1, i + 1].Text.ToString();//считываем текст в строку

            var ds = list[7, 24];
            if (list[15, 15] == "основное общее образование")
            {
                await TitulPageForCollegeAsync(list, profile);
            }
            else if (ds == "по программе  аспирантуры")
            {
                await TitulPageForPostGraduate(list, profile);
                await PlanSvodPage(ObjWorkBook, profile, true);
            }
            else
            {
                await TitulPageForVuz(list, profile);
                await PlanSvodPage(ObjWorkBook, profile, false);
            }
        }

        /// <summary>
        /// Парсинг титульной страницы для учебного плана колледжа
        /// </summary>
        /// <param name="list"></param>
        /// <param name="profile"></param>
        /// <returns></returns>
        private async Task TitulPageForCollegeAsync(string[,] list, Profile profile)
        {
            string code = list[0, 13];
            profile.LevelEdu = await _searchEntity.SearchLevelEdu("основное общее образование");
            profile.LevelEduId = profile.LevelEdu.Id;
            profile.CaseCEdukindId = await _searchEntity.SearchEdukind(list[6, 26]);
            profile.CaseSDepartmentId = await _searchEntity.SearchCaseSDepartment(list[6, 13].Split(" ")[^1])
                                     ?? await _searchEntity.SearchCaseSDepartment(list[6, 13].Split(code)[^1].Trim());
            profile.ProfileName = list[20, 28];
            profile.TermEdu = list[28, 26];
        }

        /// <summary>
        /// Парсинг титульной страницы для учебного плана Вуза
        /// </summary>
        /// <param name="list"></param>
        /// <param name="profile"></param>
        /// <returns></returns>
        private async Task TitulPageForVuz(string[,] list, Profile profile)
        {
            profile.CaseCEdukindId = await _searchEntity.SearchEdukind(list[2, 41].Split(" ")[^1]);

            switch (list[7, 24].Split(" ")[^1])
            {
                case "Специалистов":
                    profile.LevelEdu = await _searchEntity.SearchLevelEdu("специалитет");
                    profile.LevelEduId = profile.LevelEdu.Id;
                    break;
                case "магистратуры":
                    profile.LevelEdu = await _searchEntity.SearchLevelEdu("магистратура");
                    profile.LevelEduId = profile.LevelEdu.Id;
                    break;
                case "бакалавриата":
                    profile.LevelEdu = await _searchEntity.SearchLevelEdu("бакалавриат");
                    profile.LevelEduId = profile.LevelEdu.Id;
                    break;
            }

            var code = list[3, 26];

            profile.ProfileName = list[3, 29];
            profile.TermEdu = list[2, 42].Split(" ")[^1][0].ToString();
            profile.CaseSDepartmentId = await _searchEntity.SearchCaseSDepartment(list[3, 28].Split(" ")[^1])
                ?? await _searchEntity.SearchCaseSDepartment(list[3, 28].Split(code)[^1].Trim()); ;

            if (profile.CaseSDepartmentId == null)
            {
                string v = list[3, 28].Split(code)[^1].Trim() + $" ({profile.LevelEdu?.Name})";
                profile.CaseSDepartmentId = await _searchEntity.SearchCaseSDepartment(v);
            }

            profile.Year = int.Parse(list[22, 39]);
        }

        /// <summary>
        /// Парсинг титульной страницы для учебного плана аспирантуры
        /// </summary>
        /// <param name="list"></param>
        /// <param name="profile"></param>
        /// <returns></returns>
        private async Task TitulPageForPostGraduate(string[,] list, Profile profile)
        {
            profile.CaseCEdukindId = await _searchEntity.SearchEdukind(list[2, 40].Split(":")[1].Trim());

            profile.LevelEdu = await _searchEntity.SearchLevelEdu("аспирантура");
            profile.LevelEduId = profile.LevelEdu.Id;

            profile.ProfileName = list[3, 28];
            profile.TermEdu = list[2, 41].Split(":")[1].Trim().ToString();

            profile.Year = int.Parse(list[22, 39]);
        }

        /// <summary>
        /// Парсинг страницы с дисциплинами
        /// </summary>
        /// <param name="ObjWorkBook"></param>
        /// <param name="profile"></param>
        private async Task PlanSvodPage(Excel.Workbook ObjWorkBook, Profile profile, bool isPostGraduate)
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

            if (isPostGraduate)
                await DisciplinesForPostGraduate(list, profile);
            else
                await DisciplinesForVuz(list, profile);
        }

        private async Task DisciplinesForVuz(string[,] list, Profile profile)
        {
            int mandatoryDisciplinesCount = 0;
            int unmandatoryDisciplinesCount = 0;
            int complexModulesCount = 0;
            int pacticMandatoryCount = 0;
            int pacticUnmandatoryCount = 0;
            int giaCount = 0;
            for (int i = 5; i < list.GetLength(1); i++)
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
                        Code = list[1, i],
                        StatusDiscipline = await _searchEntity.SearchStatusDiscipline("Обязательная часть"),
                        Profile = profile,
                        ProfileId = profile.Id
                    };
                    discipline.StatusDisciplineId = discipline.StatusDiscipline.Id;
                    profile.Disciplines?.Add(discipline);
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
                        Code = list[1, i],
                        StatusDiscipline = await _searchEntity.SearchStatusDiscipline("Часть, формируемая участниками образовательных отношений"),
                        Profile = profile,
                        ProfileId = profile.Id
                    };
                    discipline.StatusDisciplineId = discipline.StatusDiscipline.Id;
                    profile.Disciplines?.Add(discipline);
                }
            }

            if (unmandatoryDisciplinesCount > 0)
            {
                for (int i = unmandatoryDisciplinesCount + 2; i < complexModulesCount; i++)
                {
                    Discipline discipline = new()
                    {
                        DisciplineName = list[2, i],
                        Code = list[1, i],
                        StatusDiscipline = await _searchEntity.SearchStatusDiscipline("К.М.Комплексные модули"),
                        Profile = profile,
                        ProfileId = profile.Id
                    };
                    discipline.StatusDisciplineId = discipline.StatusDiscipline.Id;
                    profile.Disciplines?.Add(discipline);
                }
            }
            for (int i = complexModulesCount + 2; i < pacticMandatoryCount; i++)
            {
                Discipline discipline = new()
                {
                    DisciplineName = list[2, i],
                    Code = list[1, i],
                    StatusDiscipline = await _searchEntity.SearchStatusDiscipline("Блок 2.Практика. Обязательная часть"),
                    Profile = profile,
                    ProfileId = profile.Id
                };
                discipline.StatusDisciplineId = discipline.StatusDiscipline.Id;
                profile.Disciplines?.Add(discipline);
            }

            for (int i = pacticMandatoryCount + 1; i < pacticUnmandatoryCount; i++)
            {
                Discipline discipline = new()
                {
                    DisciplineName = list[2, i],
                    Code = list[1, i],
                    StatusDiscipline = await _searchEntity.SearchStatusDiscipline("Блок 2.Практика. Часть, формируемая участниками образовательных отношений"),
                    Profile = profile,
                    ProfileId = profile.Id
                };
                discipline.StatusDisciplineId = discipline.StatusDiscipline.Id;
                profile.Disciplines?.Add(discipline);
            }

            for (int i = pacticUnmandatoryCount + 2; i < giaCount; i++)
            {
                Discipline discipline = new()
                {
                    DisciplineName = list[2, i],
                    Code = list[1, i],
                    StatusDiscipline = await _searchEntity.SearchStatusDiscipline("Блок 3.Государственная итоговая аттестация."),
                    Profile = profile,
                    ProfileId = profile.Id
                };
                discipline.StatusDisciplineId = discipline.StatusDiscipline.Id;
                profile.Disciplines?.Add(discipline);
            }

            for (int i = giaCount + 2; i < list.GetLength(1); i++)
            {
                Discipline discipline = new()
                {
                    DisciplineName = list[2, i],
                    Code = list[1, i],
                    Profile = profile,
                    ProfileId = profile.Id
                };

                var sda = list[0, giaCount + 1].Trim();

                if (sda.Length < 2)
                    discipline.StatusDiscipline = await _searchEntity.SearchStatusDiscipline($"ФТД.Факультативы".Trim());
                else
                    discipline.StatusDiscipline = await _searchEntity.SearchStatusDiscipline($"ФТД.Факультативы. {list[0, giaCount + 1]}".Trim());

                discipline.StatusDisciplineId = discipline.StatusDiscipline.Id;
                profile.Disciplines?.Add(discipline);
            }
        }

        private async Task DisciplinesForPostGraduate(string[,] list, Profile profile)
        {
            int preparetionPublications = 0;
            int interimCertification = 0;
            int educationComponent = 0;
            int pactic = 0;
            int interimCertificationForDiscipline = 0;
            int finalCertification = 0;
            for (int i = 5; i < list.GetLength(1); i++)
            {
                if (list[0, i].Trim() == "1.2.Подготовка публикаций и(или) заявок на патенты")
                {
                    preparetionPublications = i;
                }
                if (list[0, i].Trim() == "1.3.Промежуточная аттестация по этапам выполнения научного исследования")
                {
                    interimCertification = i;
                }
                if (list[0, i].Trim() == "2.Образовательный компонент")
                {
                    educationComponent = i;
                }
                if (list[0, i].Trim() == "2.2.Практика")
                {
                    pactic = i;
                }
                if (list[0, i].Trim() == "2.3.Промежуточная аттестация по дисциплинам (модулям) и практике")
                {
                    interimCertificationForDiscipline = i;
                }
                if (list[0, i].Trim() == "3.Итоговая аттестация")
                {
                    finalCertification = i;
                    break;
                }
            }

            for (int i = 5; i < preparetionPublications; i++)
            {
                bool isModule = false;
                for (int j = 0; j < list[2, i].Split(" ").Length; j++)
                {
                    if (list[2, i].Split(" ")[j] == "дисциплины" || list[2, i].Split(" ")[j] == "Дисциплины")
                    {
                        isModule = true;
                    }
                }
                if (isModule == false)
                {
                    Discipline discipline = new()
                    {
                        DisciplineName = list[2, i],
                        Code = list[1, i],
                        StatusDiscipline = await _searchEntity.SearchStatusDiscipline("1.1.Научная деятельность, направленная на подготовку диссертации к защите"),
                        Profile = profile,
                        ProfileId = profile.Id
                    };
                    discipline.StatusDisciplineId = discipline.StatusDiscipline.Id;
                    profile.Disciplines?.Add(discipline);
                }
            }

            for (int i = preparetionPublications + 1; i < interimCertification; i++)
            {
                bool isDisciplinesPoViboru = false;
                var sd = list[2, i].Split(" ")[0];
                if (sd == "Дисциплины" || list[2, i].Split(" ")[0] == "дисциплины" || list[2, i].Split(" ")[0] == "Дисциплины")
                {
                    isDisciplinesPoViboru = true;
                }
                if (isDisciplinesPoViboru == false)
                {
                    Discipline discipline = new()
                    {
                        DisciplineName = list[2, i],
                        Code = list[1, i],
                        StatusDiscipline = await _searchEntity.SearchStatusDiscipline("1.2.Подготовка публикаций и(или) заявок на патенты"),
                        Profile = profile,
                        ProfileId = profile.Id
                    };
                    discipline.StatusDisciplineId = discipline.StatusDiscipline.Id;
                    profile.Disciplines?.Add(discipline);
                }
            }

            for (int i = interimCertification + 1; i < educationComponent; i++)
            {
                bool isDisciplinesPoViboru = false;
                var sd = list[2, i].Split(" ")[0];
                if (sd == "Дисциплины" || list[2, i].Split(" ")[0] == "дисциплины" || list[2, i].Split(" ")[0] == "Дисциплины")
                {
                    isDisciplinesPoViboru = true;
                }
                if (isDisciplinesPoViboru == false)
                {
                    Discipline discipline = new()
                    {
                        DisciplineName = list[2, i],
                        Code = list[1, i],
                        StatusDiscipline = await _searchEntity.SearchStatusDiscipline("1.3.Промежуточная аттестация по этапам выполнения научного исследования"),
                        Profile = profile,
                        ProfileId = profile.Id
                    };
                    discipline.StatusDisciplineId = discipline.StatusDiscipline.Id;
                    profile.Disciplines?.Add(discipline);
                }
            }

            for (int i = educationComponent + 2; i < pactic; i++)
            {
                bool isDisciplinesPoViboru = false;
                var sd = list[2, i].Split(" ")[0];
                if (sd == "Дисциплины" || list[2, i].Split(" ")[0] == "дисциплины" || list[2, i].Split(" ")[0] == "Дисциплины" 
                                       || list[2, i].Split(" ")[1] == "дисциплины" || list[2, i].Split(" ")[1] == "Дисциплины")
                {
                    isDisciplinesPoViboru = true;
                }
                if (isDisciplinesPoViboru == false)
                {
                    Discipline discipline = new()
                    {
                        DisciplineName = list[2, i],
                        Code = list[1, i],
                        StatusDiscipline = await _searchEntity.SearchStatusDiscipline("2.Образовательный компонент"),
                        Profile = profile,
                        ProfileId = profile.Id
                    };
                    discipline.StatusDisciplineId = discipline.StatusDiscipline.Id;
                    profile.Disciplines?.Add(discipline);
                }
            }

            for (int i = pactic + 1; i < interimCertificationForDiscipline; i++)
            {
                bool isDisciplinesPoViboru = false;
                var sd = list[2, i].Split(" ")[0];
                if (sd == "Дисциплины" || list[2, i].Split(" ")[0] == "дисциплины" || list[2, i].Split(" ")[0] == "Дисциплины")
                {
                    isDisciplinesPoViboru = true;
                }
                if (isDisciplinesPoViboru == false)
                {
                    Discipline discipline = new()
                    {
                        DisciplineName = list[2, i],
                        Code = list[1, i],
                        StatusDiscipline = await _searchEntity.SearchStatusDiscipline("2.2.Практика"),
                        Profile = profile,
                        ProfileId = profile.Id
                    };
                    discipline.StatusDisciplineId = discipline.StatusDiscipline.Id;
                    profile.Disciplines?.Add(discipline);
                }
            }

            for (int i = interimCertificationForDiscipline + 1; i < finalCertification; i++)
            {
                Discipline discipline = new()
                {
                    DisciplineName = list[2, i],
                    Code = list[1, i],
                    Profile = profile,
                    ProfileId = profile.Id,
                    StatusDiscipline = await _searchEntity.SearchStatusDiscipline("2.3.Промежуточная аттестация по дисциплинам (модулям) и практике")
                };

                discipline.StatusDisciplineId = discipline.StatusDiscipline.Id;
                profile.Disciplines?.Add(discipline);
            }

            for (int i = finalCertification + 1; i < list.GetLength(1); i++)
            {
                Discipline discipline = new()
                {
                    DisciplineName = list[2, i],
                    Code = list[1, i],
                    Profile = profile,
                    ProfileId = profile.Id,
                    StatusDiscipline = await _searchEntity.SearchStatusDiscipline("3.Итоговая аттестация")
                };

                discipline.StatusDisciplineId = discipline.StatusDiscipline.Id;
                profile.Disciplines?.Add(discipline);
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
