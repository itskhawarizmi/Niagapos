/************************************
 * Author Name : Muhammad Hari      *
 * Author URI  : www.icodewizard.com  *
 ************************************/

using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Linq;
using System.Windows;
using System.ComponentModel;
using iTextSharp.text.pdf;
using iTextSharp.text;
using System.IO;
using System.Diagnostics;
using System.Text;
using Niagapos.Core;

namespace Niagapos
{
    public class TransactionViewModel : BaseViewModel
    {
        public TransactionViewModel()
        {
            if (DesignerProperties.GetIsInDesignMode(new DependencyObject()))
                return;

            InitializeCollection();
            LoadTransactionInfo();
            SetupCommand();
            LoadProductListItem();
        
        }

        #region Public Properties
        public ObservableCollection<ProductListItemModel> ProductListItemsCollection { get; set; }
        public ProductListItemModel GetProductListItemModel { get; set; }

        public ObservableCollection<OrderListItemModel> OrderListItemsCollection { get; set; }
        public OrderListItemModel GetOrderListItemModel { get; set; }

        public ObservableCollection<ConfirmOrderListItemModel> ConfirmOrderListItemsCollection { get; set; }
        public ConfirmOrderListItemModel GetConfirmOrderListItemModel { get; set; }


        public ObservableCollection<TransactionDetailViewModel> TransactionListItemsCollection { get; set; }
        public TransactionDetailViewModel GetTransactionListItemsModel { get; set; }

        public ObservableCollection<Transaction> TransactionModelCollection { get; set; }
        public Transaction GetTransactionModel { get; set; }

        public ObservableCollection<TransactionDetail> TransactionDetailModelCollection { get; set; }
        public TransactionDetail GetTransactionDetailModel { get; set; }

        NiagaposEntities Context { get; set; }
        public string TouchPadNumber { get; set; }
        public string SearchText { get; set; } = "";
        public decimal TotalPay { get; set; } = 0;
        public decimal Change { get; set; } = 0;

        public decimal Cash { get; set; } = 0;
        public int TotalQuantity { get; set; } = 0;

        public bool IsDataHasBeenLoaded { get; set; } = false;
        public bool IsListProductsControlMenuVisible { get; set; } = false;
        public bool IsConfirmOrderControlMenuVisible { get; set; } = false;
        public bool IsOrderProcessControlMenuVisible { get; set; } = false;

        public string CustomerId { get; set; }
        public string Address { get; set; } = "-";
        public string PhoneNumber { get; set; } = "-";
        public string Name { get; set; } = "Umum";
        public string Bill { get; set; } = "TO1901001";
        public string TransactionDetailId { get; set; }
        public string TransactionId { get; set; }
        public DateTime DateAndTime { get; set; } = DateTime.Now;
        public string User { get; set; } = "Administrator";
        public string Note { get; set; } = "Tidak ada catatan transaksi";

        public Document PdfDocument { get; set; }
        public Rectangle Rect { get; set; }

        public string FileName { get; set; } = "Struk transaksi.pdf";
        public string FilePath { get; set; } = "D:/WPF Project/Niagapos/Niagapos/Attachments/Transaction/";


        #endregion

        #region Public Command
        public ICommand ListProductsControlMenuCommand { get; set; }
        public ICommand AddOrderListItemCommand { get; set; }
        public ICommand DeleteConfirmOrderItemsCommand { get; set; }
        public ICommand ConfirmOrderControlMenuCommand { get; set; }
        public ICommand SearchProductCommand { get; set; }
        public ICommand SearchProductByDirectOrderCommand { get; set; }
        public ICommand ClearSearchTextHistoryCommand { get; set; }
        public ICommand OrderControlMenuCommand { get; set; }
        public ICommand OrderProcessControlMenuCommand { get; set; }
        public ICommand ProcessAndPrintTOCommand { get; set; }
        public ICommand DataLoadedCommand { get; set; }
        public ICommand TouchPadNumberCommand { get; set; }
        public ICommand TouchPadOperationCommand { get; set; }

        public ICommand LoadCustomerInfoCommand { get; set; }

        #endregion

        public void InitializeCollection()
        {
            OrderListItemsCollection = new ObservableCollection<OrderListItemModel>();
            Context = new NiagaposEntities();
            GetOrderListItemModel = new OrderListItemModel();


            ProductListItemsCollection = new ObservableCollection<ProductListItemModel>();
            GetProductListItemModel = new ProductListItemModel();

            ConfirmOrderListItemsCollection = new ObservableCollection<ConfirmOrderListItemModel>();
            GetConfirmOrderListItemModel = new ConfirmOrderListItemModel();

            TransactionListItemsCollection = new ObservableCollection<TransactionDetailViewModel>();
            GetTransactionListItemsModel = new TransactionDetailViewModel();

            TransactionModelCollection = new ObservableCollection<Transaction>();
            GetTransactionModel = new Transaction();

            TransactionDetailModelCollection = new ObservableCollection<TransactionDetail>();
            GetTransactionDetailModel = new TransactionDetail();
        }

        public void SetupCommand()
        {

            TouchPadNumberCommand = new DelegateCommand<string>(TouchPadNumbers);
            TouchPadOperationCommand = new DelegateCommand<string>(TouchPadOperation);
            ListProductsControlMenuCommand = new RelayParameterizedCommand((parameter) => ListProductsControlVisibility(parameter));
            ConfirmOrderControlMenuCommand = new RelayParameterizedCommand((parameter) => ConfirmOrderControlVisibility(parameter));
            OrderProcessControlMenuCommand = new RelayParameterizedCommand((parameter) => OrderProcessControlVisibility(parameter));
            SearchProductByDirectOrderCommand = new RelayCommand(() => SearchDirectProductAddToConfirmOrder());
            ClearSearchTextHistoryCommand = new RelayCommand(() => ClearSearchTextHistory());
            AddOrderListItemCommand = new RelayCommand(() => AddConfrimOrderToOrderListItem());
            DeleteConfirmOrderItemsCommand = new RelayCommand(() => DeleteConfirmOrderItems());
            ProcessAndPrintTOCommand = new RelayCommand(() => ProcessAndPrintTransactionOrder());

            SearchProductCommand = new RelayCommand(() => SearchProduct());
            DataLoadedCommand = new RelayCommand(() => LoadProductListItem());
            LoadCustomerInfoCommand = new RelayCommand(() => LoadCustomerInfo());
            

        }
        
        public void ListProductsControlVisibility(object parameter)
        {
            var statusControl = parameter.ToString().Trim().ToUpper();

            switch (statusControl)
            {
                case "OPEN":
                    IsListProductsControlMenuVisible = true;
                    break;

                case "CLOSE":
                    IsListProductsControlMenuVisible = false;
                    break;

                default:
                    break;


            }



        }

        public void ConfirmOrderControlVisibility(object parameter)
        {
            var statusControl = parameter.ToString().Trim().ToUpper();

            switch (statusControl)
            {
                case "OPEN":
                    IsConfirmOrderControlMenuVisible = true;
                    AddProductToConfrimOrderListItem();
                    break;

                case "CLOSE":
                    IsConfirmOrderControlMenuVisible = false;
                    break;

                default:
                    break;


            }



        }

        public void OrderProcessControlVisibility(object parameter)
        {
            var statusControl = parameter.ToString().Trim().ToUpper();

            switch (statusControl)
            {
                case "OPEN":
                    IsOrderProcessControlMenuVisible = true;
                    break;

                case "CLOSE":
                    IsOrderProcessControlMenuVisible = false;
                    break;

                default:
                    break;


            }
        }

        public void TouchPadOperation(string action)
        {
            switch (action)
            {
                case ("Del"):
                    if (TouchPadNumber.Length > 1)
                        TouchPadNumber = TouchPadNumber.Substring(0, TouchPadNumber.Length - 1);
                    else
                        TouchPadNumber = string.Empty;
                    break;

                case ("C"):
                    if (String.IsNullOrEmpty(TouchPadNumber))
                        TouchPadNumber = String.Empty;
                    else
                        TouchPadNumber = String.Empty;
                    break;


                case ("."):
                    if (string.IsNullOrEmpty(TouchPadNumber))
                        TouchPadNumber += "0,";
                    else
                        TouchPadNumber += ",";
                    break;

                case ("+/-"):
                    if (TouchPadNumber.Contains("-") || TouchPadNumber.Contains("0,"))
                        TouchPadNumber = TouchPadNumber.Remove(TouchPadNumber.IndexOf("-"), 1);
                    else
                        TouchPadNumber = "-" + TouchPadNumber;
                    break;



            }
        }

        public void TouchPadNumbers(string action)
        {
            switch (action)
            {
                case ("0"):
                    TouchPadNumber += "0";
                    break;
                case ("1"):
                    TouchPadNumber += "1";
                    break;
                case ("2"):
                    TouchPadNumber += "2";
                    break;
                case ("3"):
                    TouchPadNumber += "3";
                    break;
                case ("4"):
                    TouchPadNumber += "4";
                    break;
                case ("5"):
                    TouchPadNumber += "5";
                    break;
                case ("6"):
                    TouchPadNumber += "6";
                    break;
                case ("7"):
                    TouchPadNumber += "7";
                    break;
                case ("8"):
                    TouchPadNumber += "8";
                    break;
                case ("9"):
                    TouchPadNumber += "9";
                    break;

            }
        }

        public void ClearSearchTextHistory()
        {
            if (SearchText == String.Empty || SearchText == "")
            {
                DI.UI.ShowMessage(new MessageBoxDialogViewModel
                {
                    Title = "Message",
                    Message = "Tidak ada data yang harus dibersihkan"
                });

                return;
            }

            SearchText = string.Empty;
        }
        
        public void SearchProduct()
        {
            if (SearchText == "")
            {
                DI.UI.ShowMessage(new MessageBoxDialogViewModel
                {
                    Title = "Message",
                    Message = "Data kosong!, Masukan ID, nama, kategori, atau brand produk untuk proses pencarian"
                });

                return;
            }

            try
            {
                var query = (from product in Context.Products
                             join category in Context.ProductCategories on product.ProductCategoryId equals category.ProductCategoryId
                             join supplier in Context.ProductSuppliers on product.ProductSupplierId equals supplier.ProductSupplierId
                             where product.ProductId.Contains(SearchText.Trim()) || product.ProductName.Contains(SearchText.Trim()) || category.Category.Contains(SearchText.Trim()) ||
                                   supplier.Brand.Contains(SearchText.Trim())
                             orderby product.ProductId
                             select new
                             {
                                 product.ProductId,
                                 product.ProductName,
                                 category.Category,
                                 supplier.Brand,
                                 product.CurrentStock,
                                 product.Price,
                                 product.Description,
                                 product.ImgUrl,

                             }).ToList();



                if (query != null)
                {

                    if(query.Count < 1)
                    {

                        throw new Exception();
                    }
                    else
                    {
                        ProductListItemsCollection.Clear();

                        foreach (var pro in query)
                        {
                            ProductListItemsCollection.Add(new ProductListItemModel
                            {
                                ProductId = pro.ProductId,
                                ProductName = pro.ProductName,
                                ProductCategory = pro.Category,
                                Brand = pro.Brand,
                                CurrentStock = pro.CurrentStock,
                                Price = pro.Price ?? 0,
                                Description = pro.Description,
                                ImgUrl = pro.ImgUrl,

                            });
                        }


                    }
                }
                else
                {
                    throw new Exception();
                }
            }

            catch(Exception Ex)
            {
                DI.UI.ShowMessage(new MessageBoxDialogViewModel
                {
                    Title = "Message",
                    Message = $"Sistem tidak menemukan data '{SearchText}' di dalam database!"
                });

                CoreDI.Logger.Log(Ex.Message);
            }

            



        }

        public void SearchDirectProductAddToConfirmOrder()
        {
            if (SearchText == null || SearchText == "")
            {
                DI.UI.ShowMessage(new MessageBoxDialogViewModel
                {
                    Title = "Message",
                    Message = "Data kosong!, masukan ID produk untuk proses pencarian"
                });

                return;

            }
            

            try
            {

                var query = (from product in Context.Products
                             join category in Context.ProductCategories on product.ProductCategoryId equals category.ProductCategoryId
                             join supplier in Context.ProductSuppliers on product.ProductSupplierId equals supplier.ProductSupplierId
                             where product.ProductId == SearchText
                             select new
                             {
                                 product.ProductId,
                                 product.ProductName,
                                 category.Category,
                                 supplier.Brand,
                                 product.CurrentStock,
                                 product.Price,
                                 product.Description,
                                 product.ImgUrl,

                             }).ToList();

                if (query != null)
                {
                    foreach (var pro in query)
                    {
                        if ((pro.ProductId == SearchText) || (pro.ProductName == SearchText))
                        {
                            ConfirmOrderListItemsCollection.Add(new ConfirmOrderListItemModel
                            {
                                ProductId = GetProductListItemModel.ProductId,
                                ProductName = GetProductListItemModel.ProductName,
                                ProductCategory = GetProductListItemModel.ProductCategory,
                                Brand = GetProductListItemModel.Brand,
                                Price = GetProductListItemModel.Price,
                                ImgUrl = GetProductListItemModel.ImgUrl,


                            });

                            IsConfirmOrderControlMenuVisible = true;
                        }
                     
                        else
                        {
                            throw new Exception();
                        }
                    }

                    if(query.Count < 1)
                    {
                        throw new Exception();
                    }

                    ClearSearchTextHistory();
                   
                }
            }
            catch(Exception Ex)
            {
                DI.UI.ShowMessage(new MessageBoxDialogViewModel
                {
                    Title = "Message",
                    Message = $"Sistem tidak menemukan data '{SearchText}' di dalam server!"
                });

                CoreDI.Logger.Log(Ex.Message);
            }

           
                

              

        }
        
        public async void LoadTransactionInfo()
        {
            var counter = 0000;
  
            var rand = new Random();
            
            TransactionId = $"TM{DateTime.Now.ToLocalTime().ToString("yyMM")}{counter = rand.Next(1000, 9999)}";
            Bill = $"TM{DateTime.Now.ToLocalTime().ToString("yyyyMMdd")}{RandomString(4, false)}";
            TransactionDetailId = $"TO{DateTime.Now.ToLocalTime().ToString("yyMM")}{counter = rand.Next(1000, 9999)}";

            await Task.Delay(TimeSpan.FromSeconds(0.5));
        }

        public string RandomString(int size, bool lowerCase)
        {
            StringBuilder builder = new StringBuilder();
            Random random = new Random();
            char ch;
            for (int i = 0; i < size; i++)
            {
                ch = Convert.ToChar(Convert.ToInt32(Math.Floor(26 * random.NextDouble() + 65)));
                builder.Append(ch);
            }
            if (lowerCase)
                return builder.ToString().ToLower();
            return builder.ToString();
        }

        public async void LoadCustomerInfo()
        {
            var customers = (from customer in Context.Customers
                             where customer.CustomerId == CustomerId
                             select customer).ToList();

            if (customers.Count >= 1)
            {
                foreach (var customer in customers)
                {
                    Name = customer.Name.ToString();
                    PhoneNumber = customer.PhoneNumber.ToString();
                    Address = customer.Address.ToString();
                    
                }
                
            }

            else
            {
                await DI.UI.ShowMessage(new MessageBoxDialogViewModel
                {
                    Title = "Message",
                    Message = $"No dengan ID {CustomerId} belum terdaftar"
                });
            }


            await Task.Delay(TimeSpan.FromSeconds(0.5));
        }

        public async void ProcessAndPrintTransactionOrder()
        {
            Cash = Convert.ToDecimal(TouchPadNumber);

            if (Cash < TotalPay)
            {
                await DI.UI.ShowMessage(new MessageBoxDialogViewModel
                {
                    Title = "Message",
                    Message = "Opss.. nominal uang pembayaran tidak cukup!"
                });

                return;
            }
            else
            {
                Change = Cash - TotalPay;

                var totalDisc = OrderListItemsCollection.Sum(s => s.Discount);

                foreach (var order in OrderListItemsCollection)
                {
                    TransactionListItemsCollection.Add(new TransactionDetailViewModel
                    {
                        TransactionId = TransactionId,
                        TransactionDetailId = TransactionDetailId,
                        Bill = Bill,
                        CustomerName = Name,
                        User = User,
                        CustomerId = CustomerId,
                        PhoneNumber = PhoneNumber,
                        Address = Address,
                        DateAndTime = DateAndTime.ToString("yyyy/MM/dd hh:mm:ss"),
                        Note = Note,
                        Change = Change,
                        Total = TotalPay,
                        Cash = Cash,
                        Disc = totalDisc,
                        Quantity = TotalQuantity,
                        ProductId = order.ProductId,
                        PurchaseTotal = order.Quantity,
                        UnitPrice = order.Price,
                        TotalPayItem = order.Subtotal,

                    });


                }



                try
                {
                    var rect = new Rectangle(190, 350);

                    PdfDocument = new Document(rect);

                    var writer = PdfWriter.GetInstance(PdfDocument, new FileStream(FilePath + FileName, FileMode.Create));
                    var font = new Font(Font.FontFamily.COURIER, 6f, 0, new BaseColor(System.Drawing.ColorTranslator.FromHtml("#333")));
                    PdfDocument.Open();
                    var transactionInfo = default(string);
                    var date = DateTime.Now.ToString("yyyy/MM/dd");
                    var time = DateTime.Now.ToString("hh:mm:dd");

                    transactionInfo =
$@"
Nota         : {Bill.ToString()}
Tanggal      : {date}
Jam          : {time}
Kasir        : {User.ToString() ?? "System"}
Pelanggan    : {Name.ToString() ?? "Umum"}
-------------------------------     
Name         Qty    Subtotal
";
                    var paragraph = new Paragraph(transactionInfo, font)
                    {
                        Alignment = Element.ALIGN_LEFT
                    };

                    PdfDocument.Add(paragraph);
                    var totalItem = 0;

                    var text = "";
                    foreach (var dataOrder in OrderListItemsCollection)
                    {
                        text = $"{dataOrder.ProductName.ToString().Substring(0, 8) ?? dataOrder.ProductName.ToString().Substring(0, 6) }      {dataOrder.Quantity.ToString() ?? "None"}    {Convert.ToDecimal(dataOrder.Subtotal).ToString("C0") ?? "None"}";
                        paragraph = new Paragraph(text, font);
                        PdfDocument.Add(paragraph);
                        totalItem++;

                        if (totalItem == OrderListItemsCollection.Count)
                        {
                            var lineText = "-------------------------------";
                            var line = new Phrase(lineText, font);
                            PdfDocument.Add(line);

                            text = $"Total   :          {Convert.ToDecimal(TotalPay).ToString("C0") ?? "0"}";
                            paragraph = new Paragraph(text, font);
                            PdfDocument.Add(paragraph);
                            text = $"Dibayar :          {Convert.ToDecimal(Cash).ToString("C0") ?? "0"}";
                            paragraph = new Paragraph(text, font);
                            PdfDocument.Add(paragraph);
                            text = $"Kembali :          {Convert.ToDecimal(Change).ToString("C0") ?? "0"}";
                            paragraph = new Paragraph(text, font);
                            PdfDocument.Add(paragraph);

                            paragraph = new Paragraph($"{Environment.NewLine}");
                            PdfDocument.Add(paragraph);

                            paragraph = new Paragraph($"Catatan:", font);
                            PdfDocument.Add(paragraph);
                            paragraph = new Paragraph($"{Note.Substring(0, 27) ?? "Tidak ada catatan"}", font);
                            PdfDocument.Add(paragraph);
                            paragraph = new Paragraph($"{Environment.NewLine}");
                            PdfDocument.Add(paragraph);
                            line = new Phrase(" Terimakasih telah berbelanja", font);
                            PdfDocument.Add(line);
                            paragraph = new Paragraph($"{Environment.NewLine}");
                            PdfDocument.Add(paragraph);


                        }

                    }

                    if (TransactionListItemsCollection != null || TransactionListItemsCollection.Count >= 1)
                    {

                        foreach (var transaction in TransactionListItemsCollection)
                        {

                            var users = (from user in Context.Users
                                         where user.Name == User
                                         select user).ToList();

                            if (users.Count >= 1)
                            {
                                foreach (var user in users)
                                {

                                    GetTransactionModel = new Transaction
                                    {
                                        TransactionId = transaction.TransactionId,
                                        Bill = transaction.Bill,
                                        Date = DateAndTime,
                                        GrandTotal = TotalPay,
                                        Cash = Cash,
                                        Note = Note,
                                        CustomerId = CustomerId ?? "None",
                                        UsersId = user.UserId ?? "System",


                                    };

                                    var counter = 0;
                                    var rand = new Random();

                                    GetTransactionDetailModel = new TransactionDetail
                                    {
                                        TransactionDetailId = $"TO{DateTime.Now.ToLocalTime().ToString("yyMM")}{counter = rand.Next(1000, 9999)}",
                                        TransactionId = transaction.TransactionId,
                                        ProductId = transaction.ProductId,
                                        PurchaseTotal = transaction.PurchaseTotal,
                                        UnitPrice = transaction.UnitPrice,
                                        Disc = transaction.Disc,
                                        Total = transaction.TotalPayItem,


                                    };

                                    if (GetTransactionModel != null)
                                        Context.Transactions.Add(GetTransactionModel);

                                    if (GetTransactionDetailModel != null)
                                    {
                                       
                                        Context.TransactionDetails.Add(GetTransactionDetailModel);

                                    }

                                    Context.SaveChanges();

                                    Process.Start(FilePath + FileName);


                                    await Task.Delay(TimeSpan.FromSeconds(0.5));


                                }
                            }



                        }
                    }


                }
                catch (Exception Ex)
                {
                    CoreDI.Logger.Log(Ex.Message);
                }
                finally
                {
                    PdfDocument.Close();
                    OrderListItemsCollection.Clear();
                    TransactionDetailModelCollection.Clear();
                    TransactionModelCollection.Clear();
                    TransactionListItemsCollection.Clear();
                    LoadTransactionInfo();
                    Address = "-";
                    PhoneNumber = "-";
                    Name = "Umum";
    }
              

            }
        }

        public async void LoadProductListItem()
        {
         
            if (ProductListItemsCollection != null)
                ProductListItemsCollection.Clear();
            
            
            var query = (from product in Context.Products
                         join category in Context.ProductCategories on product.ProductCategoryId equals category.ProductCategoryId
                         join supplier in Context.ProductSuppliers on product.ProductSupplierId equals supplier.ProductSupplierId
                         orderby product.ProductId
                         select new
                         {
                             product.ProductId,
                             product.ProductName,
                             category.Category,
                             supplier.Brand,
                             product.CurrentStock,
                             product.Price,
                             product.Description,
                             product.ImgUrl,

                         }).ToList();

            if (query != null)
            {
                foreach (var pro in query)
                    ProductListItemsCollection.Add(new ProductListItemModel
                    {
                        ProductId = pro.ProductId,
                        ProductName = pro.ProductName,
                        ProductCategory = pro.Category,
                        Brand = pro.Brand,
                        CurrentStock = pro.CurrentStock,
                        Price = pro.Price ?? 0,
                        Description = pro.Description,
                        ImgUrl = pro.ImgUrl,

                    });
            }
  

            await Task.Delay(TimeSpan.FromSeconds(0.5));

        }
        
        public void AddProductToConfrimOrderListItem()
        {
            if (GetProductListItemModel == null)
            {
                DI.UI.ShowMessage(new MessageBoxDialogViewModel
                {
                    Title = "Message",
                    Message = "Please choose one or more some product to the order list!"
                });

                return;
            }

            try
            {
                if (GetProductListItemModel != null)
                {
                    ConfirmOrderListItemsCollection.Add(new ConfirmOrderListItemModel
                    {
                        ProductId = GetProductListItemModel.ProductId,
                        ProductName = GetProductListItemModel.ProductName,
                        ProductCategory = GetProductListItemModel.ProductCategory,
                        Brand = GetProductListItemModel.Brand,
                        Price = GetProductListItemModel.Price,
                        ImgUrl = GetProductListItemModel.ImgUrl,


                    });

                  
                }

                
            }

            catch (Exception Ex)
            {
                CoreDI.Logger.Log(Ex.Message);
                System.Windows.MessageBox.Show(Ex.Message);
            }



        }
        
        public void AddConfrimOrderToOrderListItem()
        {
            if (GetConfirmOrderListItemModel == null)
            {
                DI.UI.ShowMessage(new MessageBoxDialogViewModel
                {
                    Title = "Message",
                    Message = "Please choose one or more some product to the order list!"
                });

                return;
            }

            try
            {
                if (GetConfirmOrderListItemModel != null)
                {
                    if (System.Windows.MessageBox.Show($"Apakah Anda yakin ingin menambahkan {GetConfirmOrderListItemModel.ProductName} kedalam Daftar Pembelian ?", "Konfirmasi", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                    {
                        OrderListItemsCollection.Add(new OrderListItemModel
                        {
                            ProductId = GetConfirmOrderListItemModel.ProductId,
                            ProductName = GetConfirmOrderListItemModel.ProductName,
                            ProductCategory = GetConfirmOrderListItemModel.ProductCategory,
                            Brand = GetConfirmOrderListItemModel.Brand,
                            Price = GetConfirmOrderListItemModel.Price,
                            Quantity = GetConfirmOrderListItemModel.Quantity,
                            Discount = GetConfirmOrderListItemModel.Total * (GetConfirmOrderListItemModel.Discount / 100),
                            Total = GetConfirmOrderListItemModel.Price * GetConfirmOrderListItemModel.Quantity,
                            Subtotal = GetConfirmOrderListItemModel.Total - (GetConfirmOrderListItemModel.Total * (GetConfirmOrderListItemModel.Discount / 100)),



                        });

                        if (OrderListItemsCollection != null)
                        {

                                TotalPay = OrderListItemsCollection.Sum(items => items.Subtotal);
                                TotalQuantity = OrderListItemsCollection.Sum(items => items.Quantity);

                            
                        }
                    }
                   

                   
                }
                
            }

            catch (Exception Ex)
            {
                CoreDI.Logger.Log(Ex.Message);
                System.Windows.MessageBox.Show(Ex.Message);
            }



        }

        public void DeleteConfirmOrderItems()
        {
            if (GetConfirmOrderListItemModel == null)
            {
                DI.UI.ShowMessage(new MessageBoxDialogViewModel
                {
                    Title = "Message",
                    Message = "Please choose one or more some product to the order list!"
                });

                return;
            }

            try
            {
                if (GetConfirmOrderListItemModel != null)
                {
                    if (System.Windows.MessageBox.Show($"Apakah Anda yakin ingin menghapus {GetConfirmOrderListItemModel.ProductName} dari Daftar ?", "Konfirmasi", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                    {
                        ConfirmOrderListItemsCollection.Remove(GetConfirmOrderListItemModel);
                        
                    }



                }

            }

            catch (Exception Ex)
            {
                CoreDI.Logger.Log(Ex.Message);
                System.Windows.MessageBox.Show(Ex.Message);
            }


        }

        #region Public Class Helper


        public class ProductListItemModel
        {
            public string ProductId { get; set; }
            public string ProductName { get; set; }

            public string ProductCategory { get; set; }
            public string Brand { get; set; }

            public int CurrentStock { get; set; }

            public decimal Price { get; set; }

            public string Description { get; set; }

            public string ImgUrl { get; set; }

        }

        public class ConfirmOrderListItemModel
        {
            private decimal total;
            
            public string ProductId { get; set; }
            public string ProductName { get; set; }

            public string ProductCategory { get; set; }
            public string Brand { get; set; }

            public int Quantity { get; set; } = 1;

            public decimal Price { get; set; }

            public string ImgUrl { get; set; } = "Not Available";

            public decimal Discount { get; set; }
            public decimal Total { get => total = Price * Quantity; set => total = value; }
        }
        public class OrderListItemModel
        {
            public string ProductId { get; set; }
            public string ProductName { get; set; }

            public string ProductCategory { get; set; }
            public string Brand { get; set; }

            public int Quantity { get; set; } = 1;

            public decimal Price { get; set; } = 0;

            public string ImgUrl { get; set; } = "Not Available";

            public decimal Discount { get; set; }
            public decimal Subtotal { get; set; }
            public decimal Total { get; set; }

        }
        
        #endregion

    }
}

