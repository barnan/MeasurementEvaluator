using NLog;

namespace DataAcquisitions.HDDXmlSerializator
{
    internal class HDDXmlSerializatorParameters
    {
        internal ILogger Logger { get; }

        public HDDXmlSerializatorParameters(string id)
        {
            Logger = LogManager.GetLogger(id);
        }

    }
}
