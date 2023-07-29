using System.ComponentModel;

namespace BBL.Enums;

public enum TipoPessoa
{
    [Description("Pessoa Física")]
    PF = 1,
    [Description("Pessoa Jurídica")]
    PJ = 2,
}