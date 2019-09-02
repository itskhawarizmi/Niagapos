using LiveCharts;
using LiveCharts.Defaults;
using LiveCharts.Wpf;
using System;
using System.Linq;
using Niagapos.Core;
using System.Data.Entity;
using System.ComponentModel;
using System.Windows;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace Niagapos
{
    public class DashboardViewModel : BaseViewModel
    {
        #region Constructor

        /// <summary>
        /// Default constructor.
        /// </summary>
        public DashboardViewModel()
        {
            if (DesignerProperties.GetIsInDesignMode(new DependencyObject()))
                return;

            Context = new NiagaposEntities();
            SeriesCollection = new SeriesCollection();
            SeriesCollectionPie = new SeriesCollection();

            DataLoadedCommand = new RelayCommand(() => DataLoaded());

            LoadChart();
            LoadDoughnutChart();

            
        }

        #endregion

        #region Public Properties

        public SeriesCollection SeriesCollection { get; set; }
        public SeriesCollection SeriesCollectionPie { get; set; }
        public List<string> Labels { get; set; }
        public double TotalIncome { get; set; }
        public int TotalQuantity { get; set; }
        public string BestProduct { get; set; }

        public NiagaposEntities Context { get; set; }

        public Func<double, string> YFormatter { get; set; }

        public ICommand DataLoadedCommand { get; set; }

        #endregion

        public void DataLoaded()
        {
            LoadChart();
            LoadDoughnutChart();
        }

        public void LoadDoughnutChart()
        {
            SeriesCollectionPie = new SeriesCollection
            {
                new PieSeries
                {
                    Title = "Nike",
                    Values = new ChartValues<ObservableValue> { new ObservableValue(8) },
                    DataLabels = true
                },
                new PieSeries
                {
                    Title = "Adidas",
                    Values = new ChartValues<ObservableValue> { new ObservableValue(14) },
                    DataLabels = true
                },
                new PieSeries
                {
                    Title = "Sketcher",
                    Values = new ChartValues<ObservableValue> { new ObservableValue(1) },
                    DataLabels = true
                },
                new PieSeries
                {
                    Title = "Puma",
                    Values = new ChartValues<ObservableValue> { new ObservableValue(1) },
                    DataLabels = true
                }
            };
        }

        public void LoadChart()
        {
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


            foreach (var report in query)
            {
                var counter = query.Count();

                ChartValues<decimal> values = new ChartValues<decimal>();
                Labels = new List<string>();
                foreach(var item in query)
                {
                    values.Add(item.GrandTotal);
                    Labels.Add(item.Date.ToString("MMM - dd"));
                    
                    TotalIncome = query.Sum(s => Convert.ToDouble(s.GrandTotal));
                    TotalQuantity = query.Sum(s => s.PurchaseTotal);
                   
                }


                SeriesCollection = new SeriesCollection()
                {
                    new LineSeries
                    {
                        Title = "Series 2",
                        Values = values,
                        PointGeometry = DefaultGeometries.Circle,
                        PointGeometrySize = 15,



                    },


                };

              
                YFormatter = value => value.ToString("C");

            }

        }

        private class ReportDataModel
        {
            public decimal TotalIncome { get; set; }
            public DateTime Date { get; set; }
        }

    }
}
