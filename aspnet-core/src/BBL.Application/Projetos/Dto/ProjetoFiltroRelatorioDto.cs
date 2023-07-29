using System;
using System.Collections.Generic;

namespace BBL.Projetos.Dto;

public class ProjetoFiltroRelatorioDto
{
    public IList<int> parceiros { get; set; }
    public IList<int> fases { get; set; }
    public DateTime DataInicio { get; set; }
    public DateTime DataFim { get; set; }
}