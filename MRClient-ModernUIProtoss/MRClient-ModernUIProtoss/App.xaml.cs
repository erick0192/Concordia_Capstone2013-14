using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using MarsRoverClient.Log;

namespace MarsRoverClient
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            Initialize();           
        }

        private void Initialize()
        {
            ApplicationLogger.Instance.LogLevel = LogLevel.Debug;
            ApplicationLogger.Instance.LogToDebugConsole = true;
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            ApplicationLogger.Instance.Log("Application started.", LogLevel.Info);
        }

    }
}
