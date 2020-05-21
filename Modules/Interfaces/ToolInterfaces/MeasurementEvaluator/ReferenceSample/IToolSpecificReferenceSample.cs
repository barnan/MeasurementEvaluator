namespace ToolSpecificInterfaces.MeasurementEvaluator.ReferenceSample
{

    public interface IFlippableSample
    {
        bool DoFlipDefects { get; set; }
    }


    public interface IWsiReferenceSample : IReferenceSample, IFlippableSample
    {
    }


    public interface ITtrReferenceSample : IReferenceSample, IFlippableSample
    {
    }
}
