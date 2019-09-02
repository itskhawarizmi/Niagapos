using Pospedia.Core;
using System.Windows.Controls;
using System.Windows.Input;

namespace Pospedia
{
    /// <summary>
    /// Interaction logic for SettingsControl.xaml
    /// </summary>
    public partial class SettingsControl : UserControl
    {

        public SettingsControl()
        {
            InitializeComponent();

            DataContext = IoC.Setting;
            
        }

     
    }
}
