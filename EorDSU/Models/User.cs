﻿using Microsoft.AspNetCore.Identity;

namespace EorDSU.Models
{
    public class User : IdentityUser
    {
        public int PersDepartmentId { get; set; }
    }
}
