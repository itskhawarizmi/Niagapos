/************************************
 * Author Name : Muhammad Hari      *
 * Author URI  : www.icodewizard.com  *
 ************************************/

using Niagapos.Core;

namespace Niagapos
{
    public class SideMenuListItemDesignModel : SideMenuListItemViewModel
    {
        #region Constructor

        /// <summary>
        /// Default constructor.
        /// </summary>
        public SideMenuListItemDesignModel()
        {
            NameMenuItems = "Dashboard";
            MenuIcons = IconType.DashboardIcon;
        }

        #endregion

        #region Singleton

        /// <summary>
        /// A single instance of this design model
        /// </summary>
        public SideMenuListItemDesignModel Instance => new SideMenuListItemDesignModel();

        #endregion



    }


}

