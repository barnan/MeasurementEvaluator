﻿using Interfaces.MeasuredData;
using System.Collections.Generic;

namespace DataStructures.MeasuredData
{
    public class ToolMeasurementDataOnHDD
    {
        public string ToolName { get; }

        public List<IMeasurementSerie> Results { get; }



        public ToolMeasurementDataOnHDD(string toolName, List<IMeasurementSerie> results)
        {
            ToolName = toolName;
            Results = results;
        }


    }
}
