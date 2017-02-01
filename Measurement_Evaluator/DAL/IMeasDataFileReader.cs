using System;
using System.Collections.Generic;
using Measurement_Evaluator.Interfaces;

namespace Measurement_Evaluator.DAL
{
    public interface IMeasDataFileReader
    {
        IToolMeasurementData Read(string fileName);
        List<string> CheckFileExtension(List<string> inputs, List<string> enumDescriptions);
    }
}
