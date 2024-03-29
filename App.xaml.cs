﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace ideabuilder_status
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private static string _guid = "2252d290-4372-11ee-8f23-0800200c9a66";
        private static Mutex _mutex = null;
        protected override void OnStartup(StartupEventArgs e)
        {
            bool createdNew;

            _mutex = new Mutex(true, _guid, out createdNew);

            if (!createdNew)
            {
                //app is already running! Exiting the application
                Application.Current.Shutdown();
            }

            base.OnStartup(e);
        }
    }


}
