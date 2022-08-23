using BaseClasses.Repository;

namespace HDDConfigRespository
{
    internal class HDDConfigRepositoryParameters : GenericHDDRepositoryParameters
    {

        internal bool Load(string sectionName)
        {
            base.Load(sectionName);

            return true;
        }

    }
}
