using BBL.Utils.ExceptionUtils;
using BBL.Utils.HttpUtils;
using Abp.AspNetCore.Configuration;
using Abp.AspNetCore.Mvc.ExceptionHandling;
using Abp.Web.Configuration;
using Abp.Web.Models;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace BBL.Exceptions
{
    public class GlobalExceptionFilter : AbpExceptionFilter
    {
        public GlobalExceptionFilter(IErrorInfoBuilder errorInfoBuilder,
            IAbpAspNetCoreConfiguration configuration,
            IAbpWebCommonModuleConfiguration abpWebCommonModuleConfiguration)
            : base(errorInfoBuilder, configuration, abpWebCommonModuleConfiguration)
        {
        }

        private bool TryGetSqlExceptionStatusCode(Exception innerException, out int statusCode)
        {
            statusCode = 0;
            if (innerException.TryGetSqlExceptionCode(out var sqlExceptionCode))
            {
                switch (sqlExceptionCode)
                {
                    case 547:
                        statusCode = HttpStatusCode.BadRequest.GetCode();
                        return true;
                }
            }

            return false;
        }

        protected override int GetStatusCode(ExceptionContext context, bool wrapOnError)
        {
            if (TryGetSqlExceptionStatusCode(context.Exception.InnerException, out var statusCode))
                return statusCode;

            return base.GetStatusCode(context, wrapOnError);
        }
    }
}
