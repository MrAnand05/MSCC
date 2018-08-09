// Copyright (c) Microsoft Corporation. All rights reserved. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using ViewSwitchingNavigation.Infrastructure;

namespace ViewSwitchingNavigation
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private readonly AuthenticationService _authenticationService = new AuthenticationService();
        private int _attempts;
        //public static int FinancialYear
        //{
        //    get { return 15; }
        //}
        public App()
        {
            Current.ShutdownMode = ShutdownMode.OnExplicitShutdown;
            ShowLogOn();
        }
        private void ShowLogOn()
        {
            var logon = new LogOn();
            logon.Attempts = _attempts;
            bool? res = logon.ShowDialog();
            if (!res ?? true)
            {
                Shutdown(1);
            }
            else
            {
                AuthenticationContext ac = _authenticationService.Authenticate(logon.UserName, logon.Password,logon.FinancialYear);
                if (ac.IsAuthorized)
                {
                    AuthenticationContext.GlobalFinancialYear = logon.FinancialYear;
                    StartUp(ac.AllowedModules);
                    //OnStartup(ac.AllowedModules);
                }
                else
                {
                    if (logon.Attempts > 2)
                    {
                        MessageBox.Show("Application is exiting due to invalid credentials", "Application Exit", MessageBoxButton.OK, MessageBoxImage.Error);
                        Shutdown(1);
                    }
                    else
                    {
                        _attempts += 1;
                        ShowLogOn();
                    }
                }
            }
        }
        //protected override void OnStartup(StartupEventArgs e)
        //{
        //    //Current.MainWindow = null;
        //    //Current.ShutdownMode = ShutdownMode.OnMainWindowClose;
        //    //SplashScreen splashScreen = new SplashScreen("resources/IMG_20150529_152029.jpg");
        //    //splashScreen.Show(true);
        //    QuickStartBootstrapper bootstrapper = new QuickStartBootstrapper();
        //    bootstrapper.Run();
        //}
        private static void StartUp(IList<string> allowedModules)
        {
            Current.MainWindow = null;
            Current.ShutdownMode = ShutdownMode.OnMainWindowClose;
            //QuickStartBootstrapper.a = 50;
            QuickStartBootstrapper bootstrapper = new QuickStartBootstrapper();
            bootstrapper.Run();
            //bootstrapper.LoadSecuredModules(allowedModules);
        }
    }
}
