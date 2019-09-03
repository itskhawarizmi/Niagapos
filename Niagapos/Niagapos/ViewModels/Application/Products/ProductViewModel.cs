using Niagapos.Core;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Input;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;

namespace Niagapos
{
    public class ProductViewModel : BaseViewModel, IDatabaseManager<NiagaposEntities, Product>
    {

        public ProductViewModel()
        {
            if (DesignerProperties.GetIsInDesignMode(new DependencyObject()))
                return;


            ConnectionString = @"data source=ALPHA-IDR1896;initial catalog=Niagapos;integrated security=True;";
          

           Context = new NiagaposEntities();
           InitializeObject();
           CategorySetupComponent();
            ProductItems = new ObservableCollection<Product>();
            ProductListItemHelper = new ObservableCollection<ProductDataModel>();
            SelectedListItemHelper = new ProductDataModel();
            SelectedItems = new Product();
            GetCategories();
            GetSuppliers();
            LoadData();
            SetupCommand();
        }
        

        public string ConnectionString { get; set; }
        public ObservableCollection<ProductDataModel> ProductListItemHelper { get; set; }
        public ProductDataModel SelectedListItemHelper { get; set; }

        public ICommand AddDataCommand { get; set; }
        public ICommand UpdateDataCommand { get; set; }
        public ICommand DeleteDataCommand { get; set; }
        public ICommand RefreshDataCommand { get; set; }
        public ICommand SearchDataCommand { get; set; }
        public ICommand ExportDataCommand { get; set; }

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

        public ICommand ClearSearchHistoryCommand { get; set; }

        public ICommand ClearImagePathCommand { get; set; }

        public Product SelectedItems { get; set; }
        public NiagaposEntities Context { get; set; }
        public ObservableCollection<Product> ProductItems { get; set; }

        public ObservableCollection<ProductSupplier> ProductSupplierItems { get; set; }
        public ProductSupplier GetProductSupplierItem { get; set; }

        public ObservableCollection<ProductCategory> ProductCategoryItem { get; set; }
        public ProductCategory GetProductCategoryItem { get; set; }

        public ObservableCollection<CategoryItemsDataModel> CategoryItemsCollection { get; set; }
        public CategoryItemsDataModel GetCategoryItems { get; set; }

        public ObservableCollection<SupplierItemsDataModel> SupplierItemsCollection { get; set; }
        public SupplierItemsDataModel GetSupplierItems { get; set; }

        public bool Flag { get; set; }

        public bool CanEdit { get; set; }

        public string FilePathImg { get; set; }
        public string FileName { get; set; } = "Laporan Stok.pdf";
        public string FilePath { get; set; } = "D:/WPF Project/Niagapos/Niagapos/Attachments/Product/";

        public Document PdfDocument { get; set; }
        public Rectangle Rect { get; set; }

        public string SearchText { get; set; }
        public string CategoryProduct { get; set; }

        public void SetupCommand()
        {
            AddDataCommand = new RelayParameterizedCommand((parameter) => AddData(parameter));
            DeleteDataCommand = new RelayCommand(() => DeleteData());
            UpdateDataCommand = new RelayCommand(async () => await UpdateData());
            RefreshDataCommand = new RelayCommand(async () => await Refresh());
            SearchDataCommand = new RelayCommand(() => Search());
            ClearSearchHistoryCommand = new RelayCommand(() => ClearSearchHistory());
            ClearImagePathCommand = new RelayCommand(() => ClearImagePath());
            ExportDataCommand = new RelayCommand(() => ExportData());

            EditControlMenuCommand = new RelayCommand(() => CanEditControl());
            CloseEditControlMenuCommand = new RelayCommand(() => CloseEditControl());
            AddControlMenuCommand = new RelayCommand(() => AddProductControl());
            CloseAddControlMenuCommand = new RelayCommand(() => CloseMenu());
            DialogPictureCommand = new RelayCommand(() => DialogPicture());
        }

        public void InitializeObject()
        {
            ProductSupplierItems = new ObservableCollection<ProductSupplier>();
            GetProductSupplierItem = new ProductSupplier();

            ProductCategoryItem = new ObservableCollection<ProductCategory>();
            GetProductCategoryItem = new ProductCategory();

            GetCategory = new ProductCategoryDataModel();
            ProductCategoryItemsHelper = new ObservableCollection<ProductCategoryDataModel>();

            GetSupplier = new ProductSupplierDataModel();
            ProductSupplierItemsHelper = new ObservableCollection<ProductSupplierDataModel>();

            GetCategoryItems = new CategoryItemsDataModel();
            CategoryItemsCollection = new ObservableCollection<CategoryItemsDataModel>();

            GetSupplierItems = new SupplierItemsDataModel();
            SupplierItemsCollection = new ObservableCollection<SupplierItemsDataModel>();

        }

        public void AddProductControl()
        {
            DI.Application.AddProductControlMenuVisible = true;
        }

        public void CloseMenu()
        {
            DI.Application.AddProductControlMenuVisible = false;
        }

        public void CanEditControl() => CanEdit = true;

        public void CloseEditControl() => CanEdit = false;

        public void ClearImagePath()
        {
            if (FilePathImg != null)
                FilePathImg = "";
        }

        public async void AddData(object parameter)
        {
            var getCategoryId = GetCategoryItems.CategoryId;
            var getSupplierId = GetSupplierItems.SupplierId;

            try
            {

                if ((SelectedListItemHelper.ProductId == null && SelectedListItemHelper.CurrentStock == 0 &&
                       SelectedListItemHelper.ProductName == null && SelectedListItemHelper.Description == null &&
                       SelectedListItemHelper.Category == null && SelectedListItemHelper.Price == null &&
                       SelectedListItemHelper.Brand == null && SelectedListItemHelper.Supplier == null && SelectedListItemHelper.ImgUrl == null))
                {
                    await DI.UI.ShowMessage(new MessageBoxDialogViewModel
                    {
                        Title = "Pesan",
                        Message = "Tidak dapat menyimpan data, karna semua field masih kosong!",

                    });

                    return;
                }
                else
                {
                    var products = (from product in Context.Products
                                    where product.ProductId == SelectedListItemHelper.ProductId
                                    select product).ToList();

                    if (products.Count > 0)
                    {
                        await DI.UI.ShowMessage(new MessageBoxDialogViewModel
                        {
                            Title = "Pesan",
                            Message = "User ID ini sudah ada, silakan gunakan ID yang lain!",
                            DialogsYesVisible = false

                        });

                        return;
                    }



                    SelectedItems = new Product
                    {
                        ProductId = SelectedListItemHelper.ProductId,
                        ProductName = SelectedListItemHelper.ProductName,
                        CurrentStock = (int)SelectedListItemHelper.CurrentStock,
                        Price = (int)SelectedListItemHelper.Price,
                        ImgUrl = FilePathImg ?? @"D:/WPF Project/Niagapos/Niagapos/Images/Assets/NotAvailable.jpg",
                        Size = SelectedListItemHelper.Size,
                        Color = SelectedListItemHelper.Color,
                        ProductCategoryId = getCategoryId,
                        ProductSupplierId = getSupplierId,
                        Description = SelectedListItemHelper.Description,

                    };




                    if (SelectedItems != null)
                    {
                        Context.Products.Add(SelectedItems);
                        Context.SaveChanges();
                        await Task.Delay(TimeSpan.FromSeconds(0.1));
                        await DI.UI.ShowMessage(new MessageBoxDialogViewModel
                        {
                            Title = "Pesan",
                            Message = "Data baru telah ditambahkan dan berhasil disimpan!",
                            DialogsYesVisible = false

                        });

                    }


                    await Refresh();
                }
                    


            }
            catch (Exception)
            {
                await DI.UI.ShowMessage(new MessageBoxDialogViewModel
                {
                    Title = "Pesan",
                    Message = "Terjadi kesalahan saat proses penambahan data!",
                    DialogsYesVisible = false


                });
            }
        }

        private async void GetCategories()
        {
            using (SqlConnection Connection = new SqlConnection(ConnectionString))
            {
                using (SqlCommand CommandQuery = new SqlCommand())
                {
                    try
                    {
                        var counter = 0;
                        Connection.Open();
                        CommandQuery.Connection = Connection;
                        CommandQuery.CommandText = "SELECT ProductCategoryId, Category FROM ProductCategory";
                        using (SqlDataReader  reader = CommandQuery.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                CategoryItemsCollection.Add(new CategoryItemsDataModel
                                {
                                    CategoryId = reader["ProductCategoryId"].ToString(),
                                    Category = reader["Category"].ToString(),
                                    Index = counter++,
                                });
                            }
                        }

                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine(ex.Message);

                    }

                    await Task.Delay(TimeSpan.FromSeconds(0.1));
                }
            }
        }


        private async void GetSuppliers()
        {
            using (SqlConnection Connection = new SqlConnection(ConnectionString))
            {
                using (SqlCommand CommandQuery = new SqlCommand())
                {
                    try
                    {
                        var counter = 0;
                        Connection.Open();
                        CommandQuery.Connection = Connection;
                        CommandQuery.CommandText = "SELECT ProductSupplierId, CompanyName FROM ProductSupplier";
                        using (SqlDataReader reader = CommandQuery.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                SupplierItemsCollection.Add(new SupplierItemsDataModel
                                {
                                    SupplierId = reader["ProductSupplierId"].ToString(),
                                    CompanyName = reader["CompanyName"].ToString(),
                                    Index = counter++,
                                });
                            }
                        }

                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine(ex.Message);

                    }

                    await Task.Delay(TimeSpan.FromSeconds(0.1));
                }
            }
        }

        public async void DeleteData()
        {
            try
            {

                if (System.Windows.MessageBox.Show($"Apakah Anda yakin ingin menghapus data ini {SelectedListItemHelper.ProductId} ?", "Message",
                     MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.Yes) == MessageBoxResult.Yes)
                {
                    if (SelectedListItemHelper.Equals(null))
                        return;

                    var query = (from product in Context.Products
                                 where product.ProductId == SelectedListItemHelper.ProductId
                                 select product).FirstOrDefault();


                    if (query.Equals(null))
                        return;

                    else if (query != null)
                    {
                        Context.Products.Remove(query);
                        await Context.SaveChangesAsync();
                        await Refresh();
                        Flag = true;
                    }

                    if (Flag)
                    {

                        await DI.UI.ShowMessage(new MessageBoxDialogViewModel
                        {
                            Title = "Success",
                            Message = "The data has been deleted successfully!",
                            DialogsYesVisible = false
                        });

                    }
                    
                    

                }

                else
                {
                    throw new Exception();
                }

            }
            catch (Exception)
            {
                await DI.UI.ShowMessage(new MessageBoxDialogViewModel
                {
                    Title = "Error",
                    Message = "Opss.. Unable to delete the data!",
                    DialogsYesVisible = false
                });

                Flag = false;
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

        public void LoadData()
        {
            if (!string.IsNullOrEmpty(SearchText))
                SearchText = string.Empty;

            // Don't make any duplicate data in view on Datagrid
            ProductListItemHelper.Clear();

            var query = (from product in Context.Products
                         join category in Context.ProductCategories on product.ProductCategoryId equals category.ProductCategoryId
                         join supplier in Context.ProductSuppliers on product.ProductSupplierId equals supplier.ProductSupplierId
                         orderby product.ProductId
                         select new
                         {
                            product.ProductId,
                            product.ProductName,
                            product.CurrentStock,
                            product.Price,
                            product.Description,
                            product.ImgUrl,
                            product.Size,
                            product.Color,
                            category.Category,
                            supplier.CompanyName,
                            supplier.Brand,
                            
                         }).ToList();

           

            foreach (var product in query)
            {
                ProductListItemHelper.Add(new ProductDataModel
                {
                    ProductId = product.ProductId,
                    ProductName = product.ProductName,
                    CurrentStock = product.CurrentStock,
                    Price = product.Price,
                    Description = product.Description,
                    ImgUrl = product.ImgUrl,
                    Color = product.Color,
                    Size = product.Size,
                    Category = product.Category,
                    Supplier = product.CompanyName,
                    Brand = product.Brand,


                });
            }
            
        }
        

        public async Task Refresh()
        {
            Context.Products.Load();
            LoadData();
            LoadProductCategory();
            LoadProductSupplier();
            await Task.Delay(TimeSpan.FromSeconds(0.1));
        }

        public async void Search()
        {
            if (string.IsNullOrEmpty(SearchText))
                return;

            else
            {

                var query = (from product in Context.Products
                             join category in Context.ProductCategories on product.ProductCategoryId equals category.ProductCategoryId
                             join supplier in Context.ProductSuppliers on product.ProductSupplierId equals supplier.ProductSupplierId
                             where (product.ProductName == SearchText) || (product.ProductName.Contains(SearchText)) ||
                                   (product.ProductId == SearchText) || (product.ProductId.Contains(SearchText)) ||
                                   (category.Category == SearchText) || (supplier.Brand.Contains(SearchText))

                             select new
                             {
                                 product.ProductId,
                                 product.ProductName,
                                 product.CurrentStock,
                                 product.Price,
                                 product.Description,
                                 product.ImgUrl,
                                 product.Size,
                                 product.Color,
                                 category.Category,
                                 supplier.CompanyName,
                                 supplier.Brand,

                             }).ToList();
                
                ProductListItemHelper.Clear();


                foreach (var product in query)
                {
                    ProductListItemHelper.Add(new ProductDataModel
                    {
                        ProductId = product.ProductId,
                        ProductName = product.ProductName,
                        CurrentStock = product.CurrentStock,
                        Price = product.Price,
                        Size = product.Size,
                        Color = product.Color,
                        Description = product.Description,
                        ImgUrl = product.ImgUrl,
                        Category = product.Category,
                        Supplier = product.CompanyName,
                        Brand = product.Brand,
                    });
                    
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
                var query = (from product in Context.Products
                                where product.ProductId == SelectedListItemHelper.ProductId
                                select new
                                {
                                    product.ProductId,
                                    product.ProductName,
                                    product.CurrentStock,
                                    product.Price,
                                    product.Size,
                                    product.Color,
                                    product.Description,
                                    product.ImgUrl,
                                    product.ProductSupplierId,
                                    product.ProductCategoryId

                                }).ToList();

                    foreach(var pro in query)
                    {
                        if (SelectedListItemHelper != null)
                        {

                            using (SqlConnection Connection = new SqlConnection(ConnectionString))
                            {
                                using(SqlCommand CommandQuery = new SqlCommand())
                                {
                                    try
                                    {

                                        Connection.Open();
                                        CommandQuery.Connection = Connection;
                                        CommandQuery.CommandText = "UPDATE Product SET ProductName = @name, CurrentStock = @stock, Price = @price, Size = @size, Color = @color, Description = @description, ImgUrl = @img, ProductSupplierId = @supplierId, ProductCategoryId = @categoryId WHERE ProductId = @id";
                                        CommandQuery.Parameters.AddWithValue("@id", SelectedListItemHelper.ProductId ?? (object)DBNull.Value);
                                        CommandQuery.Parameters.AddWithValue("@name", SelectedListItemHelper.ProductName ?? (object)DBNull.Value);
                                        CommandQuery.Parameters.AddWithValue("@stock", SelectedListItemHelper.CurrentStock);
                                        CommandQuery.Parameters.AddWithValue("@size", SelectedListItemHelper.Size ?? (object)DBNull.Value);
                                        CommandQuery.Parameters.AddWithValue("@color", SelectedListItemHelper.Color ?? (object)DBNull.Value);
                                        CommandQuery.Parameters.AddWithValue("@price", SelectedListItemHelper.Price ?? (object)DBNull.Value);
                                        CommandQuery.Parameters.AddWithValue("@description", SelectedListItemHelper.Description ?? (object)DBNull.Value);
                                        CommandQuery.Parameters.AddWithValue("@img", SelectedListItemHelper.ImgUrl ?? FilePathImg ?? (object)DBNull.Value);
                                        CommandQuery.Parameters.AddWithValue("@categoryId", pro.ProductCategoryId ?? (object)DBNull.Value);
                                        CommandQuery.Parameters.AddWithValue("@supplierId", pro.ProductSupplierId ?? (object)DBNull.Value);
                                        CommandQuery.ExecuteNonQuery();

                                        Context.SaveChanges();

                                        await Task.Delay(TimeSpan.FromSeconds(0.1));

                                        await Refresh();

                                        await DI.UI.ShowMessage(new MessageBoxDialogViewModel
                                        {
                                            Title = "Pesan",
                                            Message = "Data baru telah diperbarui dan berhasil disimpan!",
                                            DialogsYesVisible = false

                                        });


                                    }
                                    catch (Exception ex)
                                    {
                                        await DI.UI.ShowMessage(new MessageBoxDialogViewModel
                                        {
                                            Title = "Pesan",
                                            Message = "Terjadi kesalahan saat proses pembaruan data!",
                                            DialogsYesVisible = false


                                        });

                                       CoreDI.Logger.Log(ex.Message);

                                    }
                                }
                            }
                            
                        }
                    
                }


            }
            catch (Exception)
            {
                await DI.UI.ShowMessage(new MessageBoxDialogViewModel
                {
                    Title = "Pesan",
                    Message = "Terjadi kesalahan saat proses pembaruan data!",
                    DialogsYesVisible = false
                });

                
            }
        }

        
        public ICommand DeleteCategoryCommand { get; set; }
        public ICommand AddCategoryCommand { get; set; }

        public ICommand EditCategoryCommand { get; set; }

        public ICommand DeleteSupplierCommand { get; set; }
        public ICommand AddSupplierCommand { get; set; }

        public ICommand EditSupplierCommand { get; set; }

        public void CategorySetupComponent()
        {
            LoadProductCategory();
            LoadProductSupplier();

            EditCategoryControlCommand = new RelayCommand(() => CanEditCategoryControl());
            CloseEditCategoryControlCommand = new RelayCommand(() => CloseEditCategoryControl());
            AddCategoryControlCommand = new RelayCommand(() => CanAddCategoryControl());
            CloseAddCategoryControlCommand = new RelayCommand(() => CloseAddCategoryControl());

            EditSupplierControlCommand = new RelayCommand(() => CanEditSupplierControl());
            CloseEditSupplierControlCommand = new RelayCommand(() => CloseEditSupplierControl());
            AddSupplierControlCommand = new RelayCommand(() => CanAddSupplierControl());
            CloseAddSupplierControlCommand = new RelayCommand(() => CloseAddSupplierControl());

            DeleteCategoryCommand = new RelayCommand(() => DeleteProductCategory());
            AddCategoryCommand = new RelayCommand(() => AddProductCategory());
            EditCategoryCommand = new RelayCommand(async () => await UpdateProductCategory());
            SearchCategoryCommand = new RelayCommand(() => SearchProductCategory());

            DeleteSupplierCommand = new RelayCommand(() => DeleteProductSupplier());
            AddSupplierCommand = new RelayCommand(() => AddProductSupplier());
            EditSupplierCommand = new RelayCommand(async () => await UpdateProductSupplier());
            SearchSupplierCommand = new RelayCommand(() => SearchProductSupplier());
        }

        public async void LoadProductCategory()
        {


            var query = (from category in Context.ProductCategories
                         orderby category.ProductCategoryId
                         select category).ToList();

            if(query.Count > 0)
            {
                ProductCategoryItem.Clear();

                foreach(var categories in query)
                {
                    ProductCategoryItem.Add(categories);
                }
            }
          

            await Task.Delay(TimeSpan.FromSeconds(0.1));
        }

        public async void AddProductCategory()
        {
            var query = (from category in Context.ProductCategories
                         where category.ProductCategoryId == GetCategory.CategoryId
                         select category).ToList();

            if (GetCategory != null)
            {
                try
                {
                    if (query.Count > 0)
                    {
                        await DI.UI.ShowMessage(new MessageBoxDialogViewModel
                        {
                            Title = "Pesan",
                            Message = "ID Kategori produk sudah ada, silakan gunakan ID yang lain",
                            DialogsYesVisible = false

                        });
                        return;
                    }
                    else
                    {
                        Context.ProductCategories.Add(new ProductCategory
                        {
                            ProductCategoryId = GetCategory.CategoryId,
                            Category = GetCategory.Category,
                            Description = GetCategory.Description,

                        });
                        await Task.Delay(TimeSpan.FromSeconds(0.1));
                        await DI.UI.ShowMessage(new MessageBoxDialogViewModel
                        {
                            Title = "Pesan",
                            Message = "Data baru telah ditambahkan dan berhasil disimpan!",
                            DialogsYesVisible = false

                        });

                        Context.SaveChanges();


                        LoadProductCategory();
                    }


                }
                catch (Exception Ex)
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
        }

        public async Task UpdateProductCategory()
        {
            try
            {
               
                    if ((GetProductCategoryItem != null))
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



                    LoadProductCategory();

                


            }
            catch (Exception)
            {
                await DI.UI.ShowMessage(new MessageBoxDialogViewModel
                {
                    Title = "Pesan",
                    Message = "Terjadi kesalahan saat pembaruan data!",
                    DialogsYesVisible = false
                });
            }


        }

        public async void DeleteProductCategory()
        {

            if (System.Windows.MessageBox.Show("Apakah Anda yakin ingin hapus data ini ?", "Message",
                MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.Yes) == MessageBoxResult.Yes)
            {
                try
                {
                    if (GetProductCategoryItem.Equals(null))
                        return;

                    var query = (from category in Context.ProductCategories
                                 where category.ProductCategoryId == GetProductCategoryItem.ProductCategoryId
                                 select category).FirstOrDefault();


                    if (query.Equals(null))
                        return;

                    else if (query != null)
                    {
                        Context.ProductCategories.Remove(query);
                        await Context.SaveChangesAsync();
                        LoadProductCategory();
                        Flag = true;
                    }

                    if (Flag)
                    {

                        await DI.UI.ShowMessage(new MessageBoxDialogViewModel
                        {
                            Title = "Pesan",
                            Message = "Data berhasil dihapus!",
                            DialogsYesVisible = false
                        });

                    }

                }
                catch (Exception Ex)
                {
                    await DI.UI.ShowMessage(new MessageBoxDialogViewModel
                    {
                        Title = "Pesan",
                        Message = "Terjadi kesalahan saat hapus data!",
                        DialogsYesVisible = false
                    });

                    CoreDI.Logger.Log(Ex.Message);
                }
             

            }
        }

        public async void SearchProductCategory()
        {
            if (SearchTextSupplier != string.Empty)
            {
                try
                {
                    var categories = (from category in Context.ProductCategories
                                      where category.ProductCategoryId.Contains(SearchTextCategory) ||
                                      SearchTextCategory.Contains(category.ProductCategoryId)
                                      select category).ToList();

                    if (categories.Count > 0)
                    {
                        ProductCategoryItem.Clear();

                        foreach (var category in categories)
                        {
                            ProductCategoryItem.Add(category);
                        }
                        
                    }
                    else
                    {
                        await DI.UI.ShowMessage(new MessageBoxDialogViewModel
                        {
                            Title = "Pesan",
                            Message = $"Data {SearchTextCategory} tidak ditemukan di dalam database!",
                        });
                    }

                }
                catch (Exception Ex)
                {
                    await DI.UI.ShowMessage(new MessageBoxDialogViewModel
                    {
                        Title = "Pesan",
                        Message = "Terjadi kesalahan saat proses pencarian data",
                    });

                    CoreDI.Logger.Log(Ex.Message);
                }
            }

        }



        public void CanEditCategoryControl() => CanEditCategory = true;

        public void CloseEditCategoryControl() => CanEditCategory = false;

        public void CanAddCategoryControl() => CanAddCategory = true;

        public void CloseAddCategoryControl() => CanAddCategory = false;

        public void CanEditSupplierControl() => CanEditSupplier = true;

        public void CloseEditSupplierControl() => CanEditSupplier = false;
        public void CanAddSupplierControl() => CanAddSupplier = true;

        public void CloseAddSupplierControl() => CanAddSupplier = false;

        /// Show edit menu control to visible
        /// </summary>
        public ICommand EditCategoryControlCommand { get; set; }

        /// <summary>
        /// Close edit menu control or hide the menu
        /// </summary>
        public ICommand CloseEditCategoryControlCommand { get; set; }

        /// <summary>
        /// Show Add menu control to visible
        /// </summary>
        public ICommand AddCategoryControlCommand { get; set; }

        /// <summary>
        /// Close add menu control or hide the menu
        /// </summary>
        public ICommand CloseAddCategoryControlCommand { get; set; }




        /// Show edit menu control to visible
        /// </summary>
        public ICommand EditSupplierControlCommand { get; set; }

        /// <summary>
        /// Close edit menu control or hide the menu
        /// </summary>
        public ICommand CloseEditSupplierControlCommand { get; set; }

        /// <summary>
        /// Show Add menu control to visible
        /// </summary>
        public ICommand AddSupplierControlCommand { get; set; }

        public ICommand SearchCategoryCommand { get; set; }
        public ICommand SearchSupplierCommand { get; set; }

        /// <summary>
        /// Close add menu control or hide the menu
        /// </summary>
        public ICommand CloseAddSupplierControlCommand { get; set; }


        public bool CanAddCategory { get; set; }

        public bool CanEditCategory { get; set; }


        public bool CanAddSupplier { get; set; }

        public bool CanEditSupplier { get; set; }

        public string SearchTextCategory { get; set; }
        public string SearchTextSupplier { get; set; }
        public ProductCategoryDataModel GetCategory { get; set; }
        public ObservableCollection<ProductCategoryDataModel> ProductCategoryItemsHelper { get; set; }

        public ProductSupplierDataModel GetSupplier { get; set; }
        public ObservableCollection<ProductSupplierDataModel> ProductSupplierItemsHelper { get; set; }


        public async void LoadProductSupplier()
        {


            var query = (from supplier in Context.ProductSuppliers
                         orderby supplier.ProductSupplierId
                         select supplier).ToList();

            if (query.Count > 0)
            {
                ProductSupplierItems.Clear();

                foreach (var suppliers in query)
                {
                    ProductSupplierItems.Add(suppliers);
                }
            }
           

            await Task.Delay(TimeSpan.FromSeconds(0.1));
        }

        public async void AddProductSupplier()
        {
            var query = (from supplier in Context.ProductSuppliers
                         where supplier.ProductSupplierId == GetSupplier.ProductSupplierId
                         select supplier).ToList();

            if (GetSupplier != null)
            {
                try
                {
                    if (query.Count > 0)
                    {
                        await DI.UI.ShowMessage(new MessageBoxDialogViewModel
                        {
                            Title = "Pesan",
                            Message = "ID supplier produk sudah ada, silakan gunakan ID yang lain",
                            DialogsYesVisible = false

                        });
                        return;
                    }
                    else
                    {
                        Context.ProductSuppliers.Add(new ProductSupplier
                        {
                            ProductSupplierId = GetSupplier.ProductSupplierId,
                            CompanyName = GetSupplier.CompanyName,
                            Brand = GetSupplier.Brand,
                            Address = GetSupplier.Address,
                            Email = GetSupplier.Email,
                            PhoneNumber = GetSupplier.PhoneNumber,

                        });
                        await Task.Delay(TimeSpan.FromSeconds(0.1));
                        await DI.UI.ShowMessage(new MessageBoxDialogViewModel
                        {
                            Title = "Pesan",
                            Message = "Data baru telah ditambahkan dan berhasil disimpan!",
                            DialogsYesVisible = false

                        });

                        Context.SaveChanges();


                        LoadProductSupplier();
                    }


                }
                catch (Exception Ex)
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
        }

        public async Task UpdateProductSupplier()
        {
            try
            {
                if ((GetProductSupplierItem != null))
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



                LoadProductSupplier();
            
                


            }
            catch (Exception)
            {
                await DI.UI.ShowMessage(new MessageBoxDialogViewModel
                {
                    Title = "Pesan",
                    Message = "Terjadi kesalahan saat pembaruan data!",
                    DialogsYesVisible = false
                });
            }


        }

        public async void DeleteProductSupplier()
        {

            if (System.Windows.MessageBox.Show("Apakah Anda yakin ingin hapus data ini ?", "Message",
                MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.Yes) == MessageBoxResult.Yes)
            {
                try
                {
                    if (GetProductSupplierItem.Equals(null))
                        return;

                    var query = (from supplier in Context.ProductSuppliers
                                 where supplier.ProductSupplierId == GetProductSupplierItem.ProductSupplierId
                                 select supplier).FirstOrDefault();


                    if (query.Equals(null))
                        return;

                    else if (query != null)
                    {
                        Context.ProductSuppliers.Remove(query);
                        await Context.SaveChangesAsync();
                        LoadProductSupplier();
                        Flag = true;
                    }

                    if (Flag)
                    {

                        await DI.UI.ShowMessage(new MessageBoxDialogViewModel
                        {
                            Title = "Pesan",
                            Message = "Data berhasil dihapus!",
                            DialogsYesVisible = false
                        });

                    }

                }
                catch (Exception Ex)
                {
                    await DI.UI.ShowMessage(new MessageBoxDialogViewModel
                    {
                        Title = "Pesan",
                        Message = "Terjadi kesalahan saat hapus data!",
                        DialogsYesVisible = false
                    });

                    CoreDI.Logger.Log(Ex.Message);
                }


            }
        }

        public async void SearchProductSupplier()
        {
            if(SearchTextSupplier != string.Empty)
            {
                try
                {
                    var suppliers = (from supplier in Context.ProductSuppliers
                                     where supplier.ProductSupplierId.Contains(SearchTextSupplier)
                                     select supplier).ToList();

                    if (suppliers.Count > 0)
                    {
                        ProductSupplierItems.Clear();

                        foreach(var sup in suppliers)
                        {
                            ProductSupplierItems.Add(sup);
                        }
                        
                    }
                    else
                    {
                        await DI.UI.ShowMessage(new MessageBoxDialogViewModel
                        {
                            Title = "Pesan",
                            Message = $"Data {SearchTextSupplier} tidak ditemukan di dalam database!",
                        });
                    }

                }
                catch(Exception Ex)
                {
                    await DI.UI.ShowMessage(new MessageBoxDialogViewModel
                    {
                        Title = "Pesan",
                        Message = "Terjadi kesalahan saat proses pencarian data",
                    });

                    CoreDI.Logger.Log(Ex.Message);
                }
            }


        }

        public void AddData()
        {
            throw new NotImplementedException();
        }

        public async void ExportData()
        {
            if(ProductListItemHelper == null)
            {
                await DI.UI.ShowMessage(new MessageBoxDialogViewModel
                {
                    Title = "Pesan",
                    Message = "Maaf, Sistem tidak menemukan data apapun tentang produk di dalam database!"
                });
            }
            else
            {
                try
                {
                    if (System.Windows.MessageBox.Show($"Apakah Anda yakin ingin mencetak laporan stok produk ?", "Message",
                         MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.Yes) == MessageBoxResult.Yes)
                    {


                        var rect = new Rectangle(190, 350);

                        PdfDocument = new Document(PageSize.A4);

                        var writer = PdfWriter.GetInstance(PdfDocument, new FileStream(FilePath + FileName, FileMode.Create));
                        var font = new Font(Font.FontFamily.COURIER, 10f, 0, new BaseColor(System.Drawing.ColorTranslator.FromHtml("#111")));
                        var fontHeader = new Font(Font.FontFamily.TIMES_ROMAN, 10f, 1, new BaseColor(System.Drawing.ColorTranslator.FromHtml("#fff")));
                        PdfDocument.Open();
                        var transactionInfo = default(string);
                        var date = DateTime.Now.ToString("yyyy/MM/dd");
                        var time = DateTime.Now.ToString("hh:mm:dd") + " WIB";


                        transactionInfo =
        $@"
Nama         : Laporan Stok Produk
Tanggal      : {date}
Jam          : {time}
_______________________________________________________________________________________    
";
                        var paragraph = new Paragraph(transactionInfo, font)
                        {
                            Alignment = Element.ALIGN_LEFT
                        };

                        PdfDocument.Add(paragraph);

                        var table = new PdfPTable(8)
                        {
                            TotalWidth = 350f,
                            WidthPercentage = 100f,
                            SpacingBefore = 5f,

                        };
                        var totalStock = default(string);

                        var cellNo = new PdfPCell(new Phrase("No.", fontHeader))
                        {
                            BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#498ccd")),
                            Border = 3,
                            BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#498ccd")),
                        };

                        var cellId = new PdfPCell(new Phrase("ID", fontHeader))
                        {
                            BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#498ccd")),
                            Border = 1,
                            BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#498ccd")),
                        };

                        var cellName = new PdfPCell(new Phrase("Nama", fontHeader))
                        {
                            BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#498ccd")),
                            Border = 1,
                            BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#498ccd")),
                        };


                        var cellCategory = new PdfPCell(new Phrase("Kategori", fontHeader))
                        {
                            BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#498ccd")),
                            Border = 1,
                            BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#498ccd")),
                        };

                        var cellSupplier = new PdfPCell(new Phrase("Supplier", fontHeader))
                        {
                            BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#498ccd")),
                            Border = 1,
                            BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#498ccd")),
                        };

                        var cellBrand = new PdfPCell(new Phrase("Merek", fontHeader))
                        {
                            BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#498ccd")),
                            Border = 1,
                            BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#498ccd")),
                        };


                        var cellSize = new PdfPCell(new Phrase("Ukuran", fontHeader))
                        {
                            BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#498ccd")),
                            Border = 1,
                            BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#498ccd")),
                        };

                        var cellColor = new PdfPCell(new Phrase("Warna", fontHeader))
                        {
                            BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#498ccd")),
                            Border = 1,
                            BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#498ccd")),
                        };

                        var cellStock = new PdfPCell(new Phrase("Stok", fontHeader))
                        {
                            BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#498ccd")),
                            Border = 1,
                            BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#498ccd")),
                        };

                        var cellPrice = new PdfPCell(new Phrase("Harga", fontHeader))
                        {
                            BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#498ccd")),
                            Border = 1,
                            BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#498ccd")),
                        };




                        table.AddCell(cellNo);
                        table.AddCell(cellId);
                        table.AddCell(cellName);
                        table.AddCell(cellCategory);
                        table.AddCell(cellSupplier);
                        table.AddCell(cellBrand);
                        table.AddCell(cellStock);
                        table.AddCell(cellPrice);


                        if (ProductListItemHelper != null)
                        {
                            var counter = 1;
                            totalStock = ProductListItemHelper.Sum(s => s.CurrentStock).ToString();
                            foreach (var product in ProductListItemHelper)
                            {
                                table.AddCell(new Phrase(counter.ToString(), font));
                                table.AddCell(new Phrase(product.ProductId, font));
                                table.AddCell(new Phrase(product.ProductName, font));
                                table.AddCell(new Phrase(product.Category, font));
                                table.AddCell(new Phrase(product.Supplier, font));
                                table.AddCell(new Phrase(product.Brand, font));
                                table.AddCell(new Phrase(product.CurrentStock.ToString(), font));
                                table.AddCell(new Phrase(product.Price?.ToString(string.Format("C0")), font));
                                counter++;

                            }

                            PdfDocument.Add(table);

                        }



                        transactionInfo = $@"Total Stok   : {totalStock} pcs";
                        var stock = new Paragraph(transactionInfo, font)
                        {
                            Alignment = Element.ALIGN_LEFT
                        };

                        PdfDocument.Add(stock);

                        await DI.UI.ShowMessage(new MessageBoxDialogViewModel
                        {
                            Title = "Pesan",
                            Message = "Laporan Stok Produk Berhasil di Cetak!"
                        });

                        Process.Start(FilePath + FileName);

                        await Task.Delay(TimeSpan.FromSeconds(0.1));


                    }
                }
                catch (Exception Ex)
                {
                    CoreDI.Logger.Log(Ex.Message);
                }
                finally
                {
                    PdfDocument.Close();

                }
            }
           
            
        }

        Task IDatabaseManager<NiagaposEntities, Product>.UpdateData()
        {
            throw new NotImplementedException();
        }

        Task IDatabaseManager<NiagaposEntities, Product>.Refresh()
        {
            throw new NotImplementedException();
        }
        #region Class Helper

        /// <summary>
        /// A class that implement data model for product
        /// </summary>
        public class ProductDataModel
        {
            public string ProductId { get; set; }
            public string ProductName { get; set; }
            public int CurrentStock { get; set; }
            public string Size { get; set; }
            public string Color { get; set; }
            public decimal? Price { get; set; }
            public string Category { get; set; }

            public string Supplier { get; set; }

            public string Brand { get; set; }

            public string Description { get; set; }

            public string ImgUrl { get; set; }

        }

        public class ProductCategoryDataModel
        {

            public string CategoryId { get; set; }
            public string Category { get; set; }
            public string Description { get; set; }
        }

        public class SupplierItemsDataModel
        {
            public int Index { get; set; }
            public string SupplierId { get; set; }
            public string CompanyName { get; set; }

        }

        public class CategoryItemsDataModel
        {
            public int Index { get; set; }
            public string CategoryId { get; set; }
            public string Category { get; set; }

        }
        public class ProductSupplierDataModel
        {
            public string ProductSupplierId { get; set; }
            public string CompanyName { get; set; }
            public string Brand { get; set; }
            public string Address { get; set; }

            public string Size { get; set; }
            public string Email { get; set; }
            public string PhoneNumber { get; set; }
        }

        #endregion

    }

}
