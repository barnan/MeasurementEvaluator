using Interfaces;
using Interfaces.ToolSpecifications;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataStructures.ToolSpecifications
{

    public class QuantitySpecification : IQuantitySpecification
    {
        public string QuantityName { get; }
        public Units Dimension { get; }
        public IReadOnlyList<ICondition> Conditions { get; }



        public QuantitySpecification(IReadOnlyList<ICondition> conditions, Units dimension, string quantityName)
        {
            Conditions = conditions;
            Dimension = dimension;
            QuantityName = quantityName;
        }

        #region IFormattable

        public string ToString(string format, IFormatProvider formatProvider)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(QuantityName);
            sb.AppendLine(Dimension.ToString());

            foreach (var item in Conditions)
            {
                sb.Append(item);
            }

            return sb.ToString();
        }

        #endregion

        #region IComparable

        public int CompareTo(IQuantitySpecification other)
        {
            if (ReferenceEquals(this, other))
            {
                return 0;
            }

            if (ReferenceEquals(null, other))
            {
                return 1;
            }

            if (other.Conditions.Count == Conditions.Count)
            {
                return 0;
            }

            return Conditions.Count > other.Conditions.Count ? 1 : -1;
        }

        #endregion

        //public string QuantityName { get; set; }

        //public string Dimension
        //{
        //    get
        //    {
        //        string dim = AccurAbsolute.Dimension;
        //        if (string.Compare(AccurRelative.Dimension, StdAbsolute.Dimension, StringComparison.InvariantCultureIgnoreCase) == 0)
        //            return AccurRelative.Dimension;

        //        return string.Empty;
        //    }
        //}


        //public UsualCondition AccurAbs { get; set; }
        //[XmlIgnore]
        //public IUsualCondition AccurAbsolute
        //{
        //    get { return AccurAbs; }
        //    set
        //    {
        //        AccurAbs = value as UsualCondition;
        //    }
        //}


        //public UsualCondition AccurRel { get; set; }
        //[XmlIgnore]
        //public IUsualCondition AccurRelative
        //{
        //    get { return AccurRel; }
        //    set { AccurRel = value as UsualCondition; }
        //}

        //public UsualCondition StdAbs { get; set; }
        //[XmlIgnore]
        //public IUsualCondition StdAbsolute
        //{
        //    get { return StdAbs; }
        //    set { StdAbs = value as UsualCondition; }
        //}

        //public UsualCondition StdRel { get; set; }
        //[XmlIgnore]
        //public IUsualCondition StdRelative
        //{
        //    get { return StdRel; }
        //    set { StdRel = value as UsualCondition; }
        //}

        //public CpkCondition ProcCapab { get; set; }
        //[XmlIgnore]
        //public ICpkCondition Cpk
        //{
        //    get { return ProcCapab; }
        //    set { ProcCapab = value as CpkCondition; }
        //}


        //public QuantitySpecification()
        //{
        //}


        //public QuantitySpecification(string name, IUsualCondition cond1, IUsualCondition cond2, IUsualCondition cond3, IUsualCondition cond4, ICpkCondition cpk)
        //{
        //    this.QuantityName = name;
        //    this.AccurAbsolute = cond1;
        //    this.AccurRelative = cond2;
        //    this.StdAbsolute = cond3;
        //    this.StdRelative = cond4;
        //    this.Cpk = cpk;
        //}

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
