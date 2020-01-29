﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ChemiFriend.ENTITY
{
    public class UserRole
    {
        [Key]
        public int RoleId { get; set; }
        public string Role { get; set; }
        public bool Status { get; set; }
    }
}
