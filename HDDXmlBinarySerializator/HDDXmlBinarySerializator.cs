using Interfaces;
using Interfaces.DataAcquisition;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace DataAcquisitions.HDDXmlBinarySerializator
{
    internal class HDDXmlBinarySerializator : IHDDFileReaderWriter
    {

        public object ReadFromFile(string fileNameAndPath, ToolNames toolName = null)
        {

            try
            {
                object emp = null;
                BinaryFormatter bf = new BinaryFormatter();
                using (FileStream fsin = new FileStream(fileNameAndPath, FileMode.Open, FileAccess.Read, FileShare.None))
                {
                    emp = bf.Deserialize(fsin);
                }
                return emp;
            }
            catch (Exception)
            {
                return null;
            }
        }


        public bool WriteToFile(object obj, string fileNameAndPath)
        {
            try
            {
                BinaryFormatter bf = new BinaryFormatter();
                using (FileStream fsout = new FileStream(fileNameAndPath, FileMode.Create, FileAccess.Write, FileShare.None))
                {
                    bf.Serialize(fsout, obj);
                }
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }


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

    }
}