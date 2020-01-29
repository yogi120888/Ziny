using ChemiFriend.Entity;
using ChemiFriend.ENTITY;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChemiFriend.Data.IRepository
{
    public interface IRegistrationRepository : IGenericRepository<Registration>
    {
        Registration CreateUserProfile(Registration registration);
        bool UpdateUserProfile(Registration registration);
    }
}
