﻿using DSUContextDBService.Models;
using EorDSU.Models;

namespace EorDSU.ResponseModel
{
    public class DataResponseForSvedenOOPDGU
    {
        public Profile? Profile { get; set; }
        public CaseSDepartment? CaseSDepartment { get; set; }
        public CaseCEdukind? CaseCEdukind { get; set; }
    }
}
