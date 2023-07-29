using System.ComponentModel;

namespace BBL.Enums;

public enum TipoGeracao
{
    [Description("Geração Própria")]
    GeracaoPropria = 1,
    [Description("Auto Consumo Remoto")]
    AutoConsumoRemoto = 2,
    [Description("Geração Compartilhada")]
    GeracaoCompartilhada = 4,
    [Description("Emuc")]
    Emuc = 8
}