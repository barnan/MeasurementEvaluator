using DataStructures;
using Interfaces.MeasuredData;
using Interfaces.Result;
using System;
using System.Xml.Linq;

namespace Calculations.Calculation
{
    internal abstract class CalculationResult<T> : ResultBase, ICalculationResult<T>
    {
        protected CalculationResult(DateTime creationTime, bool successful, IMeasurementSerie measurementSerie, T result, T average)
            : base(creationTime, successful)
        {
            MeasurementSerie = measurementSerie;
            ResultValue = result;
            Average = average;
        }

        public T Average { get; protected set; }

        public T ResultValue { get; protected set; }

        public IMeasurementSerie MeasurementSerie { get; }


        public abstract override XElement SaveToXml(XElement input);

        public abstract override bool LoadFromXml(XElement input);
    }
}
