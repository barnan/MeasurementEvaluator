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

        internal ILogger Logger { get; private set; }

        [Configuration("Name of date and time provider", "Date and time Provider", true)]
        private string _dateTimeProvider;
        internal IDateTimeProvider DateTimeProvider { get; private set; }

        [Configuration("Name of the measurement Data repository", Name = "Measurement Data Repository", LoadComponent = true)]
        private string _measurementDataRepository;
        internal IRepository<IToolMeasurementData> MeasurementDataRepository { get; private set; }

        [Configuration("Name of the reference repository", Name = "Reference Repository", LoadComponent = true)]
        private string _referenceRepository;
        internal IRepository<IReferenceSample> ReferenceRepository { get; private set; }

        [Configuration("Name of the specification repository", Name = "Specification Repository", LoadComponent = true)]
        private string _specificationRepository;
        internal IRepository<IToolSpecification> SpecificationRepository { get; private set; }



        internal bool Load()
        {
            Logger = LogManager.GetCurrentClassLogger();

            SpecificationRepository = PluginLoader.CreateInstance<IRepository<IToolSpecification>>(_specificationRepository);
            ReferenceRepository = PluginLoader.CreateInstance<IRepository<IReferenceSample>>(_referenceRepository);
            MeasurementDataRepository = PluginLoader.CreateInstance<IRepository<IToolMeasurementData>>(_measurementDataRepository);
            DateTimeProvider = PluginLoader.CreateInstance<IDateTimeProvider>(_dateTimeProvider);

            return CheckComponent();
        }

        private bool CheckComponent()
        {
            if (SpecificationRepository == null)
            {
                Logger.Error($"Error in the {nameof(DataCollectorParameters)} instantiation. {nameof(SpecificationRepository)} is null.");
                return false;
            }

            if (ReferenceRepository == null)
            {
                Logger.Error($"Error in the {nameof(DataCollectorParameters)} instantiation. {nameof(ReferenceRepository)} is null.");
                return false;
            }


            if (MeasurementDataRepository == null)
            {
                Logger.Error($"Error in the {nameof(DataCollectorParameters)} instantiation. {nameof(MeasurementDataRepository)} is null.");
                return false;
            }

            if (DateTimeProvider == null)
            {
                Logger.Error($"Error in the {nameof(DataCollectorParameters)} instantiation. {nameof(DateTimeProvider)} is null.");
                return false;
            }

            return true;
        }
    }
}
