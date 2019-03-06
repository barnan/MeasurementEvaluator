namespace Interfaces.ToolSpecifications
{
    public interface IRotatable
    {
        /// <summary>
        ///  Sets the orientation to the given value
        /// </summary>
        /// <param name="orientation"></param>
        /// <returns></returns>
        bool SetCurrentOrientation(SampleOrientation orientation);

        /// <summary>
        /// Query the available orientains
        /// </summary>
        /// <returns>returns the available orientation angles in an array</returns>
        SampleOrientation[] GetAvailableOrientations();

    }
}
