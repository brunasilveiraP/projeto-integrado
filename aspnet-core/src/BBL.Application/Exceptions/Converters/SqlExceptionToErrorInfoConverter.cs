using BBL.Utils.ExceptionUtils;
using Abp.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BBL.Exceptions.Converters
{
    public class SqlExceptionToErrorInfoConverter : IExceptionToErrorInfoConverter
    {
        public IExceptionToErrorInfoConverter Next { private get; set; }

        public ErrorInfo Convert(Exception exception)
        {
            if (exception.InnerException.TryGetSqlExceptionCode(out var sqlExceptionCode))
            {
                return new ErrorInfo(sqlExceptionCode, exception.InnerException.Message);
            }

            return Next.Convert(exception);
        }
    }
}
