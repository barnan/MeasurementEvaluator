using Interfaces.ReferenceSample;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace DataAcquisition.Repository
{
    class ReferenceRepository : SimpleHDDRepository<IReferenceSample>
    {
        public override IEnumerable<IReferenceSample> GetAll()
        {
            throw new NotImplementedException();
        }

        public override void Remove(IReferenceSample item)
        {
            throw new NotImplementedException();
        }

        public override void RemoveRange(IEnumerable<IReferenceSample> items)
        {
            throw new NotImplementedException();
        }

        public override IEnumerable<IReferenceSample> Find(Expression<Func<IReferenceSample>> predicate)
        {
            throw new NotImplementedException();
        }

        public override IReferenceSample Get(int index)
        {
            throw new NotImplementedException();
        }

        public override void Add(IReferenceSample item)
        {
            throw new NotImplementedException();
        }

        public override void AddRange(IEnumerable<IReferenceSample> items)
        {
            throw new NotImplementedException();
        }
    }
}
