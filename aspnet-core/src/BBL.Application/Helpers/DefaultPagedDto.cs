using Abp.Application.Services.Dto;

namespace BBL.Helpers;

public class DefaultPagedDto : PagedAndSortedResultRequestDto
{
    public string SearchText { get; set; }

    public DefaultPagedDto() : base()
    {
        MaxResultCount = 50;
    }
}