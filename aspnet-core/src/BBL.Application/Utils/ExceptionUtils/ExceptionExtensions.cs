using Microsoft.Data.SqlClient;
using System;

namespace BBL.Utils.ExceptionUtils
{
    public static class ExceptionExtensions
    {
        public static bool TryGetSqlExceptionCode(this Exception value, out int sqlExceptionCode)
        {
            sqlExceptionCode = 0;
            if (value is SqlException sqlException)
            {
                sqlExceptionCode = sqlException.Number;
                return true;
            }
            return false;
        }
    }
}
