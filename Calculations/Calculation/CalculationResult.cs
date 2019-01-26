using Interfaces.Result;
using System;
using System.Xml.Linq;

namespace Calculations.Calculation
{
    class CalculationResult : ICalculationResult
    {
        public DateTime StartTime { get; }

        public DateTime EndTime { get; }

        public bool SuccessfulCalculation { get; }



        public CalculationResult(DateTime startTime, DateTime endTime, bool successful)
        {
            StartTime = startTime;
            EndTime = endTime;
            SuccessfulCalculation = successful;
        }



        public bool Load(XElement input)
        {
            throw new NotImplementedException();
        }

        public XElement Save()
        {
            throw new NotImplementedException();
        }
    }
}
