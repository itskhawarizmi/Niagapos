using Pospedia.Core;
using System.Windows.Controls;

namespace Pospedia
{
    /// <summary>
    /// Interaction logic for CalculatorControl.xaml
    /// </summary>
    public partial class CalculatorControl : UserControl
    {
        public CalculatorControl()
        {
            InitializeComponent();

            DataContext = IoC.Calc;
        }

    }
}
