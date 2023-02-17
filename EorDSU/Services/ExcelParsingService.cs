﻿using EorDSU.Common.Interfaces;
using EorDSU.Models;
using EorDSU.Services.Interface;
using System.Collections.Generic;
using Excel = Microsoft.Office.Interop.Excel;

namespace EorDSU.Service
{
    public class ExcelParsingService : IExcelParsingService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ExcelParsingService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
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

            if (list[15, 15] == "основное общее образование")
            {
                await TitulPageForCollegeAsync(list, profile);
            }
            else
            {
                await TitulPageForVuz(list, profile);
                await PlanSvodPage(ObjWorkBook, profile);
            }
        }

        private async Task TitulPageForCollegeAsync(string[,] list, Profile profile)
        {
            string code = list[0, 13];
            profile.LevelEdu = await _unitOfWork.SearchEntity.SearchLevelEdu("основное общее образование");
            profile.LevelEduId = profile.LevelEdu.Id;
            profile.CaseCEdukindId = await _unitOfWork.SearchEntity.SearchEdukind(list[6, 26]);
            profile.CaseSDepartmentId = await _unitOfWork.SearchEntity.SearchCaseSDepartment(list[6, 13].Split(" ")[^1])
                                     ?? await _unitOfWork.SearchEntity.SearchCaseSDepartment(list[6, 13].Split(code)[^1].Trim());
            profile.ProfileName = list[20, 28];
            profile.TermEdu = list[28, 26];
        }

        private async Task TitulPageForVuz(string[,] list, Profile profile)
        {
            profile.CaseCEdukindId = await _unitOfWork.SearchEntity.SearchEdukind(list[2, 41].Split(" ")[^1]);

            switch (list[7, 24].Split(" ")[^1])
            {
                case "Специалистов":
                    profile.LevelEdu = await _unitOfWork.SearchEntity.SearchLevelEdu("специалитет");
                    profile.LevelEduId = profile.LevelEdu.Id;
                    break;
                case "магистратуры":
                    profile.LevelEdu = await _unitOfWork.SearchEntity.SearchLevelEdu("магистратура");
                    profile.LevelEduId = profile.LevelEdu.Id;
                    break;
                case "бакалавриата":
                    profile.LevelEdu = await _unitOfWork.SearchEntity.SearchLevelEdu("бакалавриат");
                    profile.LevelEduId = profile.LevelEdu.Id;
                    break;
            }

            var code = list[3, 26];

            profile.ProfileName = list[3, 29];
            profile.TermEdu = list[2, 42].Split(" ")[^1][0].ToString();
            profile.CaseSDepartmentId = await _unitOfWork.SearchEntity.SearchCaseSDepartment(list[3, 28].Split(" ")[^1])
                ?? await _unitOfWork.SearchEntity.SearchCaseSDepartment(list[3, 28].Split(code)[^1].Trim()); ;

            if (profile.CaseSDepartmentId == null)
            {
                string v = list[3, 28].Split(code)[^1].Trim() + $" ({profile.LevelEdu?.Name})";
                profile.CaseSDepartmentId = await _unitOfWork.SearchEntity.SearchCaseSDepartment(v);
            }

            profile.PersDepartmentId = await _unitOfWork.SearchEntity.SearchPersDepartment("Кафедра " + list[3, 36].ToLower());
            profile.Year = int.Parse(list[22, 39]);
        }

        /// <summary>
        /// Парсинг страницы с дисциплинами
        /// </summary>
        /// <param name="ObjWorkBook"></param>
        /// <param name="profile"></param>
        private async Task PlanSvodPage(Excel.Workbook ObjWorkBook, Profile profile)
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
                        StatusDiscipline = await _unitOfWork.SearchEntity.SearchStatusDiscipline("Обязательная часть"),
                        Profile = profile,
                        ProfileId = profile.Id,
                        CreateDate = DateTime.Now,
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
                        StatusDiscipline = await _unitOfWork.SearchEntity.SearchStatusDiscipline("Часть, формируемая участниками образовательных отношений"),
                        Profile = profile,
                        ProfileId = profile.Id,
                        CreateDate = DateTime.Now,
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
                        StatusDiscipline = await _unitOfWork.SearchEntity.SearchStatusDiscipline("К.М.Комплексные модули"),
                        Profile = profile,
                        ProfileId = profile.Id,
                        CreateDate = DateTime.Now,
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
                    StatusDiscipline = await _unitOfWork.SearchEntity.SearchStatusDiscipline("Блок 2.Практика. Обязательная часть"),
                    Profile = profile,
                    ProfileId = profile.Id,
                    CreateDate = DateTime.Now,
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
                    StatusDiscipline = await _unitOfWork.SearchEntity.SearchStatusDiscipline("Блок 2.Практика. Часть, формируемая участниками образовательных отношений"),
                    Profile = profile,
                    ProfileId = profile.Id,
                    CreateDate = DateTime.Now,
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
                    StatusDiscipline = await _unitOfWork.SearchEntity.SearchStatusDiscipline("Блок 3.Государственная итоговая аттестация."),
                    Profile = profile,
                    ProfileId = profile.Id,
                    CreateDate = DateTime.Now,
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
                    ProfileId = profile.Id,
                    CreateDate = DateTime.Now,
                };

                var sda = list[0, giaCount + 1].Trim();

                if (sda.Length < 2)
                    discipline.StatusDiscipline = await _unitOfWork.SearchEntity.SearchStatusDiscipline($"ФТД.Факультативы".Trim());
                else
                    discipline.StatusDiscipline = await _unitOfWork.SearchEntity.SearchStatusDiscipline($"ФТД.Факультативы. {list[0, giaCount + 1]}".Trim());

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
