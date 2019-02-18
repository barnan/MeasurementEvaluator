﻿using Interfaces;
using Interfaces.DataAcquisition;
using System.IO;

namespace DataAcquisitions.DAL
{

    public abstract class HDDFileReaderWriterBase : IHDDFileReader, IHDDFileWriter
    {

        protected readonly object _lockObject = new object();



        public abstract bool WriteToFile<T>(T obj, string fileNameAndPath);

        public abstract T ReadFromFile<T>(string fileNameAndPath, ToolNames toolName = null);



        public bool CanRead(string fileNameAndPath)
        {
            if (!string.IsNullOrEmpty(fileNameAndPath))
            {
                using (FileStream fstream = new FileStream(fileNameAndPath, FileMode.Open, FileAccess.Read))
                {
                    if (fstream.CanRead)
                    {
                        return true;
                    }
                }
            }
            return false;
        }


        protected bool CheckFilePath(string fileNameAndPath)
        {
            return File.Exists(fileNameAndPath);
        }
    }
}