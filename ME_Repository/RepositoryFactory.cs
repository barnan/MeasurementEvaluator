﻿namespace DataAcquisitions.ME_Repository
{
    //class RepositoryFactory<T>
    //    where T : class, IComparable<T>, INamed
    //{
    //    Dictionary<string, IRepository<IToolSpecification>> _specificationRepositoryDictionary = new Dictionary<string, IRepository<IToolSpecification>>();
    //    Dictionary<string, IRepository<IReferenceSample>> _referenceRepositoryDictionary = new Dictionary<string, IRepository<IReferenceSample>>();
    //    Dictionary<string, IRepository<IToolMeasurementData>> _measurementDataRepositoryDictionary = new Dictionary<string, IRepository<IToolMeasurementData>>();


    //    public object Create(Type t, string name)
    //    {
    //        if (t.IsAssignableFrom(typeof(HDDSpecificationRepository)) || t.IsAssignableFrom(typeof(HDDMeasurementDataRepository)) || t.IsAssignableFrom(typeof(HDDReferenceRepository)))
    //        {

    //            if (t.IsAssignableFrom(typeof(HDDSpecificationRepository)))
    //            {
    //                HDDRepositoryParameters parameters = new HDDRepositoryParameters();
    //                if (parameters.Load())
    //                {
    //                    HDDSpecificationRepository instance = new HDDSpecificationRepository(parameters);
    //                    _specificationRepositoryDictionary.Add(name, instance);
    //                }

    //                return _specificationRepositoryDictionary[name];
    //            }

    //            if (t.IsAssignableFrom(typeof(HDDReferenceRepository)))
    //            {
    //                HDDRepositoryParameters parameters = new HDDRepositoryParameters();
    //                if (parameters.Load())
    //                {
    //                    HDDReferenceRepository instance = new HDDReferenceRepository(parameters);
    //                    _referenceRepositoryDictionary.Add(name, instance);
    //                }

    //                return _referenceRepositoryDictionary[name];
    //            }

    //            if (t.IsAssignableFrom(typeof(HDDMeasurementDataRepository)))
    //            {
    //                HDDRepositoryParameters parameters = new HDDRepositoryParameters();
    //                if (parameters.Load())
    //                {
    //                    HDDMeasurementDataRepository instance = new HDDMeasurementDataRepository(parameters);
    //                    _measurementDataRepositoryDictionary.Add(name, instance);
    //                }

    //                return _measurementDataRepositoryDictionary[name];
    //            }
    //        }
    //        return null;
    //    }
    //}
}