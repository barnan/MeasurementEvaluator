using Interfaces.ToolSpecifications;
using Measurement_Evaluator.BLL;
using System;
using System.Xml.Serialization;

namespace DataAcquisition
{

    public class QuantitySpecification : IQuantitySpecification
    {

        public string QuantityName { get; set; }

        public string Dimension
        {
            get
            {
                string dim = AccurAbsolute.Dimension;
                if (string.Compare(AccurRelative.Dimension, StdAbsolute.Dimension, StringComparison.InvariantCultureIgnoreCase) == 0)
                    return AccurRelative.Dimension;

                return string.Empty;
            }
        }


        public UsualCondition AccurAbs { get; set; }
        [XmlIgnore]
        public IUsualCondition AccurAbsolute
        {
            get { return AccurAbs; }
            set
            {
                AccurAbs = value as UsualCondition;
            }
        }


        public UsualCondition AccurRel { get; set; }
        [XmlIgnore]
        public IUsualCondition AccurRelative
        {
            get { return AccurRel; }
            set { AccurRel = value as UsualCondition; }
        }

        public UsualCondition StdAbs { get; set; }
        [XmlIgnore]
        public IUsualCondition StdAbsolute
        {
            get { return StdAbs; }
            set { StdAbs = value as UsualCondition; }
        }

        public UsualCondition StdRel { get; set; }
        [XmlIgnore]
        public IUsualCondition StdRelative
        {
            get { return StdRel; }
            set { StdRel = value as UsualCondition; }
        }

        public CpkCondition ProcCapab { get; set; }
        [XmlIgnore]
        public ICpkCondition Cpk
        {
            get { return ProcCapab; }
            set { ProcCapab = value as CpkCondition; }
        }


        public QuantitySpecification()
        {
        }


        public QuantitySpecification(string name, IUsualCondition cond1, IUsualCondition cond2, IUsualCondition cond3, IUsualCondition cond4, ICpkCondition cpk)
        {
            this.QuantityName = name;
            this.AccurAbsolute = cond1;
            this.AccurRelative = cond2;
            this.StdAbsolute = cond3;
            this.StdRelative = cond4;
            this.Cpk = cpk;
        }

        //public void Save(string FileName)
        //{
        //    using (var writer = new StreamWriter(FileName))
        //    {
        //        var serializer = new XmlSerializer(this.GetType());
        //        serializer.Serialize(writer, this);
        //        writer.Flush();
        //    }
        //}

    }
}
