using Frame.PluginLoader;
using Interfaces.Evaluation;
using NUnit.Framework;

namespace Test_Matcher
{
    [TestFixture]
    internal class Test_Matching
    {
        private IMatching _matcher;


        [OneTimeSetUp]
        public void OneTimeSetupMatcher()
        {

            PluginLoader loader = new PluginLoader();



            //_matcher = PluginLoader.CreateInstance<IMatching>();
        }




    }
}
