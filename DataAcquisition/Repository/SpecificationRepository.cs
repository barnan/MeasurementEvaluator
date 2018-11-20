using Interfaces.ToolSpecifications;
using Measurement_Evaluator.BLL;
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

        private List<IToolSpecification> SpecificationList;


        public SpecificationRepository(SimpleHDDRepositoryParameter parameters)
            : base(parameters)
        {
            SpecificationList = new List<IToolSpecification>();
        }


        public override IEnumerable<IToolSpecification> GetAll()
        {
            throw new NotImplementedException();
        }

        public override void Remove(IToolSpecification item)
        {
            throw new NotImplementedException();
        }

        public override void RemoveRange(IEnumerable<IToolSpecification> items)
        {
            throw new NotImplementedException();
        }

        public override IEnumerable<IToolSpecification> Find(Expression<Func<IToolSpecification>> predicate)
        {
            throw new NotImplementedException();
        }

        public override IToolSpecification Get(int index, IComparer<IToolSpecification> comprarer = null)
        {
            try
            {
                CheckFolder(_parameters.FullDirectoryPath);

                FileList = Directory.GetFiles(_parameters.FullDirectoryPath).ToList();

                //read them
                List<XmlDocument> documents = new List<XmlDocument>();
                foreach (string item in FileList)
                {
                    IToolSpecification spec = new ToolSpecification();

                    XmlDocument currentDocument = new XmlDocument();
                    currentDocument.LoadXml(item);

                    _parameters.XmlParser.ParseDocument()

                }



                //parse them

                // order them

                // give back the required
            }
            catch (Exception ex)
            {
                _parameters.Logger.Error($"Exception occured: {ex}");
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

    }


}
