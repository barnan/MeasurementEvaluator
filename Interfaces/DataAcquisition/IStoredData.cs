namespace Interfaces.DataAcquisition
{
    public interface IStoredData
    {
        /// <summary>
        /// full path and name of the data on the media
        /// </summary>
        string FullNameOnStorage { get; }
    }


    public interface IStoredDataHandler : IStoredData
    {
        /// <summary>
        /// full path and name of the data on the media
        /// </summary>
        string FullNameOnStorage { get; set; }
    }

}
