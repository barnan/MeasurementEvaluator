using NLog;
using System;
using System.IO;
using System.Xml;

namespace Frame.ConfigHandler
{
    public class ConfigManager
    {
        private ILogger _logger;
        private string _configFolder;
        private readonly string _configFileExtension = ".config";

        public ConfigManager(string folder)
        {
            _logger = LogManager.GetCurrentClassLogger();
            _configFolder = folder;
        }


        public bool Load(object inputObj, string sectionName)
        {
            if (inputObj == null)
            {
                _logger.Error("Received object is null.");
                return false;
            }

            if (string.IsNullOrEmpty(sectionName) || string.IsNullOrWhiteSpace(sectionName))
            {
                _logger.Error($"Received {nameof(sectionName)} is null, empty, or white space");
                return false;
            }

            Type type = inputObj.GetType();
            string namespaceOfType = type.Namespace;

            _logger.Info($"Reading object (type: {type}) parameters in section name: {sectionName}");
            _logger.Info($"Object (type: {type}) namespace {namespaceOfType}");

            string[] configFiles;
            try
            {
                //get config file list:
                configFiles = Directory.GetFiles(_configFolder, _configFileExtension);

                if (configFiles.Length == 0)
                {
                    _logger.Error($"No config files were found in folder: {_configFolder}");
                    return false;
                }
            }
            catch (Exception ex)
            {
                _logger.Error($"Exception occured during config file search for {sectionName} in folder: {_configFolder}. {ex.Message}");
                return false;
            }

            string[] namespaceFragments = namespaceOfType.Split('.');

            if (!Array.Exists<string>(configFiles, p => Path.GetFileNameWithoutExtension(p) == namespaceFragments[0]))
            {
                _logger.Error($"{namespaceFragments[0]}{_configFileExtension} file was not found.");
                return false;
            }

            XmlDocument xmlDoc = new XmlDocument();
            XmlReaderSettings readerSettings = new XmlReaderSettings();
            using (StreamReader sr = new StreamReader(namespaceFragments[0] + _configFileExtension))
            {
                using (XmlReader xmlReader = XmlReader.Create(sr, readerSettings))
                {
                    xmlDoc.Load(xmlReader);
                }
            }
            return true;
        }


        private bool Parse(object obj, Type type, XmlDocument xmlDocument)
        {
            if (obj == null)
            {
                _logger.Error("Received obj is null.");
                return false;
            }

            if (xmlDocument == null)
            {
                _logger.Error($"Received {nameof(xmlDocument)} is null.");
                return false;
            }

            if (obj.GetType() != type)
            {
                _logger.Error($"Type of the arrived .");
                return false;
            }

            XmlNodeList sectionNodes = xmlDocument.DocumentElement.ChildNodes;
            foreach (var section in sectionNodes)
            {

            }

            return true;
        }

    }
}
