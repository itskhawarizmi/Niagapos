using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Input;
using System.Linq;
using System.Data.Entity;
using System.Windows;
using System.ComponentModel;
using System.Data.SqlClient;
using Niagapos.Core;

namespace Niagapos
{
    public class UserViewModel : BaseViewModel, IDatabaseManager<NiagaposEntities, User>
    {

        public UserViewModel()
        {
            if (DesignerProperties.GetIsInDesignMode(new DependencyObject()))
                return;

            ConnectionString = @"data source=ALPHA-IDR1896;initial catalog=Niagapos;integrated security=True;";

            Context = new NiagaposEntities();
            UsersItem = new ObservableCollection<User>();
            SelectedItems = new User();
            UsersModelCollection = new ObservableCollection<UserModel>();
            GetUserModel = new UserModel();
            LoadData();

            SetupCommand();
        }

        public string ConnectionString { get; set; }
        public User SelectedItems { get; set; }
        public NiagaposEntities Context { get; set; }
        public ObservableCollection<User> UsersItem { get; set; }

        public ObservableCollection<UserModel> UsersModelCollection { get; set; }
        public UserModel GetUserModel { get; set; }

        public ICommand AddDataCommand { get; set; }
        public ICommand UpdateDataCommand { get; set; }
        public ICommand DeleteDataCommand { get; set; }
        public ICommand RefreshDataCommand { get; set; }
        public ICommand SearchDataCommand { get; set; }

        /// <summary>
        /// Show edit menu control to visible
        /// </summary>
        public ICommand EditControlMenuCommand { get; set; }

        /// <summary>
        /// Close edit menu control or hide the menu
        /// </summary>
        public ICommand CloseEditControlMenuCommand { get; set; }

        /// <summary>
        /// Show Add menu control to visible
        /// </summary>
        public ICommand AddControlMenuCommand { get; set; }

        /// <summary>
        /// Close add menu control or hide the menu
        /// </summary>
        public ICommand CloseAddControlMenuCommand { get; set; }

        /// <summary>
        /// Show dialogs to users for input a specific data image
        /// </summary>
        public ICommand DialogPictureCommand { get; set; }
        

        public string ActiveStatus { get; set; }
        public string AdmLevel { get; set; }
        public string AccessLevel { get; set; }

        public string UnsecurePassword { get; set; }
        public bool Flag { get; set; }

        public bool CanEdit { get; set; }

        public string FilePathImg { get; set; }

        public string SearchText { get; set; }
        public ICommand ClearSearchHistoryCommand { get; set; }

        public void SetupCommand()
        {
            AddDataCommand = new RelayParameterizedCommand(async (parameter) => await AddData(parameter));
            DeleteDataCommand = new RelayCommand(() => DeleteData());
            UpdateDataCommand = new RelayParameterizedCommand(async (parameter) => await UpdateData(parameter));
            RefreshDataCommand = new RelayCommand(async () => await Refresh());
            SearchDataCommand = new RelayCommand(() => Search());
            ClearSearchHistoryCommand = new RelayCommand(() => ClearSearchHistory());

            EditControlMenuCommand = new RelayCommand(() => CanEditControl());
            CloseEditControlMenuCommand = new RelayCommand(() => CloseEditControl());
            AddControlMenuCommand = new RelayCommand(() => AddUsersControl());
            CloseAddControlMenuCommand = new RelayCommand(() => CloseMenu());
            DialogPictureCommand = new RelayCommand(() => DialogPicture());
            
        }

     

        public void AddUsersControl()
        {
            DI.Application.AddUsersControlMenuVisible = true;
        }

        public void CloseMenu()
        {
            DI.Application.AddUsersControlMenuVisible = false;
        }

        public void CanEditControl() => CanEdit = true;

        public void CloseEditControl() => CanEdit = false;

        public string ValidationUserAccess(string accessData)
        {
            accessData = accessData.ToLower();

            switch (accessData)
            {
                case "administrator":
                    return accessData = "USR01001";
                case "cashier":
                    return accessData = "USR02001";
                default:
                    DI.UI.ShowMessage(new MessageBoxDialogViewModel
                    {
                        Title = "Message",
                        Message = "Akses Level User Saat ini Antara Administrator dengan Cashier",
                        DialogsYesVisible = false

                    });
                    break;
                    
            }

            return accessData;
            
        }

        public async Task AddData(object parameter)
        {
            try
            {
                if ((GetUserModel.UserId == null && GetUserModel.Username == null &&
                       GetUserModel.UserAccessId == null && GetUserModel.Password == null &&
                       GetUserModel.LevelAccess == null && GetUserModel.AdministratorLevel == null &&
                       GetUserModel.Caption == null && GetUserModel.ActiveStatus == null && GetUserModel.LastLoggedIn == null))
                {
                    await DI.UI.ShowMessage(new MessageBoxDialogViewModel
                    {
                        Title = "Pesan",
                        Message = "Tidak dapat menyimpan data, karna semua field masih kosong!",

                    });

                    return;
                }

                    UnsecurePassword = (parameter as IHavePassword).SecurePassword.Unsecure();
                    UnsecurePassword = PasswordEncryptions.EncryptToMD5Cryptography(UnsecurePassword);

                    var userAccessId = ValidationUserAccess(GetUserModel.LevelAccess);
                    SelectedItems = new User
                    {
                        UserId = GetUserModel.UserId,
                        Username = GetUserModel.Username,
                        Password = UnsecurePassword,
                        Name = GetUserModel.Name,
                        ActiveStatus = GetUserModel.ActiveStatus,
                        Administrator = GetUserModel.AdministratorLevel,
                        LastLoggedIn = DateTime.Now,
                        UserAccessId = userAccessId,
                        
                        
                    };

                    if (SelectedItems != null)
                    {
                      
                    var users = (from user in Context.Users
                                        where user.UserId == GetUserModel.UserId || user.UserId.Contains(GetUserModel.UserId)
                                        select user).ToList();

                        if (users.Count > 0)
                        {
                            await DI.UI.ShowMessage(new MessageBoxDialogViewModel
                            {
                                Title = "Pesan",
                                Message = "User ID ini sudah ada, silakan gunakan ID yang lain!",
                                DialogsYesVisible = false

                            });

                            return;
                        }
                        else
                        {
                            Context.Users.Add(SelectedItems);
                            Context.SaveChanges();
                            await Task.Delay(TimeSpan.FromSeconds(0.1));
                            await DI.UI.ShowMessage(new MessageBoxDialogViewModel
                            {
                                Title = "Pesan",
                                Message = "Data baru telah ditambahkan dan berhasil disimpan!",
                                DialogsYesVisible = false

                            });


                        }

                    }


                    Context.SaveChanges();

                    await Refresh();
                

            }
            catch (Exception)
            {
                await DI.UI.ShowMessage(new MessageBoxDialogViewModel
                {
                    Title = "Error",
                    Message = "Opss.. Telah terjadi kesalahan saat tambah data..",
                    DialogsYesVisible = false


                });
            }

            await Task.Delay(TimeSpan.FromSeconds(0.1));
        }

        public async void DeleteData()
        {


            if (System.Windows.MessageBox.Show($"Apakah Anda yakin ingin menghapus data ini {GetUserModel.UserId} ?", "Message",
                MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.Yes) == MessageBoxResult.Yes)
            {

                try
                {

                    if (GetUserModel.UserId == null || GetUserModel.UserId == "")
                    {
                        await DI.UI.ShowMessage(new MessageBoxDialogViewModel
                        {
                            Title = "Pesan",
                            Message = "Silakan pilih data yang ingin dihapus",
                            DialogsYesVisible = false
                        });
                        return;

                    }
                    else
                    {
                        var query = (from user in Context.Users
                                     where user.UserId == GetUserModel.UserId
                                     select user).SingleOrDefault();


                        if (query != null)
                        {
                            Context.Users.Remove(query);
                            Context.SaveChanges();
                            Flag = true;
                        }

                        if (Flag)
                        {

                            await DI.UI.ShowMessage(new MessageBoxDialogViewModel
                            {
                                Title = "Pesan",
                                Message = "Data berhasil dihapus",
                                DialogsYesVisible = false
                            });

                        }

                    }




                    await Refresh();


                }
                catch (Exception)
                {
                    await DI.UI.ShowMessage(new MessageBoxDialogViewModel
                    {
                        Title = "Error",
                        Message = "Terjadi kesalahan saat hapus data!",
                        DialogsYesVisible = false
                    });

                    Flag = false;
                }

            }

        }

        public async void DialogPicture()
        {
            try
            {
                using (var ofd = new OpenFileDialog() { Filter = "JPEG|*.jpg|PNG|*.png", ValidateNames = true })
                {
                    if (ofd.ShowDialog() == DialogResult.OK)
                    {
                        FilePathImg = ofd.FileName;
                    }
                }
            }
            catch (Exception Ex)
            {
                await DI.UI.ShowMessage(new MessageBoxDialogViewModel
                {
                    Title = "Error",
                    Message = "The format is wrong please try again",
                    DialogsYesVisible = false
                });

                CoreDI.Logger.Log(Ex.ToString(), LogLevel.Error);
            }

        }

        public async void LoadData()
        {
            if (!string.IsNullOrEmpty(SearchText))
                SearchText = string.Empty;

            UsersModelCollection.Clear();

            var query = (from user in Context.Users
                         join access in Context.UserAccesses on user.UserAccessId equals access.UserAccessId
                         orderby user.UserId
                         select new
                         {
                            user.UserId,
                            user.UserAccessId,
                            user.Username,
                            user.Password,
                            user.Name,
                            user.Administrator,
                            user.ActiveStatus,
                            user.LastLoggedIn,
                            access.Caption,
                            access.LevelAccess,
                            
                            

                         }).ToList();



            foreach (var user in query)
            {
                UsersModelCollection.Add(new UserModel
                {
                    UserId = user.UserId,
                    Username = user.Username,
                    Password = user.Password,
                    UserAccessId = user.UserAccessId,
                    Name = user.Name,
                    AdministratorLevel = user.Administrator,
                    LastLoggedIn = user.LastLoggedIn.ToString(),
                    LevelAccess = user.LevelAccess,
                    Caption = user.Caption,
                    ActiveStatus = user.ActiveStatus,
                    


                });
            }

            await Task.Delay(TimeSpan.FromSeconds(0.5));


        }

        public async Task Refresh()
        {
            Context.Products.Load();
            LoadData();
            await Task.Delay(TimeSpan.FromSeconds(0.1));
        }

        public async void Search()
        {
            if (string.IsNullOrEmpty(SearchText))
                return;

            else
            {
                try
                {
                    var query = (from user in Context.Users
                                 join access in Context.UserAccesses on user.UserAccessId equals access.UserAccessId
                                 where (user.Name == SearchText) || (user.Name.Contains(SearchText)) ||
                                       (user.UserId == SearchText) || (user.UserId.Contains(SearchText)) ||
                                       (user.Username == SearchText) || (user.Username.Contains(SearchText))

                                 select new
                                 {
                                     user.UserId,
                                     user.Username,
                                     user.Password,
                                     user.Administrator,
                                     user.ActiveStatus,
                                     user.LastLoggedIn,
                                     user.UserAccessId,
                                     user.Name,
                                     access.Caption,

                                 });

                    UsersModelCollection.Clear();


                    foreach (var v in query.ToList())
                    {
                        UsersModelCollection.Add(new UserModel
                        {
                            UserId = v.UserId,
                            Name = v.Name,
                            Username = v.Username,
                            Password = v.Password,
                            AdministratorLevel = v.Administrator,
                            ActiveStatus = v.ActiveStatus,
                            Caption = v.Caption,
                        });
                    }

                    await Task.Delay(TimeSpan.FromSeconds(0.1));
                }
                catch (Exception)
                {
                    await DI.UI.ShowMessage(new MessageBoxDialogViewModel
                    {
                        Title = "Error",
                        Message = $"Data {SearchText} Tidak ditemukan di dalam database",
                        DialogsYesVisible = false


                    });
                }
               
            }
        }

        public void ClearSearchHistory()
        {
            if (!string.IsNullOrEmpty(SearchText))
                SearchText = string.Empty;
        }


        public async Task UpdateData(object parameter)
        {
            try
            {
               
                using (SqlConnection Connection = new SqlConnection(ConnectionString))
                {
                    using (SqlCommand CommandQuery = new SqlCommand())
                    {
                        UnsecurePassword = (parameter as IHavePassword).SecurePassword.Unsecure();
                        UnsecurePassword = PasswordEncryptions.EncryptToMD5Cryptography(UnsecurePassword);


                        try
                        {
                            var userAccessId = ValidationUserAccess(GetUserModel.Caption);

                            Connection.Open();
                            CommandQuery.Connection = Connection;
                            CommandQuery.CommandText = "UPDATE Users SET Username = @username, Name = @name, Password = @password, Administrator = @administrator, ActiveStatus = @activeStatus, UserAccessId = @userAccessId, LastLoggedIn = @lastLoggedIn WHERE UserId = @id";
                            CommandQuery.Parameters.AddWithValue("@id", GetUserModel.UserId ?? (object)DBNull.Value);
                            CommandQuery.Parameters.AddWithValue("@name", GetUserModel.Name ?? (object)DBNull.Value);
                            CommandQuery.Parameters.AddWithValue("@username", GetUserModel.Username);
                            CommandQuery.Parameters.AddWithValue("@password", UnsecurePassword ?? (object)DBNull.Value);
                            CommandQuery.Parameters.AddWithValue("@userAccessId", userAccessId ?? (object)DBNull.Value);
                            CommandQuery.Parameters.AddWithValue("@activeStatus", GetUserModel.ActiveStatus ?? (object)DBNull.Value);
                            CommandQuery.Parameters.AddWithValue("@administrator", GetUserModel.AdministratorLevel ?? (object)DBNull.Value);
                            CommandQuery.Parameters.AddWithValue("@lastLoggedIn", DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") ?? (object)DBNull.Value);
                            CommandQuery.ExecuteNonQuery();

                            Context.SaveChanges();

                            await Task.Delay(TimeSpan.FromSeconds(0.1));

                            await Refresh();

                            await DI.UI.ShowMessage(new MessageBoxDialogViewModel
                            {
                                Title = "Pesan",
                                Message = "Data telah diperbarui dan berhasil disimpan",
                                DialogsYesVisible = false

                            });


                        }
                        catch (Exception ex)
                        {
                            await DI.UI.ShowMessage(new MessageBoxDialogViewModel
                            {
                                Title = "Pesan",
                                Message = "Terjadi kesalahan saat pembaruan data!",
                                DialogsYesVisible = false,


                            });

                            CoreDI.Logger.Log(ex.Message);

                        }
                    }
                }
                

               


            }
            catch (Exception)
            {
                await DI.UI.ShowMessage(new MessageBoxDialogViewModel
                {
                    Title = "Pesan",
                    Message = "Terjadi kesalahan saat pembaruan data!",
                });
            }
        }

        public void AddData()
        {
            throw new NotImplementedException();
        }

        public Task UpdateData()
        {
            throw new NotImplementedException();
        }

        public class UserModel
        {
            public string UserId { get; set; }
            public string UserAccessId { get; set; }
            public string Username { get; set; }
            public string Password { get; set; }
            public string Name { get; set; }
            public string LevelAccess { get; set; }
            public string Caption { get; set; }
            public string ActiveStatus { get; set; }
            public string AdministratorLevel { get; set; }
            
            public string LastLoggedIn { get; set; }
        }
    }

    
}
