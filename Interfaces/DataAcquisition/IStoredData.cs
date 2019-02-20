﻿namespace Interfaces.DataAcquisition
{
    public interface IStoredData
    {
    }


    public interface IStoredDataOnHDD : IStoredData
    {
        /// <summary>
        /// full path and name of the data on the media
        /// </summary>
        string FullNameOnHDD { get; }
    }


    public interface IStoredDataOnHDDHandler : IStoredDataOnHDD
    {
        /// <summary>
        /// full path and name of the data on the media
        /// </summary>
        new string FullNameOnHDD { get; set; }
    }


}
