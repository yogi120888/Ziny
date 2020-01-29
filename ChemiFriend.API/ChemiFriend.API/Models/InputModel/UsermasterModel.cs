
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ChemiFriend.API.Models.InputModel
{
    public class UsermasterModel
    {
        public Int64 UserId { get; set; }
        [Required]
        public string Name { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Phone { get; set; }
        [Required]
        public int Role { get; set; }
        public byte Status { get; set; } 
        public bool IsDeleted { get; set; }
    }
}