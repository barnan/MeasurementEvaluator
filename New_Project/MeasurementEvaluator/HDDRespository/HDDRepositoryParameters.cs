using BaseClasses.Repository;

namespace HDDRespository
{
    internal class HDDRepositoryParameters : GenericHDDRepositoryParameters
    {

        internal bool Load(string sectionName)
        {
            base.Load(sectionName);

            return true;
        }

    }
}
