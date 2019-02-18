using MeasurementEvaluatorUI.Base;

namespace MeasurementEvaluatorUI.SampleOrientationUIWPF
{
    class SampleOrientationViewModel : ViewModelBase
    {





        #region properties

        private bool _is_0_degreeVisible;
        public bool Is0DegreeVisible
        {
            get { return _is_0_degreeVisible; }
            set { _is_0_degreeVisible = value; }
        }

        private bool _is_90_degreevisible;
        public bool Is90Degreevisible
        {
            get { return _is_90_degreevisible; }
            set { _is_90_degreevisible = value; }
        }

        private bool _is_180_degreeVisible;
        public bool Is180DegreeVisible
        {
            get { return _is_180_degreeVisible; }
            set { _is_180_degreeVisible = value; }
        }

        private bool _is_270_degreevisible;
        public bool Is270Degreevisible
        {
            get { return _is_270_degreevisible; }
            set { _is_270_degreevisible = value; }
        }

        private bool _isdefectOrderFlipVisible;
        public bool IsdefectOrderFlipVisible
        {
            get { return _isdefectOrderFlipVisible; }
            set { _isdefectOrderFlipVisible = value; }
        }



        #endregion


    }
}
