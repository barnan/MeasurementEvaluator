using DataStructures.ToolSpecifications;
using Interfaces.ToolSpecifications;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Xml;

namespace DataAcquisition.Repository
{
    class SpecificationRepository : SimpleHDDRepository<IToolSpecification>
    {


        public SpecificationRepository(SimpleHDDRepositoryParameter parameters)
            : base(parameters)
        {
        }


        public override IEnumerable<IToolSpecification> GetAll()
        {
            try
            {
                List<IToolSpecification> SpecificationList = GetSpecificationList(_parameters.FullDirectoryPath);

                return SpecificationList;
            }
            catch (Exception ex)
            {
                _parameters.Logger.Error($"Exception occured: {ex}");
                return null;
            }
        }


        public override void Remove(IToolSpecification item)
        {
            try
            {
                List<IToolSpecification> SpecificationList = GetSpecificationList(_parameters.FullDirectoryPath);

                SpecificationList.Remove(item);
            }
            catch (Exception ex)
            {
                _parameters.Logger.Error($"Exception occured: {ex}");
                return;
            }
        }




        public override void RemoveRange(IEnumerable<IToolSpecification> items)
        {

        }

        public override IEnumerable<IToolSpecification> Find(Expression<Func<IToolSpecification>> predicate)
        {
            throw new NotImplementedException();
        }


        public override IToolSpecification Get(int index, IComparer<IToolSpecification> comprarer = null)
        {
            try
            {
                if (index < 0)
                {
                    _parameters.Logger.Error("The arrived index is below 0..");
                    return null;
                }

                List<IToolSpecification> SpecificationList = GetSpecificationList(_parameters.FullDirectoryPath);

                SpecificationList.Sort();

                if (index > SpecificationList.Count)
                {
                    _parameters.Logger.Error("The arrived index is higher than the length of the specification list.");
                    return null;
                }

                return SpecificationList[index];

            }
            catch (Exception ex)
            {
                _parameters.Logger.Error($"Exception occured: {ex}");
                return null;
            }
        }


        public override void Add(IToolSpecification item)
        {
            throw new NotImplementedException();
        }

        public override void AddRange(IEnumerable<IToolSpecification> items)
        {
            throw new NotImplementedException();
        }





        private List<IToolSpecification> GetSpecificationList(string fullPath)
        {
            if (!CheckFolder(_parameters.FullDirectoryPath))
            {
                _parameters.Logger.Error($"The given folder does not exists: {fullPath}");
                return null;
            }

            List<string> FileList = Directory.GetFiles(_parameters.FullDirectoryPath).ToList();
            List<IToolSpecification> SpecificationList = new List<IToolSpecification>();
            List<XmlDocument> documents = new List<XmlDocument>();

            foreach (string item in FileList)
            {
                IToolSpecification spec = new ToolSpecification();

                XmlDocument currentXmlDocument = new XmlDocument();
                currentXmlDocument.LoadXml(item);

                _parameters.XmlParser.ParseDocument(spec, currentXmlDocument);

                SpecificationList.Add(spec);

            }

            if (_parameters.Logger.IsTraceEnabled)
            {
                foreach (var item in SpecificationList)
                {
                    _parameters.Logger.Trace(item.ToString());
                }
            }


            return SpecificationList;
        }



    }
}
