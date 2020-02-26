﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChemiFriend.Utility
{
    public enum Roles
    {
        Admin = 1,
        Distributor = 2,
        Retrailer = 3
    }
    public enum CommonBinding
    {
        State = 1,
        Roles = 2
    }

    public enum Status
    {
        New = 0,
        Active = 1,
        Deactive = 2,
        InActive = 3 //ReadyToActive
    }

    public enum ApplicableTaxType
    {
        DPCO = 1,  
        NONDPCO = 2
    }

    public enum Month
    {
        Jan = 1,
        Feb = 2,
        Mar = 3,
        Apr = 4,
        May = 5,
        Jun = 6,
        Jul = 7,
        Aug = 8,
        Sep = 9,
        Oct = 10,
        Nov = 11,
        Dec = 12,
    }

}
