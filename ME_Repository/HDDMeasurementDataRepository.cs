//using Frame.PluginLoader.Interfaces;
//using Interfaces.DataAcquisition;
//using Interfaces.MeasuredData;
//using Miscellaneous;
//using System;
//using System.Collections.Generic;
//using System.IO;
//using System.Linq;
//using System.Xml.Linq;

//namespace DataAcquisitions.ME_Repository
//{
//    internal class HDDMeasurementDataRepository : HDDRepository<IToolMeasurementData>, IMeasurementDataRepository
//    {

//        internal HDDMeasurementDataRepository(HDDRepositoryParameters parameters)
//        : base(parameters)
//        {
//            _repositoryPath = Frame.PluginLoader.PluginLoader.MeasurementDataFolder;
//        }

//        protected override List<IReferenceSample> GetItemList(string fullPath)
//        {
//            try
//            {
//                if (!CheckFolder(fullPath))
//                {
//                    _parameters.Logger.MethodError($"The given folder can not be used: {fullPath}");
//                    return null;
//                }

//                List<string> fileNameList = Directory.GetFiles(fullPath, $"*.{_parameters.FileExtensionFilters}").ToList();
//                List<IReferenceSample> fileContentDictionary = new List<IReferenceSample>(fileNameList.Count);

//                foreach (string fileName in fileNameList)
//                {
//                    XElement inputelement = XElement.Load(fullPath);
//                    IReferenceSample reference = new ReferenceSample();
//                    reference.LoadFromXml(inputelement);

//                    fileContentDictionary.Add(reference);

//                    if (_parameters.Logger.IsTraceEnabled)
//                    {
//                        _parameters.Logger.MethodTrace($"File read: {fileName}");
//                    }
//                }

//                if (_parameters.Logger.IsTraceEnabled)
//                {
//                    foreach (var item in fileContentDictionary)
//                    {
//                        _parameters.Logger.MethodTrace($"Items: {item}");
//                    }
//                }

//                return fileContentDictionary;
//            }
//            catch (Exception ex)
//            {
//                _parameters.Logger.MethodError($"Exception occured: {ex}");
//                return null;
//            }
//        }
//    }


//    public class HDDMeasurementDataRepositoryFactory : IPluginFactory
//    {
//        private readonly Dictionary<string, IMeasurementDataRepository> _specificationRepositoryDictionary = new Dictionary<string, IMeasurementDataRepository>();


//        public object Create(Type t, string name)
//        {
//            if (t.IsAssignableFrom(typeof(HDDMeasurementDataRepository)))
//            {
//                HDDRepositoryParameters parameters = new HDDRepositoryParameters();
//                if (parameters.Load(name))
//                {
//                    HDDMeasurementDataRepository instance = new HDDMeasurementDataRepository(parameters);
//                    _specificationRepositoryDictionary.Add(name, instance);
//                }

//                return _specificationRepositoryDictionary[name];
//            }
//            return null;
//        }
//    }
//}
