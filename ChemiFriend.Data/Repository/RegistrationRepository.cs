using ChemiFriend.Data.DataContext;
using ChemiFriend.Data.IRepository;
using ChemiFriend.Entity;
using ChemiFriend.ENTITY;
using log4net;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChemiFriend.Data.Repository
{
    public class RegistrationRepository : GenericRepository<DataBaseContext, Registration>, IRegistrationRepository
    {
        private DataBaseContext _entities;
        private static readonly ILog Logger = LogManager.GetLogger(System.Environment.MachineName);
        #region [Constructor]
        public RegistrationRepository()
        {
            _entities = new DataBaseContext();
        }
        #endregion

        #region [Public Methods]

        public Registration CreateUserProfile(Registration registration)
        {
            try
            {
                var _userMaster = _entities.Usermasters.Where(x => x.UserId == registration.UserId).FirstOrDefault();
                _entities.Registrations.Add(registration);
                // Update status of usermaster table
                _userMaster.Status = 1;
                int rows = _entities.SaveChanges();
                return rows > 0 ? registration : null;
            }
            catch (Exception ex)
            {
                Logger.Error("UserMasterRepository.CreateUserProfile(Registration registration) : " + ex.Message, ex);
            }
            return null;
        }

        public bool UpdateUserProfile(Registration registration)
        {
            try
            {
                _entities.Entry(registration).State = EntityState.Modified;
                //base.Update(registration);
                int rows = _entities.SaveChanges();
                return rows > 0 ? true : false;
            }
            catch (Exception ex)
            {
                Logger.Error("UserMasterRepository.CreateUserProfile(Registration registration) : " + ex.Message, ex);
            }
            return false;
        }

        #endregion

    }
}
