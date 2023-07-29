using System;
using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;

namespace BBL.Models.TipoAnexos;

public class TipoAnexo : AuditedEntity<int>
{
    public string Nome { get; set; }
}