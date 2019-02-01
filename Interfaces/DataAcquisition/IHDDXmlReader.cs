namespace Interfaces.DataAcquisition
{
    public interface IHDDXmlReader
    {
        T DeserializeObject<T>(string filePath);
        bool SerializeObject<T>(T obj, string filePath);
    }
}
