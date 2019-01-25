using Interfaces.Result;
using System;
using System.Xml.Linq;

namespace DataStructures.Calculation
{
    public class SimpleCalculationResult : ISimpleCalculationResult
    {
        public DateTime CreationTime { get; }

        public double Result { get; }

        public DateTime StartTime => throw new NotImplementedException();

        public DateTime EndTime => throw new NotImplementedException();

        public bool SuccessfulCalculation => throw new NotImplementedException();


        public SimpleCalculationResult(DateTime creationTime, double result)
        {
            CreationTime = creationTime;
            Result = result;
        }


        // TODO: 
        public XElement Save()
        {
            throw new NotImplementedException();
        }



        public bool Load(XElement input)
        {
            throw new NotImplementedException();
        }
    }
}
