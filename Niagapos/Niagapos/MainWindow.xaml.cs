using System.Windows;

namespace Niagapos
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            DataContext = new WindowViewModel(this);
        }
        private void AppWindow_Activated(object sender, System.EventArgs e)
        {
            // Show overlay if we lose focus
            (DataContext as WindowViewModel).DimmableOverlayVisible = false;
        }

        private void AppWindow_Deactivated(object sender, System.EventArgs e)
        {
            (DataContext as WindowViewModel).DimmableOverlayVisible = true;
        }
    }
}
