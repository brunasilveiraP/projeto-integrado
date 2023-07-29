using Abp.Application.Services.Dto;
using System;

namespace BBL.Users.Dto
{
    //custom PagedResultRequestDto
    public class PagedUserResultRequestDto : PagedResultRequestDto
    {
        public string Keyword { get; set; }
        public bool? IsActive { get; set; }
        
        public string SearchText { get; set; }
    }
}
