﻿using Interfaces;
using Interfaces.MeasuredData;
using System.Collections.Generic;

namespace DataStructures.MeasuredData
{
    class MeasurementSerie : IMeasurementSerie
    {
        public string Name { get; }

        public List<IUniqueMeasurementResult> MeasData { get; }

        public Units Dimension { get; }


        public MeasurementSerie(string name, List<IUniqueMeasurementResult> measData, Units dimension)
        {
            Name = name;
            MeasData = measData;
            Dimension = dimension;
        }


        public IUniqueMeasurementResult this[int i]
        {
            get
            {
                return MeasData[i];
            }
        }

    }
}
