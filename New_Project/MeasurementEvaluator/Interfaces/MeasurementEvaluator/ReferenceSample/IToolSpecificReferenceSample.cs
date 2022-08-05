
namespace Interfaces.MeasurementEvaluator.ReferenceSample
{
    public interface IWSIReferenceSample : IReferenceSample
    {
        bool DoFlipDefects { get; set; }
    }


    public interface ITTRReferenceSample : IReferenceSample
    {
        bool DoFlipDefects { get; set; }
    }
}
