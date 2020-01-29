using ChemiFriend.Entity;
using ChemiFriend.Entity.JsonModel;
using ChemiFriend.ENTITY;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChemiFriend.Data.Repository
{
    public interface IUserMasterRepository : IGenericRepository<Usermaster>
    {

        Usermaster DoLogin(string userName, string PWD);
        Usermaster CreateUser(Usermaster usermaster);
        IQueryable<GetUsermasterModel> GetUserList();
    }
}
