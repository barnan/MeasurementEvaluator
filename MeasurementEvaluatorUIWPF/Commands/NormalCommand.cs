﻿using System;
using System.Windows.Input;

namespace MeasurementEvaluatorUI.Commands
{



    /// <summary>
    /// 
    ///  NOT READY YET
    /// </summary>



    class NormalCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;


        public bool CanExecute(object parameter)
        {
            return true;
        }


        public void Execute(object parameter)
        {

        }
    }
}