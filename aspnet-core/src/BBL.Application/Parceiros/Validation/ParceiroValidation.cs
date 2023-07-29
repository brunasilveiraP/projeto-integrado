using Abp.UI;
using BBL.Parceiros.Dto;
using FluentValidation;

namespace BBL.Parceiros.Validation;

public class ParceiroValidation : AbstractValidator<ParceiroCreateDto>
{

    public ParceiroValidation()
    {
        RuleFor((x => x.Cnpj)).NotNull().WithMessage("Campo CNPJ é obrigatório");
        RuleFor((x => x.RazaoSocial)).NotNull().WithMessage("Campo Razão Social é obrigatório");
        RuleFor((x => x.Fantasia)).NotNull().WithMessage("Campo Fantasia é obrigatório");
        RuleFor((x => x.Telefone1)).NotNull().WithMessage("Campo Telefone 1 é obrigatório");
        RuleFor((x => x.Email)).NotNull().WithMessage("Campo E-mail é obrigatório");
        RuleFor((x => x.DiasPagamento)).NotNull().WithMessage("Campo Dias Pagamento é obrigatório");
        RuleFor((x => x.PagaTrt)).IsInEnum().NotNull().WithMessage("Campo Parceiro Paga TRT é obrigatório");
        RuleFor((x => x.TabelasPreco)).NotNull().NotEmpty()
            .WithMessage("Necessário vincular ao menos uma Tabela de Preço ao parceiro.")
            ;
    }
}