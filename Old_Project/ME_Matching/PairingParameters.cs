﻿using Frame.ConfigHandler;
using Frame.MessageHandler;
using Frame.PluginLoader;
using Interfaces.DataAcquisition;
using NLog;

namespace MeasurementEvaluator.ME_Matching
{
    internal class PairingParameters : IPairingParameters
    {
        public ILogger Logger { get; private set; }

        [Configuration("Handles the reading of the matching file.", "File Reader", LoadComponent = true)]
        private string _pairingFileReader = null;
        public IHDDFileReader PairingFileReader { get; private set; }

        [Configuration("Name of the matching file", "Name of the matching file", LoadComponent = false)]
        private string _bindingFilePath = "PairingDictionary";
        public string BindingFilePath => _bindingFilePath;

        public IUIMessageControl MessageControl { get; private set; }


        public string Name { get; private set; }


        internal bool Load(string sectionName)
        {
            Name = sectionName;

            Logger = LogManager.GetCurrentClassLogger();

            PluginLoader.ConfigManager.Load(this, sectionName);

            PairingFileReader = PluginLoader.CreateInstance<IHDDFileReader>(_pairingFileReader);
            MessageControl = PluginLoader.MessageControll;

            return CheckComponent();
        }


        private bool CheckComponent()
        {
            if (PairingFileReader == null)
            {
                Logger.Error($"Error in the {nameof(PairingParameters)} loading. {nameof(PairingFileReader)} is null.");
                return false;
            }

            if (string.IsNullOrEmpty(BindingFilePath))
            {
                Logger.Error($"Error in the {nameof(PairingParameters)} loading. {nameof(BindingFilePath)} is empty.");
                return false;
            }

            if (MessageControl == null)
            {
                Logger.Error($"Error in the {nameof(PairingParameters)} loading. {nameof(MessageControl)} is null.");
                return false;
            }

            return true;
        }
    }
}
