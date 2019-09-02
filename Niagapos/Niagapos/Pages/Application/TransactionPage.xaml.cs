using Niagapos.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace Niagapos
{
    /// <summary>
    /// Interaction logic for SalesPage.xaml
    /// </summary>
    public partial class TransactionPage : BasePage<TransactionViewModel>
    {
        public TransactionPage()
        {
            InitializeComponent();
        }

        public TransactionPage(TransactionViewModel transactionViewModel) : base(transactionViewModel)
        {
            InitializeComponent();
        }

      
    }

}
