/************************************
 * Author Name : Muhammad Hari      *
 * Author URI  : www.mediasimfoni.com  *
 ************************************/


using System;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Linq;
using Niagapos.Core;
using System.Data.Entity;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Collections.Generic;

namespace Niagapos
{
    /// <summary>
    /// The view model for a login screen
    /// </summary>
    public class LoginViewModel : BaseViewModel
    {
        

        #region Constructor

        /// <summary>
        /// Default constructor.
        /// </summary>
        public LoginViewModel()
        {
            LoginCommand = new RelayParameterizedCommand(async (parameter) => await LoginAsync(parameter));
            DeveloperCommand = new RelayCommand(async () => await DeveloperAsync());
            
         
        }
        #endregion

        #region Public Properties

        /// <summary>
        /// The username of users
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// The unsecure password of users
        /// </summary>
        public string UnsecurePassword { get; set; }
     
        /// <summary>
        /// A flag indicating if the login command is running
        /// </summary>
        public bool LoginIsRunning { get; set; }
        

        #endregion

        #region Commands

        /// <summary>
        /// The command to login
        /// </summary>
        public ICommand LoginCommand { get; set; }

        /// <summary>
        /// The command to register for a new account
        /// </summary>
        public ICommand DeveloperCommand { get; set; }

        #endregion

        public async Task DeveloperAsync()
        {

            DI.Application.GoToPage(ApplicationPage.Register);

            await Task.Delay(1);
        }

        /// <summary>
        /// Attempts to log the user in
        /// </summary>
        /// <param name="parameter">The <see cref="SecureString"/> passed in from the view for the users password</param>
        /// <returns></returns>
        public async Task LoginAsync(object parameter)
        {
            await RunCommandAsync(() => LoginIsRunning, async () =>
            {
                try
                {
                    using (var Context = new NiagaposEntities())
                    {


                        UnsecurePassword = (parameter as IHavePassword).SecurePassword.Unsecure();

                        if (UnsecurePassword == "" && Username == null)
                        {
                            await DI.UI.ShowMessage(new MessageBoxDialogViewModel
                            {
                                Title = "Message",
                                Message = "Silakan masukan Username dan Password Anda",
                            });
                        }
                        else if (UnsecurePassword == "" || UnsecurePassword == null)
                        {
                            await DI.UI.ShowMessage(new MessageBoxDialogViewModel
                            {
                                Title = "Message",
                                Message = "Password masih kosong, silakan isi terlebih dahulu",
                            });
                        }
                        else if (Username == null || Username == "")
                        {
                            await DI.UI.ShowMessage(new MessageBoxDialogViewModel
                            {
                                Title = "Message",
                                Message = "Username masih kosong, silakan isi terlebih dahulu",
                            });
                        }

                        else
                        {
                            UnsecurePassword = PasswordEncryptions.EncryptToMD5Cryptography(UnsecurePassword);

                            var query = from user in Context.Users
                                        join access in Context.UserAccesses on user.UserAccessId equals access.UserAccessId
                                        where user.Username == Username.Trim() &&
                                              user.Password == UnsecurePassword.Trim()
                                        select new
                                        {
                                            user.Name,
                                            user.Username,
                                            user.Password,
                                            access.Caption,

                                        };

                            var list = new List<User>();

                            var query2 = (from user in Context.Users
                                          where user.Username == Username.Trim() &&
                                                user.Password == UnsecurePassword.Trim()
                                          orderby user.UserId
                                          select user).ToList();


                            foreach(var user in query2.ToList())
                            {
                                list.Add(new User { Username = user.Username, Password = user.Password });
                            }



                            if (query.SingleOrDefault() != null)
                            {


                                foreach (var user in query.ToList())
                                {

                                    DI.UserCredential.Caption = user.Caption;
                                    DI.UserCredential.Name = user.Name;
                                    DI.Transaction.User = user.Name;
                                }



                                DI.Application.GoToPage(ApplicationPage.Dashboard);



                                await Task.Delay(TimeSpan.FromSeconds(1));

                            }
                            else
                            {
                                await DI.UI.ShowMessage(new MessageBoxDialogViewModel
                                {
                                    Title = "Message",
                                    Message = "Username atau Password Anda salah, silakan coba lagi",
                                });
                            }


                        }




                    }
                }
                catch (Exception ex)
                {
                    CoreDI.Logger.Log(ex.Message);
                }



                await Task.Delay(TimeSpan.FromSeconds(1));

            }); 



        }

     

    }

}

