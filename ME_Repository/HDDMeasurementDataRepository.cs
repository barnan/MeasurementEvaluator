using Frame.PluginLoader.Interfaces;
using Interfaces.DataAcquisition;
using Interfaces.MeasuredData;
using System;
using System.Collections.Generic;

namespace DataAcquisitions.ME_Repository
{
    internal class HDDMeasurementDataRepository : HDDRepository<IToolMeasurementData>, IMeasurementDataRepository
    {

        internal HDDMeasurementDataRepository(HDDRepositoryParameters parameters)
        : base(parameters)
        {
            _repositoryPath = Frame.PluginLoader.PluginLoader.MeasurementDataFolder;
        }

    }


    public class HDDMeasurementDataRepositoryFactory : IPluginFactory
    {
        private readonly Dictionary<string, IMeasurementDataRepository> _specificationRepositoryDictionary = new Dictionary<string, IMeasurementDataRepository>();


        public object Create(Type t, string name)
        {
            if (t.IsAssignableFrom(typeof(HDDMeasurementDataRepository)))
            {
                HDDRepositoryParameters parameters = new HDDRepositoryParameters();
                if (parameters.Load(name))
                {
                    HDDMeasurementDataRepository instance = new HDDMeasurementDataRepository(parameters);
                    _specificationRepositoryDictionary.Add(name, instance);
                }

                return _specificationRepositoryDictionary[name];
            }
            return null;
        }
    }
}
