using NLog;

namespace DataAcquisitions.HDDXmlBinarySerializator
{
    class HDDXmlBinarySerializatorParameters
    {
        internal ILogger Logger { get; }

        public HDDXmlBinarySerializatorParameters(string id)
        {
            LogManager.GetLogger(id);
        }
    }
}
