using Interfaces.ReferenceSample;
using System;
using System.Collections.Generic;

namespace DataAcquisition.Repository
{
    class HDDReferenceRepository : HDDRepository<IReferenceSample>
    {
        public HDDReferenceRepository(SimpleHDDRepositoryParameter parameters)
            : base(parameters)
        {
        }


        protected override List<IReferenceSample> GetItemList(string fullPath)
        {
            throw new NotImplementedException();
        }


    }
}
