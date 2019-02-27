using Interfaces.DataAcquisition;
using Interfaces.MeasuredData;
using Interfaces.Misc;
using Interfaces.ReferenceSample;
using Interfaces.ToolSpecifications;
using Miscellaneous;
using NLog;
using PluginLoading;

namespace DataAcquisitions.DataCollector
{
    internal class DataCollectorParameters
    {

        //private IRepository<IToolSpecification> _specificationRepository;
        //internal IRepository<IToolSpecification> SpecificationRepository => _specificationRepository;

        //private IRepository<IReferenceSample> _referenceRepository;
        //internal IRepository<IReferenceSample> ReferenceRepository => _referenceRepository;

        //private IRepository<IToolMeasurementData> _measurementDataRepository;
        //internal IRepository<IToolMeasurementData> MeasurementDataRepository => _measurementDataRepository;

        //private IDateTimeProvider _dateTimeProvider;
        //internal IDateTimeProvider DateTimeProvider => _dateTimeProvider;

        internal ILogger Logger { get; }

        [Configuration("Date and time Provider", "Date and time Provider", true)]
        private string _dateTimeProvider;
        internal IDateTimeProvider DateTimeProvider { get; }

        [Configuration("Measurement Data Repository", "Measurement Data Repository", true)]
        private string _measurementDataRepository;
        internal IRepository<IToolMeasurementData> MeasurementDataRepository { get; }

        [Configuration("Reference Repository", "Reference Repository", true)]
        private string _referenceRepository;
        internal IRepository<IReferenceSample> ReferenceRepository { get; }

        [Configuration("Specification Repository", "Specification Repository", true)]
        private string _specificationRepository;

        internal IRepository<IToolSpecification> SpecificationRepository { get; }

        public DataCollectorParameters()
        {
            Logger = LogManager.GetCurrentClassLogger();

            SpecificationRepository = PluginLoader.CreateInstance<IRepository<IToolSpecification>>(_specificationRepository);
            ReferenceRepository = PluginLoader.CreateInstance<IRepository<IReferenceSample>>(_referenceRepository);
            MeasurementDataRepository = PluginLoader.CreateInstance<IRepository<IToolMeasurementData>>(_measurementDataRepository);
            DateTimeProvider = PluginLoader.CreateInstance<IDateTimeProvider>(_dateTimeProvider);

        }

    }
}
