﻿using Interfaces.DataAcquisition;

namespace ME_DummyObjectCreator
{
    internal class ReferenceCreator
    {
        internal void Create(string specificationPath, IHDDFileReaderWriter readerWriter)
        {
            //readerWriter.WriteToFile<IToolSpecification>(specificationHandler, specificationPath);
        }
    }
}