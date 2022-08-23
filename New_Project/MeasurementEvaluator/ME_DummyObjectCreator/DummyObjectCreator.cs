using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Interfaces.Misc;

namespace ME_DummyObjectCreator
{
    public class DummyObjectCreator : IDummyObjectCreator
    {
        private DummyObjectCreatorParameters _parameters;
        private SpecificationCreator _specCreator;
        private ReferenceCreator _refCreator;
        private PairingDictCreator _pairCreator;

        internal DummyObjectCreator(DummyObjectCreatorParameters param)
        {
            _parameters = param;

            _specCreator = new SpecificationCreator();
            _refCreator = new ReferenceCreator();
            _pairCreator = new PairingDictCreator();
        }

        public void Create(string specificationPath, string referencePath, string measDataPath)
        {
            _specCreator.Create(specificationPath, _parameters.HDDFileReaderWriter1);
            _refCreator.Create(referencePath, _parameters.HDDFileReaderWriter1);
            _pairCreator.Create(specificationPath, _parameters.HDDFileReaderWriter2);
        }
    }
}
