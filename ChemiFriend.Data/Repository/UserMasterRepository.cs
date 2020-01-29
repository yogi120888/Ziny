using ChemiFriend.Data.DataContext;
using ChemiFriend.Data;
using ChemiFriend.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChemiFriend.Utility;
using log4net.Repository.Hierarchy;
using log4net;
using ChemiFriend.ENTITY;
using ChemiFriend.Entity.JsonModel;

namespace ChemiFriend.Data.Repository
{
    public class UserMasterRepository : GenericRepository<DataBaseContext, Usermaster>, IUserMasterRepository
    {
        private DataBaseContext _entities;
        private static readonly ILog Logger = LogManager.GetLogger(System.Environment.MachineName);
        #region [Constructor]
        public UserMasterRepository()
        {
            _entities = new DataBaseContext();
        }
        #endregion

        #region [Public Methods]
        public Usermaster DoLogin(string userName, string PWD)
        {
            var LoginDetails = _entities.Usermasters.Where(x => x.UserName == userName.Trim() && x.Password == PWD && x.IsDeleted == false).FirstOrDefault();
            return LoginDetails;
        }

        public Usermaster CreateUser(Usermaster usermaster)
        {
            try
            {
                base.Add(usermaster);
                int rows = base.SaveChanges();
                return rows > 0 ? usermaster : null;
            }
            catch (Exception ex)
            {
                Logger.Error("UserMasterRepository.CreateUser(Usermaster usermaster) : " + ex.Message, ex);
            }
            return null;
        }

        public IQueryable<GetUsermasterModel> GetUserList()
        {
            var query = (from um in _entities.Usermasters
                         join rr in _entities.UserRoles on um.Role equals rr.RoleId
                         select new GetUsermasterModel
                         {
                             UserId = um.UserId,
                             Name = um.Name,
                             UserName = um.UserName,
                             Password = um.Password,
                             Email = um.Email,
                             Phone = um.Phone,
                             Role = um.Role,
                             RoleName = rr.Role,
                             Status = um.Status,
                             IsDeleted = um.IsDeleted,
                         });
            return query;
        }

        #endregion
    }
}
