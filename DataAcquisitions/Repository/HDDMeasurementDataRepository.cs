using Frame.PluginLoader.Interfaces;
using Interfaces.DataAcquisition;
using Interfaces.MeasuredData;
using System;
using System.Collections.Generic;

namespace DataAcquisitions.Repository
{
    internal class HDDMeasurementDataRepository : HDDTypedRepository<IToolMeasurementData>
    {

        internal HDDMeasurementDataRepository(HDDRepositoryParameters parameters)
        : base(parameters)
        {
        }

    }


    public class HDDMeasurementDataRepositoryFactory : IPluginFactory
    {
        private readonly Dictionary<string, IRepository<IToolMeasurementData>> _specificationRepositoryDictionary = new Dictionary<string, IRepository<IToolMeasurementData>>();


        public object Create(Type t, string name)
        {
            if (t.IsAssignableFrom(typeof(HDDMeasurementDataRepository)))
            {
                HDDRepositoryParameters parameters = new HDDRepositoryParameters();
                if (parameters.Load())
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
