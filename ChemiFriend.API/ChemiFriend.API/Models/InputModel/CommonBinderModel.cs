using ChemiFriend.ENTITY;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ChemiFriend.API.Models.InputModel
{
    public class CommonBinderModel
    {
        public List<State> lstState { get; set; }
        public List<UserRole> lstRole { get; set; }
    }
}