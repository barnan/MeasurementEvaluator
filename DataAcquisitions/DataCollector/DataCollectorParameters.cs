using Interfaces.DataAcquisition;
using Interfaces.MeasuredData;
using Interfaces.Misc;
using Interfaces.ReferenceSample;
using Interfaces.ToolSpecifications;
using NLog;

namespace DataAcquisitions.DataCollector
{
    internal class DataCollectorParameters
    {

        internal IRepository<IToolSpecification> SpecificationRepository { get; }

        internal IRepository<IReferenceSample> ReferenceRepository { get; }

        internal IRepository<IToolMeasurementData> MeasurementDataRepository { get; }

        internal ILogger Logger { get; }

        internal IDateTimeProvider DateTimeProvider { get; }


        public DataCollectorParameters(IRepository<IToolMeasurementData> measurementRepository, IRepository<IReferenceSample> referenceRepository, IRepository<IToolSpecification> specificationRepository)
        {
            Logger = LogManager.GetCurrentClassLogger();

            SpecificationRepository = specificationRepository;
            ReferenceRepository = referenceRepository;
            MeasurementDataRepository = measurementRepository;
        }

    }
}
