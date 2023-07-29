using System.ComponentModel;

namespace BBL.Enums;

public enum TipoAterramento
{
    [Description("TR")]
    Tr = 1,
    [Description("TNS")]
    Tns = 2,
    [Description("TNST")]
    Tnst = 4,
    [Description("TT")]
    Tt = 8
}