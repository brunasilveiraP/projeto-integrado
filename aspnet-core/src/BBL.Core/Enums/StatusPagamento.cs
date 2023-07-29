using System.ComponentModel;

namespace BBL.Enums;

public enum StatusPagamento
{
    [Description("Em Aberto")]
    EmAberto = 1,
    [Description("Pago")]
    Pago = 2,
    [Description("Não faturado")]
    NaoFaturado = 4
}