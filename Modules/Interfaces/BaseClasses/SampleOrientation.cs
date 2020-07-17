namespace BaseClasses
{
    public class SampleOrientation
    {
        /// <summary>
        /// angle of the sample rotation
        /// </summary>
        protected double Angle { get; set; }

        protected Units Unit => Units.deg;

        public SampleOrientation(double angle)
        {
            Angle = angle;
        }
    }


    // type safe enum
    public sealed class FixedSampleOrientation : SampleOrientation
    {
        private FixedSampleOrientation(double angle)
            : base(angle)
        {
        }

        public static FixedSampleOrientation Deg0 = new FixedSampleOrientation(0);
        public static FixedSampleOrientation Deg90 = new FixedSampleOrientation(90);
        public static FixedSampleOrientation Deg180 = new FixedSampleOrientation(180);
        public static FixedSampleOrientation Deg270 = new FixedSampleOrientation(270);
    }

}
