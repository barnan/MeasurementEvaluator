using Interfaces.MeasuredData;
using System.Collections.Generic;

namespace DataAcquisition.DAL
{

    /// <summary>
    /// 
    /// </summary>
    public interface IMeasDataFileReader
    {
        IToolMeasurementData Read();

        string ToolName { get; set; }
        List<string> InputFileList { get; set; }
        List<string[]> ExtensionList { get; set; }
    }

}
