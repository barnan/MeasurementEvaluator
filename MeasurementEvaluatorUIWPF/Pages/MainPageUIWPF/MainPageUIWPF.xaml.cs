﻿using Interfaces.Misc;
using System.Windows.Controls;

namespace MeasurementEvaluatorUIWPF.Pages.MainPageUIWPF
{
    /// <summary>
    /// Interaction logic for MainPageUIWPF.xaml
    /// </summary>
    public partial class MainPageUIWPF : Page, IMainPageUIWPF
    {
        public MainPageUIWPF(MainPageUIWPFParameters parameters)
        {
            InitializeComponent();

            DataContext = new MainPageViewModel(parameters);
        }
    }
}
