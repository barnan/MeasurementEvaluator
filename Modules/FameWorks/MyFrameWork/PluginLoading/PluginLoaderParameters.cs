using MyFrameWork.ConfigHandler;
using MyFrameWork.Interfaces;
using System;

namespace MyFrameWork.PluginLoading
{
    internal class PluginLoaderParameters
    {
        [Configuration("Logger", "Logger", LoadComponent = true)]
        private string _logger = null;

        public IMyLogger MyLogger { get; private set; }


        internal bool Load(string sectionName)
        {
            PluginLoader.ConfigHandler.Load(this, sectionName);

            MyLogger = PluginLoader.CreateInstance<IMyLogger>(_logger);

            return CheckComponent();
        }


        private bool CheckComponent()
        {
            if (MyLogger == null)
            {
                throw new Exception($"{nameof(PluginLoaderParameters)} could not been loaded.");
            }
            return true;
        }

    }
}
