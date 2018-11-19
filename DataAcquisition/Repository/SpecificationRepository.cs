using Interfaces.ToolSpecifications;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace DataAcquisition.Repository
{
    class SpecificationRepository : SimpleHDDRepository<IToolSpecification>
    {

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

        public override IToolSpecification Get(int index, )
        {
            // check folder

            // read files in folder

            // order them

            // give back the required


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
