/************************************
 * Author Name : Muhammad Hari      *
 * Author URI  : www.icodewizard.com  *
 ************************************/

using Niagapos.Core;
using System.Collections.Generic;

namespace Niagapos
{ 
    public class SideMenuListDesignModel : SideMenuListViewModel
    {
        #region Singleton

        /// <summary>
        /// A single instance of this design model
        /// </summary>
        public static SideMenuListDesignModel Instance => new SideMenuListDesignModel();

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor.
        /// </summary>
        public SideMenuListDesignModel()
        {
            Items = new List<SideMenuListItemViewModel>
            {
                 new SideMenuListItemViewModel
                 {
                     NameMenuItems = "Dashboard",
                     MenuIcons = IconType.DashboardIcon,
                 },

                 new SideMenuListItemViewModel
                 {
                     NameMenuItems = "User",
                     MenuIcons = IconType.EmployeesIcon,
                 },

                  new SideMenuListItemViewModel
                 {
                     NameMenuItems = "Customer",
                     MenuIcons = IconType.CustomersIcon,
                 },


                 new SideMenuListItemViewModel
                 {
                     NameMenuItems = "Produk",
                     MenuIcons = IconType.ProductsIcon,

                 },

                 new SideMenuListItemViewModel
                 {
                     NameMenuItems = "Transaksi",
                     MenuIcons = IconType.TransactionsIcon,

                 },


                 new SideMenuListItemViewModel
                 {
                     NameMenuItems = "Laporan",
                     MenuIcons = IconType.ReportsIcon,

                 },

           
         

                 new SideMenuListItemViewModel
                 {
                     NameMenuItems = "Logout",
                     MenuIcons = IconType.Logout,
                 },
            };
        }

        #endregion


    }
}

