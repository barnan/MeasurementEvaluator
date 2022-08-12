using System.Xml.Linq;
using FrameInterfaces;
using Interfaces.DataAcquisition;
using Interfaces.Misc;

namespace HDDRespository
{
    public class Factory : IPluginFactory
    {
        readonly Dictionary<string, IRepository<INamedContent<XElement>>> _repositories = new Dictionary<string, IRepository<INamedContent<XElement>>>();


        public object Create(Type t, string name)
        {
            if (t.IsAssignableFrom(typeof(IRepository<INamedContent<XElement>>)))
            {
                if (!_repositories.ContainsKey(name))
                {
                    HDDRepositoryParameters param = new HDDRepositoryParameters();
                    if (param.Load(name))
                    {
                        IRepository<INamedContent<XElement>> instance = new HDDRepository(param);
                        _repositories.Add(name, instance);
                        return instance;
                    }
                }
                else
                {
                    return _repositories[name];
                }
            }

            return null;
        }
    }
}
