using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Measurement_Evaluator.Interfaces;

namespace Measurement_Evaluator.DAL
{
    //class DataReaderBase
    //{

        /// <summary>
        /// 
        /// </summary>
        public interface IDataFileReader
        {
            IToolMeasurementData Read(string fileName);
        }


    //}
}
