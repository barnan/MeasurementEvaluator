namespace Interfaces.DataAcquisition
{
    public interface IStoredData
    {
    }


    public interface IStoredDataOnHDD : IStoredData
    {
        /// <summary>
        /// full path and name of the data on the media
        /// </summary>
        string FullPathOnHDD { get; }
    }


    public interface IStoredDataOnHDDHandler : IStoredDataOnHDD
    {
        /// <summary>
        /// full path and name of the data on the media
        /// </summary>
        new string FullPathOnHDD { get; set; }
    }


}
