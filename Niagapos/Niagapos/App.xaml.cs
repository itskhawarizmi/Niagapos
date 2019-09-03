using Niagapos.Core;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace Niagapos
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        /// <summary>
        /// Custom startup so we load our DI immediately before anything else
        /// </summary>
        /// <param name="e"></param>
        protected override async void OnStartup(StartupEventArgs e)
        {
            // Let the base application do what it needs
            base.OnStartup(e);

            // Setup the main application 
            await ApplicationSetupAsync();

            // Log it
            CoreDI.Logger.Log("Application starting....", LogLevel.Debug);

            // Show the main window
            Current.MainWindow = new MainWindow();
            Current.MainWindow.Show();

        }

        /// <summary>
        /// Configures our application ready for use
        /// </summary>
        private async Task ApplicationSetupAsync()
        {
            DI.Setup();

            CoreDI.Kernel.Bind<ILogFactory>().ToConstant(new BaseLogFactory(new FileLogger[]
            {
                // TODO: Add ApplicationSettings so we can set/edit a log location
                //       For now just log to the path where this application is running
                new Core.FileLogger("OldLog.txt"),
            }));

            // Bind a UI Manager
            DI.Kernel.Bind<IUIManager>().ToConstant(new UIManager());




            // Bind a file manager
            CoreDI.Kernel.Bind<IFileManager>().ToConstant(new FileManager());

            // Add our task manager
            CoreDI.Kernel.Bind<ITaskManager>().ToConstant(new TaskManager());

            await Task.Delay(1);

        }

    }
}
