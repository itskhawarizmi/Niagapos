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
using System.Data;
using System.Diagnostics;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;
using Niagapos.Core;

namespace Niagapos
{
    public class ReportViewModel : BaseViewModel
    {
        public ReportViewModel()
        {
            if (DesignerProperties.GetIsInDesignMode(new DependencyObject()))
                return;

            Context = new NiagaposEntities();
            DataReportItems = new ObservableCollection<DataReportModel>();
            SelectedItems = new DataReportModel();

            LoadData();

            SetupCommand();
        }


        public DataReportModel SelectedItems { get; set; }
        public NiagaposEntities Context { get; set; }
        public ObservableCollection<DataReportModel> DataReportItems { get; set; }
        

        public ICommand AddDataCommand { get; set; }
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
        public string FileName { get; set; } = "Laporan Penjualan.pdf";
        public string FilePath { get; set; } = "D:/WPF Project/Niagapos/Niagapos/Attachments/Report/";
        public Document PdfDocument { get; set; }
        public string SearchText { get; set; }
        public ICommand ClearSearchHistoryCommand { get; set; }

        public ICommand ExportDataCommand { get; set; }

        public void SetupCommand()
        {
            RefreshDataCommand = new RelayCommand(async () => await Refresh());
            SearchDataCommand = new RelayCommand(() => Search());
            ClearSearchHistoryCommand = new RelayCommand(() => ClearSearchHistory());

            EditControlMenuCommand = new RelayCommand(() => CanEditControl());
            CloseEditControlMenuCommand = new RelayCommand(() => CloseEditControl());
            AddControlMenuCommand = new RelayCommand(() => AddReportsControl());
            CloseAddControlMenuCommand = new RelayCommand(() => CloseMenu());
            ExportDataCommand = new RelayCommand(() => ExportData());
        }

        public async void ExportData()
        {
            if (DataReportItems == null)
            {
                await DI.UI.ShowMessage(new MessageBoxDialogViewModel
                {
                    Title = "Pesan",
                    Message = "Maaf, Sistem tidak menemukan data apapun tentang laporan penjualan di dalam database!"
                });
            }
            else
            {
                try
                {
                    if (System.Windows.MessageBox.Show($"Apakah Anda yakin ingin mencetak laporan penjualan ?", "Message",
                         MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.Yes) == MessageBoxResult.Yes)
                    {


                        PdfDocument = new Document(PageSize.A4);

                        var writer = PdfWriter.GetInstance(PdfDocument, new FileStream(FilePath + FileName, FileMode.Create));
                        var font = new Font(Font.FontFamily.COURIER, 8f, 0, new BaseColor(System.Drawing.ColorTranslator.FromHtml("#111")));
                        var fontHeader = new Font(Font.FontFamily.TIMES_ROMAN, 9f, 1, new BaseColor(System.Drawing.ColorTranslator.FromHtml("#fff")));
                        PdfDocument.Open();
                        var transactionInfo = default(string);
                        var date = DateTime.Now.ToString("yyyy/MM/dd");
                        var time = DateTime.Now.ToString("hh:mm:dd") + " WIB";


                        transactionInfo =
        $@"
Nama         : Data Laporan Penjualan
Tanggal      : {date}
Jam          : {time} 
";
                        var paragraph = new Paragraph(transactionInfo, font)
                        {
                            Alignment = Element.ALIGN_LEFT
                        };

                        PdfDocument.Add(paragraph);

                        var table = new PdfPTable(12)
                        {
                            TotalWidth = 350f,
                            WidthPercentage = 100f,
                            SpacingBefore = 5f,

                        };
                        

                        var cellNo = new PdfPCell(new Phrase("No.", fontHeader))
                        {
                            BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#498ccd")),
                            Border = 3,
                            BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#498ccd")),
                        };

                        var cellBill = new PdfPCell(new Phrase("Nota", fontHeader))
                        {
                            BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#498ccd")),
                            Border = 1,
                            BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#498ccd")),
                        };

                        var cellDate = new PdfPCell(new Phrase("Tanggal", fontHeader))
                        {
                            BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#498ccd")),
                            Border = 1,
                            BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#498ccd")),
                        };


                        var cellProductId = new PdfPCell(new Phrase("ID", fontHeader))
                        {
                            BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#498ccd")),
                            Border = 1,
                            BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#498ccd")),
                        };

                        var cellProductName = new PdfPCell(new Phrase("Nama Produk", fontHeader))
                        {
                            BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#498ccd")),
                            Border = 1,
                            BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#498ccd")),
                        };

                        var cellUnitPrice = new PdfPCell(new Phrase("Harga", fontHeader))
                        {
                            BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#498ccd")),
                            Border = 1,
                            BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#498ccd")),
                        };

                        var cellQuantity = new PdfPCell(new Phrase("Jumlah", fontHeader))
                        {
                            BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#498ccd")),
                            Border = 1,
                            BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#498ccd")),
                        };

                        var cellTotal = new PdfPCell(new Phrase("Total", fontHeader))
                        {
                            BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#498ccd")),
                            Border = 1,
                            BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#498ccd")),
                        };

                        var cellDiscount = new PdfPCell(new Phrase("Diskon", fontHeader))
                        {
                            BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#498ccd")),
                            Border = 1,
                            BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#498ccd")),
                        };

                        var cellCash = new PdfPCell(new Phrase("Uang pembayaran", fontHeader))
                        {
                            BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#498ccd")),
                            Border = 1,
                            BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#498ccd")),
                        };

                        var cellUser = new PdfPCell(new Phrase("Petugas", fontHeader))
                        {
                            BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#498ccd")),
                            Border = 1,
                            BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#498ccd")),
                        };

                        var cellCustomer = new PdfPCell(new Phrase("Customer", fontHeader))
                        {
                            BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#498ccd")),
                            Border = 1,
                            BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#498ccd")),
                        };
                        
                        table.AddCell(cellNo);
                        table.AddCell(cellBill);
                        table.AddCell(cellDate);
                        table.AddCell(cellProductId);
                        table.AddCell(cellProductName);
                        table.AddCell(cellUnitPrice);
                        table.AddCell(cellQuantity);
                        table.AddCell(cellTotal);
                        table.AddCell(cellDiscount);
                        table.AddCell(cellCash);
                        table.AddCell(cellUser);
                        table.AddCell(cellCustomer);

                        var salesTotal = default(string);
                        var salesQty = default(string);

                        if (DataReportItems != null)
                        {
                            var counter = 1;
                            salesTotal = DataReportItems.Sum(s => s.Total).ToString(string.Format("C0"));
                            salesQty = DataReportItems.Sum(s => s.Quantity).ToString(string.Format("C0"));
                            foreach (var report in DataReportItems)
                            {
                                table.AddCell(new Phrase(counter.ToString(), font));
                                table.AddCell(new Phrase(report.Bill, font));
                                table.AddCell(new Phrase(report.DateAndTime, font));
                                table.AddCell(new Phrase(report.ProductId, font));
                                table.AddCell(new Phrase(report.ProductName, font));
                                table.AddCell(new Phrase(report.UnitPrice.ToString(string.Format("C0")), font));
                                table.AddCell(new Phrase(report.Quantity.ToString(), font));
                                table.AddCell(new Phrase(report.Total.ToString(string.Format("C0")), font));
                                table.AddCell(new Phrase(report.Disc.ToString(string.Format("C0")), font));
                                table.AddCell(new Phrase(report.Cash.ToString(string.Format("C0")), font));
                                table.AddCell(new Phrase(report.User, font));
                                table.AddCell(new Phrase(report.CustomerName, font));

                                
                                counter++;

                            }

                            PdfDocument.Add(table);

                        }



                        transactionInfo = $@"
Total Perolehan Omset: Rp.{salesTotal} 
Total Kuantitas Produk Terjual : {salesQty} pcs";
                        var sales = new Paragraph(transactionInfo, font)
                        {
                            Alignment = Element.ALIGN_LEFT
                        };

                        PdfDocument.Add(sales);

                        await DI.UI.ShowMessage(new MessageBoxDialogViewModel
                        {
                            Title = "Pesan",
                            Message = "Laporan Penjualan Berhasil di Cetak!"
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
        public void AddReportsControl()
        {
            DI.Application.AddReportsControlMenuVisible = true;
        }

        public void CloseMenu()
        {
            DI.Application.AddReportsControlMenuVisible = false;
        }

        public void CanEditControl() => CanEdit = true;

        public void CloseEditControl() => CanEdit = false;

     
        public async void LoadData()
        {
        

            DataReportItems.Clear();


            var query = (from transaction in Context.Transactions
                         join detail in Context.TransactionDetails on transaction.TransactionId equals detail.TransactionId
                         join product in Context.Products on detail.ProductId equals product.ProductId
                         join customer in Context.Customers on transaction.CustomerId equals customer.CustomerId
                         join user in Context.Users on transaction.UsersId equals user.UserId
                         select new
                         {
                            transaction.TransactionId,
                            transaction.Bill,
                            transaction.Date,
                            transaction.GrandTotal,
                            transaction.Cash,
                            detail.TransactionDetailId,
                            detail.PurchaseTotal,
                            detail.Disc,
                            detail.UnitPrice,
                            product.ProductId,
                            product.ProductName,
                            customer.Name,
                            customer.CustomerId,
                            user.Username,
                            user.UserId,


                         }).ToList();


            foreach(var report in query)
            {
                DataReportItems.Add(new DataReportModel
                {
                    TransactionDetailId = report.TransactionId,
                    Bill = report.Bill,
                    ProductId = report.ProductId,
                    DateAndTime = report.Date.ToString("yyyy-MM-dd - hh:mm:ss"),
                    ProductName = report.ProductName,
                    UnitPrice = report.UnitPrice,
                    Quantity = report.PurchaseTotal,
                    Disc = Convert.ToDecimal(report.Disc),
                    Total = report.GrandTotal,
                    Cash = report.Cash,
                    User = report.Username,
                    CustomerName = report.Name,


                });
            }
            

            await Task.Delay(1);
        }


       
        public async Task Refresh()
        {
            Context.TransactionDetails.Load();
            Context.Transactions.Load();
            LoadData();
            await Task.Delay(TimeSpan.FromSeconds(0.1));
        }

        public async void Search()
        {
            if (SearchText == null)
                return;

            else
            {

                var query = (from transaction in Context.Transactions
                             join detail in Context.TransactionDetails on transaction.TransactionId equals detail.TransactionId
                             join product in Context.Products on detail.ProductId equals product.ProductId
                             join customer in Context.Customers on transaction.CustomerId equals customer.CustomerId
                             join user in Context.Users on transaction.UsersId equals user.UserId
                             where SearchText == transaction.Date.ToString() || SearchText.Contains(transaction.Date.ToString()) 
                         select new
                         {
                            transaction.TransactionId,
                            transaction.Bill,
                            transaction.Date,
                            transaction.GrandTotal,
                            transaction.Cash,
                            detail.TransactionDetailId,
                            detail.PurchaseTotal,
                            detail.Disc,
                            detail.UnitPrice,
                            product.ProductId,
                            product.ProductName,
                            customer.Name,
                            customer.CustomerId,
                            user.Username,
                            user.UserId,


                         }).ToList();


                if(query.Count > 0)
                {
                    DataReportItems.Clear();

                    foreach (var report in query)
                    {
                        DataReportItems.Add(new DataReportModel
                        {
                            TransactionDetailId = report.TransactionId,
                            Bill = report.Bill,
                            DateAndTime = report.Date.ToString("yyyy-MM-dd - hh:mm:ss"),
                            ProductName = report.ProductName,
                            UnitPrice = report.UnitPrice,
                            Quantity = report.PurchaseTotal,
                            Disc = Convert.ToDecimal(report.Disc),
                            Total = report.GrandTotal,
                            Cash = report.Cash,
                            User = report.Username,
                            CustomerName = report.Name,


                        });
                    }

                    await Task.Delay(TimeSpan.FromSeconds(0.1));
                }
                else
                {
                    await DI.UI.ShowMessage(new MessageBoxDialogViewModel
                    {
                        Title = "Pesan",
                        Message = $"Tidak ada laporan ditemukan pada {SearchText}"
                    });
                }
               
               
            }
        }

        public void ClearSearchHistory()
        {
           
        }


    }

    public class DataReportModel
    {
        public string TransactionDetailId { get; set; }
        public string TransactionId { get; set; }
        public string CustomerId { get; set; }
        public string CustomerName { get; set; }
        public string ProductName { get; set; }
        public string Bill { get; set; } = "TO1901001";
        public string DateAndTime { get; set; } = DateTime.Now.ToLocalTime().ToString("yyyy-MM-dd - hh:mm:ss");
        public string User { get; set; } = "Administrator";
        public string ProductId { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TotalPayItem { get; set; }
        public int PurchaseTotal { get; set; }
        public decimal Total { get; set; }
        public int Quantity { get; set; }
        public decimal Disc { get; set; }
        public decimal Cash { get; set; }
    }



}
