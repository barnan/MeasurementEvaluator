﻿using MeasurementEvaluatorUI;
using NLog;
using System;
using System.Diagnostics;
using System.Threading;
using System.Windows;

namespace MeasurementEvaluator
{
    class Program
    {
        private static ILogger _logger;
        private static ManualResetEvent _uiFinished = new ManualResetEvent(false);

        static void Main(string[] args)
        {
            try
            {

                _logger = LogManager.GetCurrentClassLogger();


                Thread appThread = new Thread(() =>
                {
                    Application application = new Application();

                    MainWindow mainWindow = new MainWindow { Title = "Measurement Evaluator" };
                    mainWindow.Closed += MainWindow_OnClosed;

                    System.Windows.Application.Current.MainWindow = mainWindow;
                    mainWindow.Show();
                    application.Run(mainWindow);

                });
                Debug.Assert(appThread != null);
                appThread.Name = "WpfThread";
                appThread.SetApartmentState(ApartmentState.STA);
                appThread.IsBackground = true;
                appThread.Start();

                _uiFinished.WaitOne();

            }
            catch (Exception ex)
            {
                _logger.Error($"Exception occured: {ex}");
            }

        }

        private static void MainWindow_OnClosed(object sender, EventArgs eventArgs)
        {
            _uiFinished.Set();
        }
    }
}