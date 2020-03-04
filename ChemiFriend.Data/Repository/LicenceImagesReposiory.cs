using ChemiFriend.Data.DataContext;
using ChemiFriend.Data.IRepository;
using ChemiFriend.Entity;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChemiFriend.Data.Repository
{
    public class LicenceImagesReposiory : GenericRepository<DataBaseContext, LicenceImages>, ILicenceImagesReposiory
    {
        private DataBaseContext _entities;
        private static readonly ILog Logger = LogManager.GetLogger(System.Environment.MachineName);

        #region [Constructor]
        public LicenceImagesReposiory()
        {
            _entities = new DataBaseContext();
        }
        #endregion


    }
}
