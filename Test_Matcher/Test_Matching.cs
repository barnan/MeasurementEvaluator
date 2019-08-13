using Frame.PluginLoader;
using Interfaces.Evaluation;
using MeasurementEvaluator.ME_Matching;
using NUnit.Framework;
using System.Collections.Generic;

namespace Test_Matcher
{
    [TestFixture]
    internal class Test_Matching
    {

        List<PairingKeyValuePair> _pairList;

        private IMatching _matcher;


        [OneTimeSetUp]
        public void OneTimeSetupMatcher()
        {
            PluginLoader loader = new PluginLoader();


            Create();
        }

        [OneTimeTearDown]
        public void OneTimeTearDownMatcher()
        {
            Create();
        }



        internal void Create()
        {

            PairingKeyValuePair ttrThicknessStdAbs = new PairingKeyValuePair("Thickness Average Std Abs Condition", new List<string> { "TTR,AvrThickness", "Thickness[um]" }, "Thickness Average");
            PairingKeyValuePair ttrThicknessStdRel = new PairingKeyValuePair("Thickness Average Std Rel Condition", new List<string> { "TTR,AvrThickness", "Thickness[um]" }, "Thickness Average");

            PairingKeyValuePair ttrResistivityStdRel = new PairingKeyValuePair("Resistivity Std Rel Condition", new List<string> { "Resistivity[Ohmcm]" }, "Resistivity Average");

            _pairList = new List<PairingKeyValuePair>();
            _pairList.Add(ttrThicknessStdAbs);
            _pairList.Add(ttrThicknessStdRel);
            _pairList.Add(ttrResistivityStdRel);

        }



        internal void Clear()
        {
            _pairList = null;

        }



    }
}
