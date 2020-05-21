namespace Interfaces.BaseClasses
{
    //public sealed class ToolNames : IXmlStorable
    //{

    //    private const string XELEMENT_TOOLNAME = "ToolName";

    //    public string Name { get; private set; }
    //    public int Value { get; private set; }

    //    public ToolNames(string name, int value)
    //    {
    //        Name = name;
    //        Value = value;
    //    }

    //    public ToolNames()
    //    {
    //    }

    //    public static ToolNames Unknown = new ToolNames(nameof(Unknown), 0);
    //    public static ToolNames TTR = new ToolNames(nameof(TTR), 1);
    //    public static ToolNames SHP = new ToolNames(nameof(SHP), 2);
    //    public static ToolNames WSIChipping = new ToolNames(nameof(WSIChipping), 3);
    //    public static ToolNames WSIContamination = new ToolNames(nameof(WSIContamination), 4);
    //    public static ToolNames PED = new ToolNames(nameof(PED), 5);
    //    public static ToolNames MCI = new ToolNames(nameof(MCI), 6);
    //    public static ToolNames PLI = new ToolNames(nameof(PLI), 7);
    //    public static ToolNames UPCD = new ToolNames(nameof(UPCD), 8);

    //    public override string ToString()
    //    {
    //        return Name;
    //    }

    //    public XElement SaveToXml(XElement inputElement)
    //    {
    //        inputElement.SetAttributeValue(XELEMENT_TOOLNAME, Name);
    //        return inputElement;
    //    }

    //    public bool LoadFromXml(XElement inputElement)
    //    {
    //        string attributeValue = inputElement.Attribute(XELEMENT_TOOLNAME)?.Value;
    //        var element = (ToolNames)attributeValue;

    //        if (element == null)
    //        {
    //            return false;
    //        }

    //        Name = element.Name;
    //        Value = element.Value;
    //        return true;
    //    }

    //    public static explicit operator ToolNames(string val)
    //    {
    //        if (val == Unknown.Name)
    //        {
    //            return Unknown;
    //        }
    //        if (val == TTR.Name)
    //        {
    //            return TTR;
    //        }
    //        if (val == SHP.Name)
    //        {
    //            return SHP;
    //        }
    //        if (val == WSIChipping.Name)
    //        {
    //            return WSIChipping;
    //        }
    //        if (val == WSIContamination.Name)
    //        {
    //            return WSIContamination;
    //        }
    //        if (val == PED.Name)
    //        {
    //            return PED;
    //        }
    //        if (val == MCI.Name)
    //        {
    //            return MCI;
    //        }
    //        if (val == PLI.Name)
    //        {
    //            return PLI;
    //        }
    //        if (val == UPCD.Name)
    //        {
    //            return UPCD;
    //        }
    //        return null;
    //    }

    //    public override bool Equals(object obj)
    //    {
    //        if (obj == null)
    //        {
    //            return false;
    //        }

    //        if (!(obj is ToolNames otherToolName))
    //        {
    //            return false;
    //        }

    //        return Value == otherToolName.Value && Name == otherToolName.Name;
    //    }

    //    public override int GetHashCode()
    //    {
    //        return Value.GetHashCode() + Name.GetHashCode();
    //    }

    //    public static bool operator ==(ToolNames toolName1, ToolNames toolName2)
    //    {
    //        if (object.Equals(toolName1, null) && object.Equals(toolName2, null))
    //            return true;

    //        return !object.Equals(toolName1, null) && !object.Equals(toolName2, null) && toolName1.Value == toolName2.Value && toolName1.Name == toolName2.Name;
    //    }

    //    public static bool operator !=(ToolNames toolName1, ToolNames ToolName2)
    //    {
    //        return !(toolName1 == ToolName2);
    //    }
    //}

}
