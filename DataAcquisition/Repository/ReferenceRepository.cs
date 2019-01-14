using Interfaces.ReferenceSample;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace DataAcquisition.Repository
{
    class ReferenceRepository : HDDRepository<IReferenceSample>
    {
        public ReferenceRepository(SimpleHDDRepositoryParameter parameters)
            : base(parameters)
        {
        }


        public override IEnumerable<IReferenceSample> GetAll()
        {
            throw new NotImplementedException();
        }

        public override bool Remove(IReferenceSample item)
        {
            throw new NotImplementedException();
        }

        public override IEnumerable<IReferenceSample> Find(Expression<Func<IReferenceSample>> predicate)
        {
            throw new NotImplementedException();
        }

        public override IReferenceSample Get(int index, IComparer<IReferenceSample> comparer = null)
        {
            throw new NotImplementedException();
        }

        public override bool Add(IReferenceSample item)
        {
            throw new NotImplementedException();
        }


        protected override List<IReferenceSample> GetItemList(string fullPath)
        {
            throw new NotImplementedException();
        }


    }
}
