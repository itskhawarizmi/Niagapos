using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Input;
using System.Linq;
using System.Data.Entity;
using System.Windows;
using Niagapos.Core;
using System.ComponentModel;

namespace Niagapos
{
    public class CustomerViewModel : BaseViewModel, IDatabaseManager<NiagaposEntities, Customer>
    {
        public CustomerViewModel()
        {
            if (DesignerProperties.GetIsInDesignMode(new DependencyObject()))
                return;

            Context = new NiagaposEntities();
            CustomersItem = new ObservableCollection<Customer>();
            SelectedItems = new Customer();
 
            LoadData();

            SetupCommand();
        }


        public Customer SelectedItems { get; set; }
        public NiagaposEntities Context { get; set; }
        public ObservableCollection<Customer> CustomersItem { get; set; }

        public bool GenderSet { get; set; } = false;

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

        public bool Flag { get; set; }

        public bool CanEdit { get; set; }

        public string FilePathImg { get; set; }

        public string SearchText { get; set; }
        public string Gender { get; set; }
        public ICommand ClearSearchHistoryCommand { get; set; }

        public void SetupCommand()
        {
            AddDataCommand = new RelayCommand(() => AddData());
            DeleteDataCommand = new RelayCommand(() => DeleteData());
            UpdateDataCommand = new RelayCommand(async () => await UpdateData());
            RefreshDataCommand = new RelayCommand(async () => await Refresh());
            SearchDataCommand = new RelayCommand(() => Search());
            ClearSearchHistoryCommand = new RelayCommand(() => ClearSearchHistory());

            EditControlMenuCommand = new RelayCommand(() => CanEditControl());
            CloseEditControlMenuCommand = new RelayCommand(() => CloseEditControl());
            AddControlMenuCommand = new RelayCommand(() => AddCustomersControl());
            CloseAddControlMenuCommand = new RelayCommand(() => CloseMenu());
            DialogPictureCommand = new RelayCommand(() => DialogPicture());
        }
        

        public void AddCustomersControl()
        {
            DI.Application.AddCustomerControlMenuVisible = true;
        }

        public void CloseMenu()
        {
            DI.Application.AddCustomerControlMenuVisible = false;
        }

        public void CanEditControl() => CanEdit = true;

        public void CloseEditControl() => CanEdit = false;

        public async void AddData()
        {
           
            var query = (from customer in Context.Customers
                where customer.CustomerId == SelectedItems.CustomerId
                select customer).ToList();

            if (SelectedItems.Name != null && SelectedItems.CustomerId != null &&
                SelectedItems.Gender != null && SelectedItems.Address != null &&
                SelectedItems.Email != null && SelectedItems.PhoneNumber != null)
            {
                try
                {
                    if (query.Count >= 1)
                    {
                        await DI.UI.ShowMessage(new MessageBoxDialogViewModel
                        {
                            Title = "Pesan",
                            Message = "ID ini sudah ada, silakan gunakan ID yang lain",
                            DialogsYesVisible = false

                        });
                        return;
                    }
                    else if(query.Count < 1)
                    {
                        Context.Customers.Add(SelectedItems);
                        Context.SaveChanges();
                        await Task.Delay(TimeSpan.FromSeconds(0.1));
                        await DI.UI.ShowMessage(new MessageBoxDialogViewModel
                        {
                            Title = "Pesan",
                            Message = "Data baru telah ditambahkan dan berhasil disimpan!",
                            DialogsYesVisible = false

                        });
                        


                        await Refresh();
                    }
                    else
                    {
                        await DI.UI.ShowMessage(new MessageBoxDialogViewModel
                        {
                            Title = "Message",
                            Message = "Opss telah terjadi kesalahan saat tambah data",
                            DialogsYesVisible = false


                        });
                    }

                   
                }
                catch(Exception Ex)
                {
                    await DI.UI.ShowMessage(new MessageBoxDialogViewModel
                    {
                        Title = "Message",
                        Message = "Opss telah terjadi kesalahan saat tambah data",
                        DialogsYesVisible = false


                    });

                    CoreDI.Logger.Log(Ex.Message);
                }
                
            }
            else if(SelectedItems.Name == null && SelectedItems.CustomerId == null &&
                    SelectedItems.Gender == null && SelectedItems.Address == null &&
                    SelectedItems.Email == null && SelectedItems.PhoneNumber == null)
            {
                await DI.UI.ShowMessage(new MessageBoxDialogViewModel
                {
                    Title = "Message",
                    Message = "Tidak dapat menyimpan data, karna semua field masih kosong",
                    DialogsYesVisible = false


                });

            }



        }

        public async void DeleteData()
        {

            if (System.Windows.MessageBox.Show($"Apakah Anda yakin ingin menghapus data ini {SelectedItems.CustomerId} ?", "Message",
                MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.Yes) == MessageBoxResult.Yes)
            {

                try
                {
                    
                    if (SelectedItems.CustomerId == null || SelectedItems.CustomerId == "")
                    {
                        await DI.UI.ShowMessage(new MessageBoxDialogViewModel
                        {
                            Title = "Pesan",
                            Message = "Silakan pilih data yang ingin dihapus",
                            DialogsYesVisible = false
                        });
                        return;

                    }

                    var query = (from customer in Context.Customers
                                 where customer.CustomerId == SelectedItems.CustomerId
                                 select customer).SingleOrDefault();


                    if (query != null)
                    {
                        Context.Customers.Remove(query);
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

            CustomersItem.Clear();

            var query = (from customer in Context.Customers
                        orderby customer.CustomerId
                        select customer).ToList();

            foreach(var user in query)
            {
                CustomersItem.Add(user);
            }

            await Task.Delay(TimeSpan.FromSeconds(0.1));

        }

        public async Task Refresh()
        {
            Context.Customers.Load();
            LoadData();
            await Task.Delay(TimeSpan.FromSeconds(0.1));
        }

        public async void Search()
        {
            if (string.IsNullOrEmpty(SearchText))
                return;

            else
            {

                var query = (from customer in Context.Customers
                             where (customer.Name == SearchText) || (customer.Name.Contains(SearchText)) ||
                                   (customer.CustomerId == SearchText) || (customer.CustomerId.Contains(SearchText))
                             select customer).ToList();

                if(query.Count > 0)
                {
                    CustomersItem.Clear();


                    foreach (var v in query.ToList())
                        CustomersItem.Add(v);
                }
                else
                {
                    await DI.UI.ShowMessage(new MessageBoxDialogViewModel
                    {
                        Title = "Pesan",
                        Message = $"Data {SearchText} tidak ditemukan di dalam database",
                        DialogsYesVisible = false
                    });
                    return;
                }

                

                await Task.Delay(TimeSpan.FromSeconds(0.1));
            }
        }

        public void ClearSearchHistory()
        {
            if (!string.IsNullOrEmpty(SearchText))
                SearchText = string.Empty;
        }


        public async Task UpdateData()
        {
            try
            {
                if ((SelectedItems != null))
                {
                    await Context.SaveChangesAsync();
                    await Task.Delay(TimeSpan.FromSeconds(0.1));

                    await DI.UI.ShowMessage(new MessageBoxDialogViewModel
                    {
                        Title = "Pesan",
                        Message = "Data telah diperbarui dan berhasil disimpan",
                        DialogsYesVisible = false

                    });
                }



                await Refresh();




            }
            catch (Exception)
            {
                await DI.UI.ShowMessage(new MessageBoxDialogViewModel
                {
                    Title = "Error",
                    Message = "Terjadi kesalahan saat pembaruan data!",
                    DialogsYesVisible = false
                });
            }
        }
            
        

    }

    
}
