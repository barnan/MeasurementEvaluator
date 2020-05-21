using MyFrameWork.Interfaces;

namespace MyFrameWork.PluginLoader
{
    internal class FactoryElement
    {
        internal IPluginFactory Factory { get; set; }
        internal string AssemblyName { get; set; }
    }

    internal static class FolderSettingsKeys
    {
        internal const string ConfigurationFolderKey = "ConfigurationFolder";
        internal const string SpecificactionFolderKey = "SpecificationFolder";
        internal const string ReferenceFolderKey = "ReferenceFolder";
        internal const string MeasurementDataFolderKey = "MeasurementFolder";
        internal const string PluginsFolderKey = "PluginsFolder";
        internal const string ResultFolderKey = "ResultFolder";
    }
}
