using Interfaces.Misc;
using ME_DummyObjectCreator;

namespace MeasurementEvaluator.ME_DummyObjectCreator
{
    public class DummyObjectCreator : IDummyObjectCreator
    {
        private DummyObjectCreatorParameters _parameters;
        private SpecificationCreator _specCreator;
        private ReferenceCreator _refCreator;

        internal DummyObjectCreator(DummyObjectCreatorParameters param)
        {
            _parameters = param;

            _specCreator = new SpecificationCreator();
            _refCreator = new ReferenceCreator();
        }

        public void Create(string specificationPath, string referencePath, string measDataPath)
        {
            // thickness avg conditions:
            _specCreator.Create(specificationPath, _parameters.HDDFileReaderWriter);
            _refCreator.Create(referencePath, _parameters.HDDFileReaderWriter);


        }
    }
}
