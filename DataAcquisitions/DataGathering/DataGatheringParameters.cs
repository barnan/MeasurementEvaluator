using Interfaces.DataAcquisition;
using Interfaces.MeasuredData;
using Interfaces.ReferenceSample;
using Interfaces.ToolSpecifications;
using NLog;

namespace DataAcquisitions.DataGathering
{
    internal class DataGatheringParameters
    {

        internal IRepository<IToolSpecification> SpecificationRepository { get; }

        internal IRepository<IReferenceSample> ReferenceRepository { get; }

        internal IRepository<IToolMeasurementData> MeasurementDataRepository { get; }

        internal ILogger Logger { get; }


        public DataGatheringParameters(IRepository<IToolMeasurementData> measurementRepository, IRepository<IReferenceSample> referenceRepository, IRepository<IToolSpecification> specificationRepository)
        {
            Logger = LogManager.GetCurrentClassLogger();

            SpecificationRepository = specificationRepository;
            ReferenceRepository = referenceRepository;
            MeasurementDataRepository = measurementRepository;
        }

    }
}
