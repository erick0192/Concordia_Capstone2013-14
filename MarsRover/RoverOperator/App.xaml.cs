using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using MarsRover.Streams;

namespace RoverOperator
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
            StatusUpdater.Instance.StartUpdating();
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
        }

        protected override void OnExit(ExitEventArgs e)
        {
            WebCamStreamManager.Instance.StopAllStreams();
            StatusUpdater.Instance.StopUpdating();
            base.OnExit(e);
        }

    }
}
